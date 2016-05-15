using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using System.Runtime.Serialization;

namespace evmsService.Controllers
{
    public class ParticipantController
    {
        public static int AddParticipant(int EventID, DAL dalDataContext, bool isPayable)
        {
            //if (!u.isAuthorized(u, EventController.GetEvent(EventID), EnumFunctions.Add_Guest))
            //    throw new FaultException<SException>(new SException(),
            //       new FaultReason("Invalid User, User Does Not Have Rights To Add Participant!"));
            try
            {
                //DAL dalDataContext = new DAL();
                Table<Participant> participants = dalDataContext.participants;
                Participant creatingParticipant = new Participant(EventID);
                creatingParticipant.Paid = !isPayable;

                participants.InsertOnSubmit(creatingParticipant);
                participants.Context.SubmitChanges();

                return creatingParticipant.ParticipantID;

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Participant, Please Try Again!"));
            }
        }

        public static bool EventRegistered(string Email, int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();
                FieldAnswer existingFieldAnswer = (from fieldAnswer in dalDataContext.fieldAnswer
                                                   join field in dalDataContext.fields
                                                   on fieldAnswer.FieldID equals field.FieldID
                                                   where fieldAnswer.Answer == Email &&
                                                   field.FieldName == "Email" &&
                                                   field.EventID == EventID
                                                   select fieldAnswer).FirstOrDefault();
                if (existingFieldAnswer == null)
                    return false;
                return true;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured while Checking if Email is Registered for this email"));
            }
        }

        public static bool isRegistered(string Email)
        {
            try
            {
                DAL dalDataContext = new DAL();
                FieldAnswer existingFieldAnswer = (from fieldAnswer in dalDataContext.fieldAnswer
                                                   join field in dalDataContext.fields
                                                   on fieldAnswer.FieldID equals field.FieldID
                                                   where fieldAnswer.Answer == Email &&
                                                   field.FieldName == "Email"
                                                   select fieldAnswer).FirstOrDefault();
                if (existingFieldAnswer == null)
                    return false;
                return true;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured while Checking if Email is Registered"));
            }
        }

        public static void SetPaid(string Email, int eventID)
        {
            Participant p = GetParticipant(eventID, Email);


            DAL dalDataContext = new DAL();


            p = (from participant in dalDataContext.participants

                 where participant.ParticipantID == p.ParticipantID

                 select participant).SingleOrDefault();

            p.Paid = true;

            dalDataContext.SubmitChanges();

        }

        public static bool isParticipantPaid(int EventID)
        {
            //if (!u.isAuthorized(u, EventController.GetEvent(EventID), EnumFunctions.Add_Guest))
            //    throw new FaultException<SException>(new SException(),
            //       new FaultReason("Invalid User, User Does Not Have Rights To Add Participant!"));
            try
            {
                //DAL dalDataContext = new DAL();
                List<Participant> participants = ViewParticipant(EventID);

                for (int i = 0; i < participants.Count; i++)
                {
                    if (participants[i].Paid == true)
                        return true;
                }
                return false;

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Checking Participant information, Please Try Again!"));
            }
        }

