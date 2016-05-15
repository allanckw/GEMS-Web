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
    public class FieldController
    {

        //public static void AddField(User user, int EventID, string FieldLabel, string FieldName, string Remarks, bool IsRequired)
        //{
        //    if (!u.isAuthorized(EventController.GetEvent(EventID)))
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("Invalid User, User Does Not Have Rights To Add Field!"));
        //    try
        //    {
                
        //            //Event evnt = EventController.GetEvent(EventID);
        //            //if(e == null)
        //            //    throw new FaultException<SException>(new SException(),
        //            //   new FaultReason("Invalid Event ID"));
               
        //            DAL dalDataContext = new DAL();
        //            Table<Field> fields = dalDataContext.fields;
        //            Field creatingField = new Field(Remarks, FieldLabel, FieldName, EventID, IsRequired);

        //            fields.InsertOnSubmit(creatingField);
        //            fields.Context.SubmitChanges();
        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("An Error occured While Adding New Field, Please Try Again!"));
        //    }
        //}

        public static void AddField(DAL dalDataContext, int EventID, Field incField)
        {
            
            try
            {
                Table<Field> fields = dalDataContext.fields;
                Field creatingField = new Field(incField.Remarks, incField.FieldLabel, incField.FieldName, EventID, incField.IsRequired);

                fields.InsertOnSubmit(creatingField);
                
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Field, Please Try Again!"));
            }
        }
        public static void AddDefaultFeids(int EventID, DAL dalDataContext)
        {
            List<Field> ListField = new List<Field>();


            Field firstName = new Field();
            firstName.FieldName = firstName.FieldLabel = "First Name";
            Field lastName = new Field();
            lastName.FieldName = lastName.FieldLabel = "Last Name";
            firstName.IsRequired = lastName.IsRequired = true;
            Field email = new Field();
            email.FieldName = email.FieldLabel = "Email";
            email.IsRequired = email.IsRequired = true;

            ListField.Add(firstName);
            ListField.Add(lastName);
            ListField.Add(email);

            try
            {
               
                
                    for (int i = 0; i < ListField.Count; i++)
                    {
                        AddField(dalDataContext, EventID, ListField[i]);
                    }

                    dalDataContext.SubmitChanges();
                    
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Field, Please Try Again!"));
            }

        }

        public static void AddField(User user, int EventID, List<Field> ListField)
        {
            if (!user.isAuthorized(EventController.GetEvent(EventID)))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Manage Field!"));
            try
            {
                //delete all first related to this event

                List<Field> lf = ViewField(EventID);
                
                //Event evnt = EventController.GetEvent(EventID);
                //if(e == null)
                //    throw new FaultException<SException>(new SException(),
                //   new FaultReason("Invalid Event ID"));

                DAL dalDataContext = new DAL();


                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {

                    ParticipantController.DeleteAllParticipant(EventID, dalDataContext);
                    for(int i=0;i<lf.Count;i++)
                    {
                        DeleteField(lf[i].FieldID, dalDataContext);
                    }
                    for (int i = 0; i < ListField.Count; i++)
                    {
                        AddField(dalDataContext, EventID, ListField[i]);
                    }

                    dalDataContext.SubmitChanges();
                    t.Complete();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Field, Please Try Again!"));
            }
        }

        private static void DeleteFieldAnswerDependency(DAL dalDataContext, int FieldID)
        {
            try
            {
                List<FieldAnswer> fas = (from fieldAnswers in dalDataContext.fieldAnswer
                                        where fieldAnswers.FieldID == FieldID
                                        select fieldAnswers).ToList<FieldAnswer>();

                dalDataContext.fieldAnswer.DeleteAllOnSubmit<FieldAnswer>(fas);
                dalDataContext.SubmitChanges();
                
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured while deleting Field Dependency!"));
            }
        }

        public static Field GetField(int FieldID)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Field existingField = (from field in dalDataContext.fields
                                       where field.FieldID == FieldID
                                       select field).FirstOrDefault();

                if (existingField == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Field"));
                }
                return existingField;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Field Data, Please Try Again!"));
            }
        }

        public static void DeleteField(int FieldID, DAL dalDataContext)
        {
            //chk if user can do this anot


            //if (!u.isAuthorized(EventController.GetEvent(GetField(FieldID).EventID)))
            //    throw new FaultException<SException>(new SException(),
            //       new FaultReason("Invalid User, User Does Not Have Rights To Delete Field!"));

            //DAL dalDataContext = new DAL();

            try
            {
                //using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                //{
                    Field fa = (from field in dalDataContext.fields
                                where field.FieldID == FieldID
                                select field).FirstOrDefault();

                    dalDataContext.fields.DeleteOnSubmit(fa);
                    
                    DeleteFieldAnswerDependency(dalDataContext, FieldID);

                //    t.Complete();
                //}
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Field, Please Try Again!"));
            }
        }

        public static List<Field> ViewField(int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Field> fs = (from fields in dalDataContext.fields
                                  where fields.EventID.Equals(EventID)
                                  orderby fields.FieldID
                                         select fields).ToList<Field>();
                return fs;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Field Data, Please Try Again!"));
            }
        }

        public static Field ViewField(int EventID, string fieldname)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Field fs = (from fields in dalDataContext.fields
                                  where fields.EventID.Equals(EventID)
                                  && fields.FieldName == fieldname
                                  orderby fields.FieldID
                                  select fields).FirstOrDefault<Field>();
                return fs;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Field Data, Please Try Again!"));
            }
        }
    }
}