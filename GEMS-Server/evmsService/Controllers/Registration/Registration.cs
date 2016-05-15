using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using System.Transactions;
using System.Runtime.Serialization;

namespace evmsService.Controllers
{
    [DataContract]
    public class QuestionIDWithAnswer
    {
        [DataMember]
        public int QuestionID;
        [DataMember]
        public string Answer;

        public QuestionIDWithAnswer(int questionID, string answer)
        {
            QuestionID = questionID;
            Answer = answer;
        }
    }
    public class Registration
    {
        public static List<ParticipantWithName> ViewEventParticipantWithName(User user, int EventID)
        {
            if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Participant))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Manage Participant!"));

            return ParticipantController.ViewParticipantWithName(EventID);
        }

        public static void RegisterParticipant(int EventID, List<QuestionIDWithAnswer> answers)
        {

            Publish publish = PublishController.GetPublish(EventID);

            if(publish == null)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Sorry But The Event Have Not Been Published Yet, Hence You Cannot Register"));

            if(DateTime.Now.CompareTo(publish.StartDateTime) < 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Sorry but this Event's Registration Have Not Started"));
            if (DateTime.Now.CompareTo(publish.EndDateTime) > 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Sorry but this Event's Registration is Already Over"));

            List<Field> eventsfields = FieldController.ViewField(EventID);

           
            try
            {
                
                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL dalDataContext = new DAL();

                    int participantID = ParticipantController.AddParticipant(EventID,  dalDataContext, publish.IsPayable);
                    

                    for (int i = 0; i < eventsfields.Count; i++)
                    {
                        for (int j = 0; j < answers.Count; j++)
                        {

                            if (eventsfields[i].FieldID == answers[j].QuestionID)
                            {
                                FieldAnswerController.AddFieldAnswer(eventsfields[i].FieldID, participantID, answers[j].Answer, dalDataContext);
                                goto cont;
                            }
                        }

                        throw new FaultException<SException>(new SException(), new FaultReason("An Error Occured Invalid Entry"));

                    cont:
                        continue;
                    }

                        t.Complete();
                }

            

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Participants, Please Try Again!"));
            }
        }


        public static void DeleteParticipant(User user, int EventID, int participantID)
        {
            ParticipantController.DeleteParticipant(user, participantID);
        }

        public static List<Participant> ViewEventParticipant(User user, int EventID)
        {
            if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Participant))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Manage Participant!"));

            return ParticipantController.ViewParticipant(EventID);
        }

        public static List<FieldAnswer> GetParticipantFieldAnswer(User user, int EventID, int ParticipantID)
        {
            if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Participant))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Manage Participant!"));

           return FieldAnswerController.ViewFieldAnswer(ParticipantID);
        }
       
    }
}