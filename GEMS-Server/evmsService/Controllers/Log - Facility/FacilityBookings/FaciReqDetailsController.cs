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
    public class FaciReqDetailsController
    {
        public static bool AddFacilityBookingReqDetails(int reqID, List<FacilityBookingRequestDetails> reqDetails)
        {
            DAL dalDataContext = new DAL();
            bool complete;
            Table<FacilityBookingRequestDetails> facReqDetails = dalDataContext.facBookReqsDetails;
            try
            {
                //Remove this 2 lines to test scope rollback
                for (int i = 0; i < reqDetails.Count; i++)
                    reqDetails[i].Requestid = reqID;

                facReqDetails.InsertAllOnSubmit(reqDetails);
                facReqDetails.Context.SubmitChanges();

                complete = true;
            }
            catch (TransactionAbortedException tAbortedex)
            {
                complete = false;
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding Booking Request: " + tAbortedex.Message));
            }
            catch (Exception ex)
            {
                complete = false;
                throw new FaultException<SException>(new SException(),
                  new FaultReason("An Error occured While Adding Booking Request: " + ex.Message));
            }

            return complete;
        }

        public static FacilityBookingRequestDetails GetBookingRequestDetails(int id)
        {
            DAL dalDataContext = new DAL();
            FacilityBookingRequestDetails detail =
                (from facBookingsDetails in dalDataContext.facBookReqsDetails
                 where facBookingsDetails.RequestDetailsID == id
                 select facBookingsDetails).FirstOrDefault<FacilityBookingRequestDetails>();

            return detail;
        }

        public static List<FacilityBookingRequestDetails> GetBookingRequestDetails
    (FacilityBookingRequest fbr)
        {
            DAL dalDataContext = new DAL();
            List<FacilityBookingRequestDetails> requestDetails;

            requestDetails = (from facBookingDetails in dalDataContext.facBookReqsDetails
                              where facBookingDetails.Requestid == fbr.RequestID
                              && facBookingDetails.EventID == fbr.EventID
                              orderby facBookingDetails.Priority ascending
                              select facBookingDetails).ToList<FacilityBookingRequestDetails>();

            return requestDetails;
        }
    }
}