        public static Participant GetParticipant(int ParticipantID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Participant existingParticipant = (from participant in dalDataContext.participants
                                                   where participant.ParticipantID == ParticipantID
                                                   select participant).FirstOrDefault();

                if (existingParticipant == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Participant"));
                }
                return existingParticipant;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Participant Data, Please Try Again!"));
            }
        }

        public static Participant GetParticipant(int EventID, string participantEmail)
        {
            try
            {
                Field f = FieldController.ViewField(EventID, "Email");

                if (f == null)
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Fields Detected, System may have been edited illegally, pleae contact admin"));
                DAL dalDataContext = new DAL();

                Participant existingParticipant = (from participant in dalDataContext.participants
                                                   join ans in dalDataContext.fieldAnswer
                                                   on participant.ParticipantID equals ans.ParticipantID
                                                   where participant.EventID == EventID
                                                   && ans.FieldID == f.FieldID
                                                   && ans.Answer == participantEmail
                                                   select participant).FirstOrDefault();

                //if (existingParticipant == null)
                //{
                //    throw new FaultException<SException>(new SException(),
                //       new FaultReason("Invalid Participant"));
                //}
                return existingParticipant;
            }
            catch (Exception ex)
            {

                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Participant Data, Please Try Again!" + ex.Message));
            }
        }

        public static Participant EditParticipant(int ParticipantID, bool paid)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();

                Participant existingParticipant = (from participant in dalDataContext.participants
                                                   where participant.ParticipantID == ParticipantID
                                                   select participant).FirstOrDefault();

                if (existingParticipant == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Participant"));
                }


                existingParticipant.Paid = paid;

                dalDataContext.SubmitChanges();

                return existingParticipant;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Participant, Please Try Again!"));
            }




        }

        public static void DeleteAllParticipant(int EventID, DAL dalDataContext)
        {
            //chk if user can do this anot
            try
            {
                // Participant p = GetParticipant(ParticipantID);
                List<Participant> participants = (from participant in dalDataContext.participants
                                                  where participant.EventID == EventID
                                                  select participant).ToList<Participant>();


                for (int i = 0; i < participants.Count; i++)
                {
                    dalDataContext.participants.DeleteOnSubmit(participants[i]);
                }

                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Clearing Participant, Please Try Again!"));
            }


            return;

        }

        public static void DeleteParticipant(User user, int ParticipantID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                // Participant p = GetParticipant(ParticipantID);
                Participant p = (from participants in dalDataContext.participants
                                 where participants.ParticipantID == ParticipantID
                                 select participants).SingleOrDefault<Participant>();
                //chk if user can do this anot
                if (!user.isAuthorized(EventController.GetEvent(p.EventID), EnumFunctions.Manage_Participant))
                    goto Error;

                dalDataContext.participants.DeleteOnSubmit(p);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Participant, Please Try Again!"));
            }


            return;

        Error:
            throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Delete Participant!"));
        }

        public static List<Participant> ViewParticipant(int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Participant> Participants = (from participants in dalDataContext.participants
                                                  where participants.EventID == EventID
                                                  select participants).ToList<Participant>();
                return Participants;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Participants Data, Please Try Again!"));
            }
        }

        public static List<ParticipantWithName> ViewParticipantWithName(int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Participant> Participants = (from participants in dalDataContext.participants
                                                  where participants.EventID == EventID
                                                  select participants).ToList<Participant>();

                List<string> ParticipantFirstNames = (from fieldAnswers in dalDataContext.fieldAnswer
                                                      join field in dalDataContext.fields on
                                                      fieldAnswers.FieldID equals field.FieldID
                                                      where field.FieldName == "First Name"
                                                      && field.EventID == EventID
                                                      orderby fieldAnswers.ParticipantID
                                                      select fieldAnswers.Answer).ToList<string>();

                List<string> ParticipantLastNames = (from fieldAnswers in dalDataContext.fieldAnswer
                                                     join field in dalDataContext.fields on
                                                     fieldAnswers.FieldID equals field.FieldID
                                                     where field.FieldName == "Last Name"
                                                     && field.EventID == EventID
                                                     orderby fieldAnswers.ParticipantID
                                                     select fieldAnswers.Answer).ToList<string>();

                List<ParticipantWithName> participantsWithNames = new List<ParticipantWithName>();
                for (int i = 0; i < Participants.Count; i++)
                    participantsWithNames.Add(new ParticipantWithName(Participants[i], ParticipantFirstNames[i] + " " + ParticipantLastNames[i]));

                return participantsWithNames;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured while Retrieving Participants Data, Please Try Again!"));
            }
        }

        public static void SaveTransaction(string transID, List<ParticipantTransaction> trans)
        {
            DAL dalDataContext = new DAL();
            Table<ParticipantTransaction> partiTrans = dalDataContext.partiTrans;

            if (!isTransExist(transID))
            {
                try
                {
                    partiTrans.InsertAllOnSubmit(trans);
                    partiTrans.Context.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured while saving transaction, please notify the Administrator immediately!"));
                }
                foreach (ParticipantTransaction pt in trans)
                {
                    SetPaid(pt.Email, pt.EventID);
                }
            }
            else
            {
                //duplicate transaction ID insertion.. do nth 
            }
        }

        public static List<ParticipantTransaction> ViewParticipantTransactions(string email, DateTime fromDate, DateTime toDate)
        {
            DAL dalDataContext = new DAL();

            List<ParticipantTransaction> trans = (from transaction in dalDataContext.partiTrans
                                                  where
                                                  transaction.Email.ToLower() == email.ToLower() &&
                                                  transaction.TransactionDateTime.Date >= fromDate &&
                                                  transaction.TransactionDateTime.Date <= toDate
                                                  select transaction).ToList<ParticipantTransaction>();

            return trans;

        }

        public static decimal GetEventParticipantIncome(int eventID)
        {
            DAL dalDataContext = new DAL();
            List<ParticipantTransaction> trans = (from transaction in dalDataContext.partiTrans
                                                  where transaction.EventID.Equals(eventID)
                                                  select transaction).ToList<ParticipantTransaction>();
            decimal total = 0;
            foreach(ParticipantTransaction tran in trans)
            {
                total += tran.Amount;
            }

            return total;
        }

        private static bool isTransExist(string transactionID)
        {
            DAL dalDataContext = new DAL();
            Table<ParticipantTransaction> partiTrans = dalDataContext.partiTrans;

            try
            {
                ParticipantTransaction trans = (from transaction in dalDataContext.partiTrans
                                                where transaction.TransactionID == transactionID
                                                select transaction).FirstOrDefault();

                return (trans != null);
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured while checking duplicate transaction id"));
            }

        }

        public static List<ParticipantTransaction> ViewTransactions(string email)
        {
            return new List<ParticipantTransaction>();
        }

    }


}