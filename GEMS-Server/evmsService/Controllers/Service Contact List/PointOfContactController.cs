using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class PointOfContactController
    {

        public static void AddPointOfContact(User user, int EventID, int serviceID, string name, string position, string phone, string email)
        {
            bool allow = false;
            if (user.isSystemAdmin || user.isEventOrganizer)
            {
                allow = true;
            }

            if (!allow)
            {
                if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Items))
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Edit this Service!"));

            }
            try
            {
                DAL dalDataContext = new DAL();
                Table<PointOfContact> pointOfContact = dalDataContext.pointOfContacts;
                PointOfContact creatingPointOfContact = new PointOfContact(serviceID, name, position, phone, email);

                pointOfContact.InsertOnSubmit(creatingPointOfContact);
                pointOfContact.Context.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Point of Contact, Please Try Again!"));
            }
        }

        public static void EditPointOfContact(User user, int EventID, int PointOfContactID, string name, string position, string phone, string email)
        {
            bool allow = false;
            if (user.isSystemAdmin || user.isEventOrganizer)
            {
                allow = true;
            }

            if (!allow)
            {
                if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Items))
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Edit this Service!"));

            }
            try
            {
                DAL dalDataContext = new DAL();

                PointOfContact pointOfContact = (from poc in dalDataContext.pointOfContacts
                                                 where poc.PointOfContactID == PointOfContactID
                                                 select poc).FirstOrDefault();
                if (pointOfContact == null)
                {

                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Point Of Point Of Contact"));
                }
                else
                {
                    pointOfContact.Name = name;
                    pointOfContact.Position = position;
                    pointOfContact.Phone = phone;
                    pointOfContact.Email = email;
                    dalDataContext.SubmitChanges();

                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Point Of Contact, Please Try Again!"));
            }
        }

        public static PointOfContact GetPointOfContact(int PointOfContactID)
        {

            try
            {
                DAL dalDataContext = new DAL();

                PointOfContact existingPointOfContact = (from pointOfContact in dalDataContext.pointOfContacts
                                                         where pointOfContact.PointOfContactID == PointOfContactID
                                                         select pointOfContact).FirstOrDefault();

                if (existingPointOfContact == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Point of Contact"));
                }
                return existingPointOfContact;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Point of Contact Data, Please Try Again!"));
            }
        }

        public static void DeletePointOfContact(User user, int PointOfContactID)
        {
            if (!user.isSystemAdmin && !user.isEventOrganizer)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Point Of Contact!"));
            //chk if user can do this anot


            DAL dalDataContext = new DAL();

            try
            {
                PointOfContact poc = (from pointOfContact in dalDataContext.pointOfContacts
                                      where pointOfContact.PointOfContactID == PointOfContactID
                                      select pointOfContact).FirstOrDefault();

                dalDataContext.pointOfContacts.DeleteOnSubmit(poc);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Point Of Contact, Please Try Again!"));
            }
        }

        public static List<PointOfContact> ViewPointOfContact( int serviceID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<PointOfContact> pocs = (from pointofcontacts in dalDataContext.pointOfContacts
                                             where pointofcontacts.ServiceID == serviceID
                                             select pointofcontacts).ToList<PointOfContact>();

                return pocs;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Field Answer Data, Please Try Again!"));
            }
        }


    }
}