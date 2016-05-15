using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using System.Data.Linq.SqlClient;

namespace evmsService.Controllers
{
    public class ServiceController
    {
        public static void AddService(User user, int EventID, string Address, string name, string url, string notes)
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
                Table<Service> services = dalDataContext.services;

                Service creatingService = new Service(Address, name, url, notes);

                services.InsertOnSubmit(creatingService);
                services.Context.SubmitChanges();
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Service, Please Try Again!"));
            }
        }

        public static void EditService(int ServiceID, User user, int EventID, string Address, string name, string url, string notes)
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

                Service s = (from service in dalDataContext.services
                             where service.ServiceID == ServiceID
                             select service).FirstOrDefault();

                if (s == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Service"));
                }
                else
                {
                    s.Address = Address;
                    s.Name = name;
                    s.Url = url;
                    s.Notes = notes;
                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Static Field, Please Try Again!"));
            }
        }

        public static Service GetService(int serviceID)
        {

            try
            {
                DAL dalDataContext = new DAL();

                Service existingService = (from service in dalDataContext.services
                                           where service.ServiceID == serviceID
                                           select service).FirstOrDefault();

                if (existingService == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Service"));
                }
                return existingService;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Service Data, Please Try Again!"));
            }
        }
        //need edit

        public static void DeleteService(User user, int serviceID)
        {

            if (!user.isSystemAdmin && !user.isEventOrganizer)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Service!"));
            //chk if user can do this anot


            DAL dalDataContext = new DAL();

            try
            {
                Service s = (from service in dalDataContext.services
                             where service.ServiceID == serviceID
                             select service).FirstOrDefault();

                dalDataContext.services.DeleteOnSubmit(s);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting service, Please Try Again!"));
            }
        }

        public static List<Service> ViewService(string SearchString)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Service> services;

                if (SearchString == null || SearchString.Trim().Equals(""))
                {
                    services = (from service in dalDataContext.services
                                select service).ToList<Service>();
                }
                else
                {
                    services = (from service in dalDataContext.services
                                where SqlMethods.Like(service.Name, "%" + SearchString + "%")
                                select service).ToList<Service>();
                }

                return services;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving services, Please Try Again!"));
            }
        }


    }
}