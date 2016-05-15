using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class FaciRequestController
    {

        public static List<Events> ViewAuthorizedEventsForFacBookings(User sender)
        {
            List<Events> evList = EventController.ViewUserAssociatedEvent(sender);
            for (int i = evList.Count - 1; i > 0; i--)
            {
                Events ev = evList[i];
                if (!sender.isAuthorized(ev, EnumFunctions.Manage_Facility_Bookings))
                {
                    evList.RemoveAt(i);
                }
            }
            return evList;
        }

        public static bool AddBookingRequest(User sender, EventDay evntDay, DateTime reqStart,
                DateTime reqEnd, Faculties fac, List<FacilityBookingRequestDetails> reqDetails)
        {
            if (sender.isFacilityAdmin ||
                sender.isAuthorized(EventController.GetEvent(evntDay.EventID),
                EnumFunctions.Manage_Facility_Bookings))//, EnumFunctions.ManageFacBookings))
            {
                bool success;
                try
                {
                    //request start > evnt.StartDateTime do not check, can book earlier...
                    if (reqStart.AddHours(2) < evntDay.StartDateTime)
                    {
                        throw new FaultException<SException>(new SException(),
                      new FaultReason("You cannot book a facility more than 2hours before your event start time"));
                    }

                    if (reqEnd.AddHours(-2) > evntDay.EndDateTime)
                    {
                        throw new FaultException<SException>(new SException(),
                      new FaultReason("You cannot book a facility for more than 2 hours after your event end time"));
                    }

                    if (reqEnd.Date != reqStart.Date)
                    {
                        if (reqEnd.Date == reqStart.Date.AddDays(1) && reqEnd.Hour == 0 && reqEnd.Minute == 0 && reqEnd.Second == 0)
                        {
                            reqEnd = reqEnd.AddSeconds(-1);
                        }
                        else
                        {
                            throw new FaultException<SException>(new SException(),
                                new FaultReason("You cannot book a across multiple days"));
                        }
                    }

                    DAL dalDataContext = new DAL();
                    Table<FacilityBookingRequest> facReq = dalDataContext.facBookReqs;
                    using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        FacilityBookingRequest f = new FacilityBookingRequest(evntDay.EventID,
                            sender.UserID, reqStart, reqEnd, fac);
                        facReq.InsertOnSubmit(f);
                        facReq.Context.SubmitChanges();

                        f = (from req in dalDataContext.facBookReqs
                             where req.EventID == evntDay.EventID &&
                             req.RequestorID == sender.UserID
                             orderby req.BookingTime descending
                             select req).FirstOrDefault<FacilityBookingRequest>();

                        success = FaciReqDetailsController.AddFacilityBookingReqDetails(f.RequestID, reqDetails);
                        tScope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    throw new FaultException<SException>(new SException(),
                      new FaultReason("An Error occured While Adding Booking Request: " + ex.Message));
                }
                if (success)
                {

                    string msg = sender.Name + " has sent you a facility booking request for "
                                    + EventController.GetEvent(evntDay.EventID).Name +
                                     Environment.NewLine + Environment.NewLine +
                                    " Please check the Facility Requests for more details.";

                    string title = "New Facility Booking Request";
                    string facAdmin = SysRoleController.GetFacilityAdmin(fac);

                    NotificationController.sendNotification(sender.UserID, facAdmin, title, msg);
                }
                return success;
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("User is not authorized to add a facility booking request!"));
            }
        }

        //Cannot be deleted now, only marked as cancel for history.. 
        //Not used, placed here for reference
        public static void RemoveBookingRequest(User sender, int requestID)
        {

            DAL dalDataContext = new DAL();
            try
            {
                FacilityBookingRequest matchedReq =
                    (from req in dalDataContext.facBookReqs
                     where req.RequestID == requestID &&
                     req.RequestorID == sender.UserID
                     select req).FirstOrDefault<FacilityBookingRequest>();

                if (matchedReq == null)
                {
                    throw new FaultException<SException>(new SException(),
                   new FaultReason("There is no such request or the requestID does not belong to you!"));
                }
                else
                {
                    dalDataContext.facBookReqs.DeleteOnSubmit(matchedReq);
                    dalDataContext.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Booking Request, Please Try Again!"));
            }
        }

        public static void UpdateFacilityRequestStatus(User user, FacilityBookingRequest request, BookingStatus status, string remarks)
        {
            DAL dalDataContext = new DAL();
            FacilityBookingRequest existingRequest =
                (from facBookings in dalDataContext.facBookReqs
                 where facBookings.EventID == request.EventID &&
                 facBookings.RequestID == request.RequestID
                 select facBookings).FirstOrDefault<FacilityBookingRequest>();

            existingRequest.Status = status;
            existingRequest.Remarks = remarks;
            dalDataContext.SubmitChanges();
        }

        public static void UpdateFacilityRequestStatus(FacilityBookingRequest request, BookingStatus status, string remarks)
        {
            DAL dalDataContext = new DAL();
            FacilityBookingRequest existingRequest =
                (from facBookings in dalDataContext.facBookReqs
                 where facBookings.EventID == request.EventID &&
                 facBookings.RequestID == request.RequestID
                 select facBookings).FirstOrDefault<FacilityBookingRequest>();

            existingRequest.Status = status;
            existingRequest.Remarks = remarks;
            dalDataContext.SubmitChanges();
        }


        public static List<FacilityBookingRequest> ViewBookingRequestByStatusNEvent
           (string sender, BookingStatus status, int evID, bool viewAllStatus, bool ViewAllDays, DateTime day)
        {
            User user = UserController.GetUser(sender);
            if (user.isFacilityAdmin ||
                user.isAuthorized(EventController.GetEvent(evID), EnumFunctions.Manage_Facility_Bookings))//, EnumFunctions.ManageFacBookings))
            {
                DAL dalDataContext = new DAL();
                IQueryable<FacilityBookingRequest> facRequest = from facBookings in dalDataContext.facBookReqs
                                                                where
                                                                facBookings.EventID == evID
                                                                orderby facBookings.BookingTime descending
                                                                select facBookings;


                if (!viewAllStatus)
                    facRequest = facRequest.Where(f => f.Status == status);

                if (!ViewAllDays)
                    facRequest = facRequest.Where(f => f.RequestStartDateTime.Date == day.Date);



                List<FacilityBookingRequest> existingRequests = (from faci in facRequest
                                                                 select faci).ToList<FacilityBookingRequest>();
                return existingRequests;
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("User is not authorized to view facility booking requests!"));
            }

        }

        public static List<FacilityBookingRequest> ViewBookingRequestByFaculty(User sender)
        {
            if ((!sender.isFacilityAdmin) && (!sender.isSystemAdmin))
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To View such requests!"));
            }

            DAL dalDataContext = new DAL();
            List<FacilityBookingRequest> existingRequests = new List<FacilityBookingRequest>();
            if (sender.isSystemAdmin)
            {
                existingRequests = (from facBookings in dalDataContext.facBookReqs
                                    where facBookings.Status == BookingStatus.Pending
                                    orderby facBookings.BookingTime descending
                                    select facBookings).ToList<FacilityBookingRequest>();
            }
            else if (sender.isFacilityAdmin)
            {
                existingRequests = (from facBookings in dalDataContext.facBookReqs
                                    where facBookings.Status == BookingStatus.Pending
                                    && facBookings.Faculty == sender.UserFaculty
                                    orderby facBookings.BookingTime descending
                                    select facBookings).ToList<FacilityBookingRequest>();
            }

            return existingRequests;
        }

        public static bool checkRequestExists(int eventID, DateTime start, DateTime end)
        {
            DAL dalDataContext = new DAL();
            List<FacilityBookingRequest> existingRequests;

            // Get all the request
            existingRequests = (from facBookings in dalDataContext.facBookReqs
                                where facBookings.EventID == eventID &&
                                facBookings.RequestStartDateTime.Date == start.Date
                                orderby facBookings.BookingTime descending
                                select facBookings).ToList<FacilityBookingRequest>();

            RequestClashingChecker checker = new RequestClashingChecker(start);

            foreach (FacilityBookingRequest request in existingRequests)
            {
                if ((request.Status == BookingStatus.Pending) || (request.Status == BookingStatus.Approved))
                    checker.SetTimeSlotTaken(request.RequestStartDateTime, request.RequestEndDateTime);
            }

            return checker.HaveClash(start, end);

        }

        public static FacilityBookingRequest GetBookingRequest(int eventID, int requestID)
        {
            DAL dalDataContext = new DAL();
            FacilityBookingRequest existingRequest =
                (from facBookings in dalDataContext.facBookReqs
                 where facBookings.EventID == eventID &&
                 facBookings.RequestID == requestID
                 select facBookings).FirstOrDefault<FacilityBookingRequest>();

            return existingRequest;
        }

        public static void approveBookingRequest(User approver, int requestID, int eventID,
            int facReqDetailID, string remarks, string purpose)
        {
            bool success;
            if ((!approver.isFacilityAdmin) && (!approver.isSystemAdmin))
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To View such requests!"));
            }

            DAL dalDataContext = new DAL();
            Table<FacilityBookingConfirmed> facReqConfirmed = dalDataContext.facConfirmedBookings;
            FacilityBookingRequest fbr = GetBookingRequest(eventID, requestID);
            try
            {
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    FacilityBookingConfirmed fbc = new FacilityBookingConfirmed(fbr,
                        FaciReqDetailsController.GetBookingRequestDetails(facReqDetailID), remarks, purpose);

                    facReqConfirmed.InsertOnSubmit(fbc);
                    facReqConfirmed.Context.SubmitChanges();

                    FaciRequestController.UpdateFacilityRequestStatus(approver, fbr, BookingStatus.Approved, remarks);

                    tScope.Complete();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                throw new FaultException<SException>(new SException(),
                   new FaultReason(ex.Message));
            }
            if (success)
            {
                string msg = "Your application for the facility for Event ID: " + fbr.EventID +
                            " On " + fbr.RequestStartDateTime.ToString("dd MMM yyyy") +
                            " has been approved.";

                NotificationController.sendNotification(approver.UserID, fbr.RequestorID,
                    "Facility Request Approved", msg);
            }

        }

        public static void rejectBookingRequest(User approver, int requestID, int eventID,
        string remarks)
        {
            if ((!approver.isFacilityAdmin) && (!approver.isSystemAdmin))
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To View such requests!"));
            }

            DAL dalDataContext = new DAL();

            FacilityBookingRequest fbr = GetBookingRequest(eventID, requestID);

            FaciRequestController.UpdateFacilityRequestStatus(approver, fbr, BookingStatus.Rejected, remarks);
            string msg = "Your application for the facility for Event ID: " + fbr.EventID +
                        " On " + fbr.RequestStartDateTime.ToString("dd MMM yyyy") +
                        " has been rejected. The remark given by the admin was " +
                        remarks;

            NotificationController.sendNotification(approver.UserID, fbr.RequestorID,
                "Facility Request Rejected", msg);
        }

        public static void cancelBookingRequest(string userid, int requestID, int eventID, string remarks)
        {
            User user = UserController.GetUser(userid);
            if (user.isFacilityAdmin ||
                user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Manage_Facility_Bookings))
            {
                FacilityBookingRequest fbr = GetBookingRequest(eventID, requestID);

                if (fbr.Status == BookingStatus.Pending)
                {
                    FaciRequestController.UpdateFacilityRequestStatus(user, fbr, BookingStatus.Cancelled, remarks);
                }
                else
                {
                    throw new FaultException<SException>(new SException(),
                          new FaultReason("You can only cancel a pending request!"));
                }
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("User is not authorized to view cancel facility booking requests!"));
            }
        }

        public static void dropConfirmedBookingRequest(FacilityBookingRequest fbr, string remarks)
        {
            DAL dalDataContext = new DAL();
            FacilityBookingConfirmed fbc =
                (from confirmedBookings in dalDataContext.facConfirmedBookings
                 where confirmedBookings.EventID == fbr.EventID &&
                 confirmedBookings.RequestID == fbr.RequestID &&
                 confirmedBookings.RequestorID == fbr.RequestorID
                 select confirmedBookings).FirstOrDefault<FacilityBookingConfirmed>();

            dalDataContext.facConfirmedBookings.DeleteOnSubmit(fbc);
            dalDataContext.SubmitChanges();
        }

        public static void dropConfirmedBookingRequest(string sender, int requestID, int eventID,
            string remarks)
        {
            User user = UserController.GetUser(sender);

            if (user.isFacilityAdmin || user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Manage_Facility_Bookings))//, EnumFunctions.ManageFacBookings))
            {

                FacilityBookingRequest fbr = GetBookingRequest(eventID, requestID);
                if (fbr.Status == BookingStatus.Approved)
                {
                    DAL dalDataContext = new DAL();
                    using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        FacilityBookingConfirmed fbc =
                            (from confirmedBookings in dalDataContext.facConfirmedBookings
                             where confirmedBookings.EventID == fbr.EventID &&
                             confirmedBookings.RequestID == fbr.RequestID &&
                             confirmedBookings.RequestorID == fbr.RequestorID
                             select confirmedBookings).FirstOrDefault<FacilityBookingConfirmed>();

                        dalDataContext.facConfirmedBookings.DeleteOnSubmit(fbc);
                        dalDataContext.SubmitChanges();

                        FaciRequestController.UpdateFacilityRequestStatus(fbr, BookingStatus.Cancelled, remarks);
                        tScope.Complete();
                    }
                }
                else
                {
                    throw new FaultException<SException>(new SException(),
                         new FaultReason("You can only drop an approved request!"));
                }
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                         new FaultReason("You are not authorized to drop a confirmed facility request"));
            }
        }

        public static void CancelRequestDueToEventTimeChange(int eventID, DateTime newStartTime,
            DateTime newEndTime)
        {
            DAL dalDataContext = new DAL();
            List<FacilityBookingRequest> existingRequests;

            // Get all the request
            existingRequests = (from facBookings in dalDataContext.facBookReqs
                                where facBookings.EventID == eventID
                                && facBookings.Status == BookingStatus.Pending
                                orderby facBookings.BookingTime descending
                                select facBookings).ToList<FacilityBookingRequest>();

            foreach (FacilityBookingRequest request in existingRequests)
            {
                if ((request.RequestStartDateTime.AddHours(-2) < newStartTime) ||
                (request.RequestEndDateTime.AddHours(2) > newEndTime))
                {
                    if (request.Status != BookingStatus.Cancelled)
                        FaciRequestController.UpdateFacilityRequestStatus(request, BookingStatus.Cancelled,
                            "Request cancelled due to event time change");

                    if (request.Status == BookingStatus.Approved)
                        FaciRequestController.dropConfirmedBookingRequest(request,
                            "Request cancelled due to event time change");
                }
            }

        }
    }
}