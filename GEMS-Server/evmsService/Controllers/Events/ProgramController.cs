using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using System.Transactions;

namespace evmsService.Controllers
{
    public class ProgramController
    {
        public static List<int> GetEventProgCount(int eventID)
        {
            DAL dalDataContext = new DAL();

            Events ev = EventController.GetEvent(eventID);

            List<int> progCountList = new List<int>();

            foreach (int dayid in ev.DaysID)
            {
                List<Program> progList = (from progs in dalDataContext.programs
                                          where progs.DayID == dayid
                                          select progs).ToList<Program>();

                progCountList.Add(progList.Count);
            }
            return progCountList;
        }


        public static void AddProgram(string ProgramName, DateTime ProgramStartDateTime, DateTime ProgramEndDatetime,
        string ProgramDescription, int ProgramDayID, string ProgramLocation, DAL dalDataContext)
        {
            try
            {
                Table<Program> programs = dalDataContext.programs;
                Program newProgram = new Program(ProgramName, ProgramStartDateTime, ProgramEndDatetime, ProgramDescription, ProgramDayID, ProgramLocation);
                programs.InsertOnSubmit(newProgram);
                programs.Context.SubmitChanges();

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Program, Please Try Again!"));
            }
        }

        public static int AddProgram(User user, string ProgramName, DateTime ProgramStartDateTime, DateTime ProgramEndDatetime,
        string ProgramDescription, int ProgramDayID, string ProgramLocation)
        {

            if (!user.isAuthorized(EventController.GetEvent(DayController.GetDay(ProgramDayID).EventID), EnumFunctions.Create_Programmes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Programs!"));

            try
            {
                DAL dalDataContext = new DAL();
                Table<Program> programs = dalDataContext.programs;
                Program newProgram = new Program(ProgramName, ProgramStartDateTime, ProgramEndDatetime, ProgramDescription, ProgramDayID, ProgramLocation);
                programs.InsertOnSubmit(newProgram);
                programs.Context.SubmitChanges();

                return newProgram.ProgramID;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Program, Please Try Again!"));
            }

        }

        public static Program GetPrograms(int ProgramID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Program existingProgram = (from programs in dalDataContext.programs
                                           where programs.ProgramID == ProgramID
                                           select programs).FirstOrDefault();

                if (existingProgram == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Program"));
                }
                return existingProgram;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Program Data, Please Try Again!"));
            }
        }

        public static void DeleteProgram(User user, int ProgramID)
        {
            //chk if user got rights or is organizer

            Program P = GetPrograms(ProgramID);

            if (!user.isAuthorized(EventController.GetEvent(P.EventID), EnumFunctions.Delete_Programmes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Programs!"));

            DAL dalDataContext = new DAL();
            try
            {
                Program matchedprograms = (from programs in dalDataContext.programs
                                           where programs.ProgramID == P.ProgramID
                                           select programs).FirstOrDefault();


                dalDataContext.programs.DeleteOnSubmit(matchedprograms);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Adding Deleting Program, Please Try Again!"));
                //throw exception here
            }
        }

        public static void DeleteProgram(int ProgramID, DAL dalDataContext)
        {
            //chk if user got rights or is organizer

            Program P = GetPrograms(ProgramID);

          
            
            try
            {
                Program matchedprograms = (from programs in dalDataContext.programs
                                           where programs.ProgramID == P.ProgramID
                                           select programs).FirstOrDefault();


                dalDataContext.programs.DeleteOnSubmit(matchedprograms);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Adding Deleting Program, Please Try Again!"));
                //throw exception here
            }
        }

        public static void EditProgram(User user, int ProgramID, string ProgramName, DateTime ProgramStartDateTime
        , DateTime ProgramEndDatetime, string ProgramDescription, string ProgramLocation)
        {

            Program prog = GetPrograms(ProgramID);

            if (!user.isAuthorized(EventController.GetEvent(prog.EventID), EnumFunctions.Edit_Programmes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit this Program!"));
            using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
            {

                bool clash = ValidateProgramTime(prog.DayID, ProgramStartDateTime,
                    ProgramEndDatetime, prog.ProgramID);

                if (clash)
                {
                    throw new FaultException<SException>(new SException(),
                            new FaultReason("There is a clash with an existing program!"));
                }

                DAL dalDataContext = new DAL();

                try
                {
                    var matchedprograms = (from programs in dalDataContext.programs
                                           where programs.ProgramID == prog.ProgramID
                                           select programs).FirstOrDefault();

                    matchedprograms.Name = ProgramName;

                    matchedprograms.StartDateTime = ProgramStartDateTime;
                    matchedprograms.EndDateTime = ProgramEndDatetime;
                    matchedprograms.Description = ProgramDescription;
                    matchedprograms.Location = ProgramLocation;

                    dalDataContext.SubmitChanges();

                }
                catch
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Adding Editing Program, Please Try Again!"));

                }
            }
        }

        public static void SwapProgram(User user, int ProgramID1, int ProgramID2)
        {

            Program prog1 = GetPrograms(ProgramID1);
            Program prog2 = GetPrograms(ProgramID2);
            
            if (!user.isAuthorized(EventController.GetEvent(prog1.EventID), EnumFunctions.Edit_Programmes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit this Program!"));

            

            try
            {
                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL dalDataContext = new DAL();

                    DeleteProgram(prog1.ProgramID,dalDataContext);
                    DeleteProgram(prog2.ProgramID,dalDataContext);
            
                    
                    bool clash = ValidateProgramTime(prog1.DayID, prog2.StartDateTime,
                   prog2.EndDateTime, dalDataContext);

            

                    if (clash)
                    {
                        throw new FaultException<SException>(new SException(),
                                new FaultReason("There is a clash with an existing program!"));
                    }
                    AddProgram(prog1.Name,prog2.StartDateTime,prog2.EndDateTime,prog1.Description,prog1.DayID,prog1.Location,dalDataContext);


                    clash = ValidateProgramTime(prog1.DayID, prog1.StartDateTime,
                   prog1.EndDateTime, dalDataContext);

            

                    if (clash)
                    {
                        throw new FaultException<SException>(new SException(),
                                new FaultReason("There is a clash with an existing program!"));
                    }
                    AddProgram(prog2.Name,prog1.StartDateTime,prog1.EndDateTime,prog2.Description,prog2.DayID,prog2.Location,dalDataContext);

                    

                    dalDataContext.SubmitChanges();
                    t.Complete();
                }

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding Editing Program, Please Try Again!"));

            }

        }


        public static List<Program> ViewProgram(int dayID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Program> existingPrograms = (from programs in dalDataContext.programs
                                                  where programs.DayID == dayID
                                                  select programs).ToList<Program>();
                return existingPrograms;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason("An Error occured While Getting the Program data, Please Try Again!"));
            }
        }

        public static bool ValidateProgramTime(int dayID, DateTime segmentStart,
            DateTime segmentEnd, DAL dalDataContext, int progID = -1)
        {
            EventDay selectedday = DayController.GetDay(dayID);

            DateTime date = selectedday.DayDate;
            DateTime start = date, end = start.AddDays(1);

            Events progEvent = EventController.GetEvent(selectedday.EventID);

            if (date.Equals(progEvent.StartDateTime.Date))
                start = progEvent.StartDateTime;
            if (date.Equals(progEvent.EndDateTime.Date))
                end = progEvent.EndDateTime;
            if (segmentStart < start)
            {
                throw new FaultException<SException>(new SException("Invalid Segment Start DateTime"),
                  new FaultReason("Event starts at " + progEvent.StartDateTime +
                      ", programme segment must start after that."));

            }
            if (segmentEnd > end)
            {
                throw new FaultException<SException>(new SException("Invalid Segment End DateTime"),
                  new FaultReason("Event ends at " + progEvent.EndDateTime +
                      ", programme segment must end before that."));

            }
            if (segmentEnd <= segmentStart)
            {
                throw new FaultException<SException>(new SException("Invalid Segment Period"),
                  new FaultReason("Programme segment's end time must be after its start time."));
            }

            
            List<Program> existingPrograms;

            // Get all the programs
            existingPrograms = (from programs in dalDataContext.programs
                                where programs.DayID == dayID
                                orderby programs.StartDateTime descending
                                select programs).ToList<Program>();

            RequestClashingChecker checker = new RequestClashingChecker(segmentStart);


            foreach (Program prog in existingPrograms)
            {
                if (prog.ProgramID != progID)
                    checker.SetTimeSlotTaken(prog.StartDateTime, prog.EndDateTime);
            }

            return checker.HaveClash(segmentStart, segmentEnd);

        }

        public static bool ValidateProgramTime(int dayID, DateTime segmentStart,
            DateTime segmentEnd, int progID = -1)
        {
            EventDay selectedday = DayController.GetDay(dayID);

            DateTime date = selectedday.DayDate;
            DateTime start = date, end = start.AddDays(1);

            Events progEvent = EventController.GetEvent(selectedday.EventID);

            if (date.Equals(progEvent.StartDateTime.Date))
                start = progEvent.StartDateTime;
            if (date.Equals(progEvent.EndDateTime.Date))
                end = progEvent.EndDateTime;
            if (segmentStart < start)
            {
                throw new FaultException<SException>(new SException("Invalid Segment Start DateTime"),
                  new FaultReason("Event starts at " + progEvent.StartDateTime +
                      ", programme segment must start after that."));

            }
            if (segmentEnd > end)
            {
                throw new FaultException<SException>(new SException("Invalid Segment End DateTime"),
                  new FaultReason("Event ends at " + progEvent.EndDateTime +
                      ", programme segment must end before that."));

            }
            if (segmentEnd <= segmentStart)
            {
                throw new FaultException<SException>(new SException("Invalid Segment Period"),
                  new FaultReason("Programme segment's end time must be after its start time."));
            }

            DAL dalDataContext = new DAL();
            List<Program> existingPrograms;

            // Get all the programs
            existingPrograms = (from programs in dalDataContext.programs
                                where programs.DayID == dayID
                                orderby programs.StartDateTime descending
                                select programs).ToList<Program>();

            RequestClashingChecker checker = new RequestClashingChecker(segmentStart);


            foreach (Program prog in existingPrograms)
            {
                if (prog.ProgramID != progID)
                    checker.SetTimeSlotTaken(prog.StartDateTime, prog.EndDateTime);
            }

            return checker.HaveClash(segmentStart, segmentEnd);

        }
    }
}