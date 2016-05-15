using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using evmsService.Controllers;
using System.Transactions;


namespace evmsService.Controllers
{

    public class RequestController
    {
        //pending ->approve , disapprove
        //approve ->nth
        //disapprove ->pending,approve
        public static string CreateNewRequest(User user, int eventid, string targetEmail, string description, string uRL, string title)
        {
            Events evnt = EventController.GetEvent(eventid);
            if (!user.isAuthorized(evnt, EnumFunctions.Manage_Requests))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Create Request!"));
            using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
            {

                DAL dalDataContext = new DAL();
                try
                {

                    Table<Request> requests = dalDataContext.requests;

               
                    Events e = EventController.GetEvent(eventid);

                    if (!e.Organizerid.Equals(user.UserID, StringComparison.CurrentCultureIgnoreCase))
                        throw new FaultException<SException>(new SException(),
                      new FaultReason("Error, User is not Organizer of this event"));


                    string otp = RequesteeController.CreateNewRequestee(targetEmail, dalDataContext);
                    Request newrequest = new Request(eventid, targetEmail, description, uRL, DateTime.Now, user.UserID, title);

                    requests.InsertOnSubmit(newrequest);
                    requests.Context.SubmitChanges();

                    RequestLogController.InsertRequestLog(newrequest);
                    tScope.Complete();

                    return otp;
                }
                catch (Exception ex)
                {
                    throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While Creating New Request: " + ex.Message));
                }
            }
        }

        public static Request GetRequest(int requestID)
        {
            DAL dalDataContext = new DAL();

            try
            {
                Request request = (from requests in dalDataContext.requests
                                   where requests.RequestID.Equals(requestID)
                                   //&& requests.TargetEmail.Equals(requestee.TargetEmail)
                                   select requests).SingleOrDefault<Request>();

                return request;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While getting Reuqest"));
            }
        }

        public static void CancelRequest(User user, int requestID)
        {

            Events evnt = EventController.GetEvent(GetRequest(requestID).EventID);
            if (!user.isAuthorized(evnt, EnumFunctions.Manage_Requests))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Cancel Request!"));

            DAL dalDataContext = new DAL();

            Request request = (from requests in dalDataContext.requests
                               where requests.RequestID == requestID
                               select requests).FirstOrDefault();

            
            if (request == null)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("Invalid Request"));
            }
            else
            {
                Events e = EventController.GetEvent(request.EventID);

                if (e.Organizerid != user.UserID) // Manage Requests, View Requests User.isAuthorized(
                    throw new FaultException<SException>(new SException(),
                        new FaultReason("Invalid User, User Does Not Have Rights To Edit This Request!"));

                request.Status = RequestStatus.Cancelled;
                dalDataContext.SubmitChanges();

                RequestLogController.InsertRequestLog(request);
            }
        }

        //requestlog : description, remark, status, date
        public static void UpdateRequest(User user, int requestID, string description, string uRL)
        {
            Events evnt = EventController.GetEvent(GetRequest(requestID).EventID);
            if (!user.isAuthorized(evnt, EnumFunctions.Manage_Requests))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit Request!"));

            DAL dalDataContext = new DAL();
            try
            {
                Request request = (from requests in dalDataContext.requests
                                   where requests.RequestID == requestID
                                   select requests).FirstOrDefault();

                if (request == null)
                {
                    throw new FaultException<SException>(new SException(),
                        new FaultReason("Invalid Request"));
                }
                else
                {
                    Events e = EventController.GetEvent(request.EventID);

                    if (e.Organizerid != user.UserID) // Manage Requests, View Requests User.isAuthorized(
                        throw new FaultException<SException>(new SException(),
                            new FaultReason("Invalid User, User Does Not Have Rights To Edit This Request!"));


                    request.Status = RequestStatus.Pending;
                    request.Description = description;
                    request.URL = uRL;
                    dalDataContext.SubmitChanges();

                    RequestLogController.InsertRequestLog(request);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                    new FaultReason("An Error occured While Creating New Request: " + ex.Message));
            }
        }


        public static List<Request> ViewRequests(Requestee requestee, DateTime fromDate, DateTime toDate, RequestStatus status, bool viewAllStatus)
        {
            // RequesteeController.ValidateRequestee(targetEmail,OTP);

            DAL dalDataContext = new DAL();

            try
            {
                IQueryable<Request> Requests = (from requests in dalDataContext.requests
                                          where requests.TargetEmail.ToLower() == requestee.TargetEmail.ToLower() &&
                                          requests.RequestDate.Date >= fromDate.Date &&
                                          requests.RequestDate.Date <= toDate.Date 
                                          select requests);
                if (!viewAllStatus)
                    Requests = Requests.Where(f => f.Status == status);

                return Requests.OrderByDescending(r => r.RequestDate).ToList<Request>();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While Viewing Request"));
            }
        }

        public static List<Request> ViewRequestViaRequester(int eventID, User user, DateTime fromDate, DateTime toDate,
                            RequestStatus status, bool viewAllStatus)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (!user.isAuthorized(evnt, EnumFunctions.Manage_Requests))
                throw new FaultException<SException>(new SException(),
                    new FaultReason("Invalid User, User Does Not Have Rights To View This Request!"));

            DAL dalDataContext = new DAL();

            try
            {
                IQueryable<Request> Requests = (from requests in dalDataContext.requests
                                          where requests.Requestor.ToLower() == user.UserID.ToLower()
                                          && requests.EventID == eventID &&
                                          requests.RequestDate.Date >= fromDate.Date &&
                                          requests.RequestDate.Date <= toDate.Date
                                          select requests);

                if (!viewAllStatus)
                    Requests = Requests.Where(f => f.Status == status);

                return Requests.OrderByDescending(r => r.RequestDate).ToList<Request>();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While Viewing Request"));
            }
        }

        public static void ChangeStatus(Requestee requestee, int requestID, RequestStatus status, string remark)//to approve or reject
        {
            DAL dalDataContext = new DAL();
            try
            {

                Request request = (from requests in dalDataContext.requests
                                   where requests.RequestID == requestID
                                   && requests.TargetEmail.Equals(requests.TargetEmail)
                                   select requests).FirstOrDefault();

                if (request == null)
                {
                    throw new FaultException<SException>(new SException(),
                        new FaultReason("Invalid Request"));
                }
                else if (!request.TargetEmail.Equals(requestee.TargetEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new FaultException<SException>(new SException(),
                        new FaultReason("You are not authorized to perform this task"));
                }
                else
                {
                    if (status == RequestStatus.Cancelled || status == RequestStatus.Pending)
                    {
                        throw new FaultException<SException>(new SException(),
                                        new FaultReason("You can either approve or reject the request!"));
                    }
                    request.Status = status;
                    request.Remark = remark;
                    dalDataContext.SubmitChanges();

                    RequestLogController.InsertRequestLog(request);
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                    new FaultReason("An Error occured While Updating Request: " + ex.Message));
            }
        }
    }
}