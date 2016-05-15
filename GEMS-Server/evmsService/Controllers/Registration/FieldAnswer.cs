using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using evmsService.Controllers;

namespace evmsService.Controllers
{
    public class FieldAnswerController
    {
        public static void AddFieldAnswer(int FieldID, int ParticipantID, string Answer, DAL dalDataContext)
        {
            
            try
            {
                //Event evnt = EventController.GetEvent(EventID);
                //if(e == null)
                //    throw new FaultException<SException>(new SException(),
                //   new FaultReason("Invalid Event ID"));
                FieldAnswer fa = GetFieldAnswer(ParticipantID, FieldID);
                //DAL dalDataContext = new DAL();
                Table<FieldAnswer> fieldAnswers = dalDataContext.fieldAnswer;
                if (fa == null)
                {
                    FieldAnswer CreateFieldAns = new FieldAnswer(ParticipantID, FieldID, Answer);
                    fieldAnswers.InsertOnSubmit(CreateFieldAns);
                    fieldAnswers.Context.SubmitChanges();
                }
                else
                {
                    fa.Answer = Answer;
                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Field Answer, Please Try Again!"));
            }
        }
        public static FieldAnswer GetFieldAnswer(int participantID, int FieldID)
        {
            try
            {
                DAL dalDataContext = new DAL();
                FieldAnswer existingFieldAnswer = (from fieldAnswer in dalDataContext.fieldAnswer
                                                   where fieldAnswer.ParticipantID == participantID &&
                                                   fieldAnswer.FieldID == FieldID
                                                   select fieldAnswer).FirstOrDefault();
                return existingFieldAnswer;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Field Answer Data, Please Try Again!"));
            }
        }

        //public static FieldAnswer GetFieldAnswer(int participantID, int FieldID, DAL dalDataContext)
        //{

        //    try
        //    {
        //        //DAL dalDataContext = new DAL();

        //        FieldAnswer existingFieldAnswer = (from fieldAnswer in dalDataContext.fieldAnswer
        //                                           where fieldAnswer.ParticipantID == participantID &&
        //                                           fieldAnswer.FieldID == FieldID
        //                                           select fieldAnswer).FirstOrDefault();

        //        if (existingFieldAnswer == null)
        //        {
        //            throw new FaultException<SException>(new SException(),
        //               new FaultReason("Invalid Field Answer"));
        //        }
        //        return existingFieldAnswer;
        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //               new FaultReason("An Error occured While Retrieving Field Answer Data, Please Try Again!"));
        //    }
        //}

        public static void DeleteFieldAnswer(User user, int participantID, int FieldID)
        {
            //if (!u.isAuthorized(u, EventController.GetEvent(FieldController.GetField(FieldID).EventID), EnumFunctions.Add_Guest))
            //    throw new FaultException<SException>(new SException(),
            //       new FaultReason("Invalid User, User Does Not Have Rights To Delete Field Answer!"));
            //chk if user can do this anot

            DAL dalDataContext = new DAL();
            try
            {
                FieldAnswer fa = GetFieldAnswer(participantID, FieldID);
                dalDataContext.fieldAnswer.DeleteOnSubmit(fa);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Field Answer, Please Try Again!"));
            }
        }
        
        public static List<FieldAnswer> ViewFieldAnswer(int participantID)
        {
            try
            {
                DAL dalDataContext = new DAL();
                List<FieldAnswer> fas = (from fieldAnswers in dalDataContext.fieldAnswer
                                         where fieldAnswers.ParticipantID == participantID
                                         select fieldAnswers).ToList<FieldAnswer>();
                return fas;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Field Answer Data, Please Try Again!"));
            }
        }

        public static List<FieldAnswer> ViewFieldAnswer(User user, int participantID, int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();
                List<FieldAnswer> fas = (from fieldAnswers in dalDataContext.fieldAnswer
                                        join field in dalDataContext.fields on
                                        fieldAnswers.FieldID equals field.FieldID
                                        where fieldAnswers.ParticipantID == participantID 
                                        && field.EventID == EventID
                                        select fieldAnswers) .ToList<FieldAnswer>();
                return fas;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Field Answer Data, Please Try Again!"));
            }
        }
    }
}