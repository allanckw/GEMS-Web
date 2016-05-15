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
    public class FacilityBookingController
    {
        //Create the list on the client
        public static bool AddBookingRequest(User user, EventDay evntDay, DateTime reqStart,
            DateTime reqEnd, Faculties fac, List<FacilityBookingRequestDetails> reqDetails)
        {
            return FaciRequestController.AddBookingRequest(user, evntDay, reqStart, reqEnd, fac, reqDetails);
        }


        //For Event on specific day, @PARAM pass in the Day StartDateTime
        public static List<FacilityBookingRequest> ViewBookingRequestByStatusNEventDay
    (string userID, BookingStatus s, int evID, bool viewAllStatus, bool ViewAllEvents, DateTime day)
        {
            return FaciRequestController.ViewBookingRequestByStatusNEvent
            (userID, s, evID, viewAllStatus, ViewAllEvents, day);
        }

        public static List<FacilityBookingRequest> ViewBookingRequestByFaculty(User user)
        {
            return FaciRequestController.ViewBookingRequestByFaculty(user);
        }

        public static List<FacilityBookingRequestDetails> GetBookingRequestDetails
            (FacilityBookingRequest fbr)
        {
            return FaciReqDetailsController.GetBookingRequestDetails(fbr);
        }

        public static bool checkRequestExists(int eventID, DateTime start, DateTime end)
        {
            return FaciRequestController.checkRequestExists(eventID, start, end);
        }

        public static void approveBookingRequest(User approver, int requestID, int eventID,
            int facReqDetailID, string remarks, string purpose)
        {
            FaciRequestController.approveBookingRequest(approver, requestID,
                eventID, facReqDetailID, remarks, purpose);
        }

        public static void rejectBookingRequest(User approver, int requestID, int eventID,
             string remarks)
        {
            FaciRequestController.rejectBookingRequest(approver, requestID, eventID, remarks);
        }

        public static void cancelBookingRequest(string userid, int requestID, int eventID, string remarks)
        {
            FaciRequestController.cancelBookingRequest(userid, requestID, eventID, remarks);
        }

        public static void dropConfirmedBookingRequest(string userid, int requestID, int eventID,
            string remarks)
        {
            FaciRequestController.dropConfirmedBookingRequest(userid, requestID, eventID, remarks);
        }

        private static FacilityBookingRequest GetBookingRequest(int eventID, int requestID)
        {
            return FaciRequestController.GetBookingRequest(eventID, requestID);
        }

        private static FacilityBookingRequestDetails GetBookingRequestDetails(int id)
        {
            return FaciReqDetailsController.GetBookingRequestDetails(id);
        }

        //Cannot be deleted now, only marked as cancel for history.. 
        //Not used, placed here for reference
        public static void RemoveBookingRequest(User user, int requestID)
        {
            FaciRequestController.RemoveBookingRequest(user, requestID);
        }

        //Facility Booking Confirmed
        public static List<FacilityBookingConfirmed> GetActivitesForLocation(User approver, DateTime day, Faculties faculty, string venue)
        {
            return FaciConfirmedBookingController.GetActivitesForLocation(approver, day, faculty, venue);
        }

        public static FacilityBookingConfirmed GetConfirmedDetail(int reqID)
        {
            return FaciConfirmedBookingController.GetConfirmedDetail(reqID);
        }
    }
}


