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
    public class FaciConfirmedBookingController
    {
        public static List<FacilityBookingConfirmed> GetActivitesForLocation(User approver, DateTime day, Faculties faculty, string venue)
        {

            if ((!approver.isFacilityAdmin) && (!approver.isSystemAdmin))
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To View such requests!"));
            }

            DAL dalDataContext = new DAL();

            List<FacilityBookingConfirmed> cList =
                (from confirmed in dalDataContext.facConfirmedBookings
                 where confirmed.Faculty == faculty &&
                 confirmed.Venue == venue &&
                 confirmed.RequestStartDateTime >= day &&
                 confirmed.RequestEndDateTime <= day.AddDays(1)
                 select confirmed).ToList<FacilityBookingConfirmed>();

            return cList;
        }

        public static FacilityBookingConfirmed GetConfirmedDetail(int reqID)
        {
            DAL dalDataContext = new DAL();
            FacilityBookingConfirmed fbc = (from confirmed in dalDataContext.facConfirmedBookings
                                            where confirmed.RequestID == reqID
                                            select confirmed).
                                            FirstOrDefault<FacilityBookingConfirmed>();
            return fbc;
        }

        public static List<FacilityBookingConfirmed> GetEventConfirmedDetail(int eventID, DateTime day)
        {
            DAL dalDataContext = new DAL();
            List<FacilityBookingConfirmed> fbc = (from confirmed in dalDataContext.facConfirmedBookings
                                            where confirmed.EventID == eventID 
                                            && confirmed.RequestStartDateTime.Date == day.Date
                                            select confirmed).ToList<FacilityBookingConfirmed>();
            return fbc;
        }
    }
}