using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class DayController
    {
        public static void AddDay(int EventID, int EventDay)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<EventDay> days = dalDataContext.days;

                EventDay creatingday = new EventDay(EventID, EventDay);

                days.InsertOnSubmit(creatingday);

                days.Context.SubmitChanges();

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Day, Please Try Again!"));
            }

        }

        public static EventDay AddDay(int EventID, int EventDay, DAL dalDataContext)
        {
            try
            {

                Table<EventDay> days = dalDataContext.days;

                EventDay creatingday = new EventDay(EventID, EventDay);

                days.InsertOnSubmit(creatingday);

                days.Context.SubmitChanges();

                return creatingday;

            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Day, Please Try Again!"));
            }

        }
        public static void DeleteDay(EventDay d)
        {
            //chk if user can do this anot

            DAL dalDataContext = new DAL();

            try
            {
                EventDay matchedday = (from days in dalDataContext.days
                                  where days.DayID == d.DayID
                                  select days).FirstOrDefault();


                dalDataContext.days.DeleteOnSubmit(matchedday);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Deleting Day, Please Try Again!"));
            }
        }

        public static int GetEventID(Program p)
        {
            try
            {
                DAL dalDataContext = new DAL();

                EventDay matchday = (from day in dalDataContext.days
                                where day.DayID == p.DayID
                                select day).FirstOrDefault();
                if (matchday == null)
                {
                    throw new Exception("day doesnt not exist");
                }
                return matchday.EventID;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While getting event, Please Try Again!"));
            }
        }

        public static int GetEventID(Guest g)
        {
            try
            {
                DAL dalDataContext = new DAL();

                EventDay matchday = (from day in dalDataContext.days
                                where day.DayID == g.DayID
                                select day).FirstOrDefault();
                if (matchday == null)
                {
                    throw new Exception("day doesnt not exist");
                }
                return matchday.EventID;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While getting event, Please Try Again!"));
            }
        }

        public static void DeleteDay(EventDay d, DAL dalDataContext)
        {

            try
            {
                EventDay matchedday = (from days in dalDataContext.days
                                  where days.DayID == d.DayID
                                  select days).FirstOrDefault();


                dalDataContext.days.DeleteOnSubmit(matchedday);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Deleting Day, Please Try Again!"));
            }
        }

        public static EventDay GetDay(int DayID)
        {
            DAL dalDataContext = new DAL();

            EventDay existingDay = (from days in dalDataContext.days
                               where days.DayID == DayID
                               select days).FirstOrDefault();

            if (existingDay == null)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Day!"));

            return existingDay;
        }

        public static List<EventDay> GetDays(int EventID)
        {
            DAL dalDataContext = new DAL();

            List<EventDay> existingDays = (from days in dalDataContext.days
                                      where days.EventID == EventID
                                      select days).ToList<EventDay>();

            return existingDays.OrderBy(s => s.DayNumber).ToList<EventDay>();
        }

        public static void DeleteDays(int EventID)
        {
            //chk if user can do this anot


            DAL dalDataContext = new DAL();

            try
            {
                List<EventDay> matchedDays = (from days in dalDataContext.days
                                         where days.EventID == EventID

                                         select days).ToList<EventDay>();


                dalDataContext.days.DeleteAllOnSubmit(matchedDays);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Days Please Try Again!"));
            }
        }

        public static void EditDay(int DayID, int DayNumber)
        {

            DAL dalDataContext = new DAL();

            EventDay matchedDay = (from day in dalDataContext.days
                              where day.DayID == DayID
                              select day).FirstOrDefault();

            if (matchedDay == null)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Day"));
            }
            else
            {

                matchedDay.DayNumber = DayNumber;
                dalDataContext.SubmitChanges();


            }
        }
        public static void EditDay(int DayID, int DayNumber, DAL dalDataContext)
        {

            //DAL dalDataContext = new DAL();

            EventDay matchedDay = (from day in dalDataContext.days
                              where day.DayID == DayID
                              select day).FirstOrDefault();

            if (matchedDay == null)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Day"));
            }
            else
            {

                matchedDay.DayNumber = DayNumber;
                dalDataContext.SubmitChanges();


            }
        }

    }
}