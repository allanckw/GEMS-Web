using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class FacilityController
    {

        public static void RemoveFacility(User user, string locID, Faculties fac)
        {
            if (!user.isFacilityAdmin)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Remove Facility!"));
            }
            DAL dalDataContext = new DAL();
            try
            {
                Facility f = (from faci in dalDataContext.Facilities
                              where faci.FacilityID == locID && faci.Faculty == fac
                              select faci).FirstOrDefault();

                if (f != null)
                {
                    if ((user.isSystemAdmin) || (f.Faculty == user.UserFaculty))
                    {
                        dalDataContext.Facilities.DeleteOnSubmit(f);
                        dalDataContext.SubmitChanges();
                    }
                    else if (f.Faculty != user.UserFaculty)
                    {
                        if (!user.isSystemAdmin)
                            throw new FaultException<SException>(new SException(),
                       new FaultReason("You do not belong to the faculty of the facility!"));
                    }

                }


            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured while deleting the facility, Please Try Again!"));
            }

        }

        public static void UpdateFacility(User user, string venue, Faculties faculty, string loc,
            string bookingCon, string techCon, int cap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            if (!user.isFacilityAdmin)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Remove Facility!"));
            }
            DAL dalDataContext = new DAL();
            Facility f = (from faci in dalDataContext.Facilities
                          where faci.FacilityID == venue && faci.Faculty == faculty
                          select faci).FirstOrDefault();
            if (f == null)
            {
                Table<Facility> facs = dalDataContext.Facilities;
                Facility fac = new Facility(venue, faculty, loc, bookingCon, techCon, cap, rtype,
                    hasWebcast, hasFlexiSeat, hasVidConf, hasMIC, hasProjector, hasVisualizer);
                facs.InsertOnSubmit(fac);
                facs.Context.SubmitChanges();
            }
            else
            {
                f.Capacity = cap;
                f.BookingContact = bookingCon;
                f.Location = loc;
                f.TechContact = techCon;
                //V0.3 Additional Fields
                f.HasflexibleSeating = hasFlexiSeat;
                f.HasMicrophone = hasMIC;
                f.HasProjector = hasProjector;
                f.HasVideoConferencing = hasVidConf;
                f.HasVisualizer = hasVisualizer;
                f.HasWebCast = hasWebcast;
                f.RoomType = rtype;
                dalDataContext.SubmitChanges();
            }
        }

        public static List<Facility> GetVenues(Faculties fac)
        {
            DAL dalDataContext = new DAL();
            try
            {
                List<Facility> fList = (from faci in dalDataContext.Facilities
                                        where  faci.Faculty == fac
                                        select faci).ToList<Facility>();

                return fList;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured retrieving facilities, Please Try Again!"));
            }
        }


        private static List<Facility> SearchFacilities(int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            DAL dalDataContext = new DAL();
            IQueryable<Facility> facilites = from facs in dalDataContext.Facilities
                                             where facs.Capacity >= minCap &&
                                             facs.Capacity <= maxCap 
                                             select facs;

            //http://stackoverflow.com/questions/11194/conditional-linq-queries
            if (hasWebcast)
                facilites = facilites.Where(f => f.HasWebCast == true);

            if (hasFlexiSeat)
                facilites = facilites.Where(f => f.HasflexibleSeating == true);

            if (hasVidConf)
                facilites = facilites.Where(f => f.HasVideoConferencing == true);

            if (hasMIC)
                facilites = facilites.Where(f => f.HasMicrophone == true);

            if (hasProjector)
                facilites = facilites.Where(f => f.HasProjector == true);

            if (hasVisualizer)
                facilites = facilites.Where(f => f.HasVisualizer == true);

            if (rtype != RoomType.All)
                facilites = facilites.Where(f => f.RoomType == rtype);

            List<Facility> fList = (from faci in facilites
                                    select faci).ToList<Facility>();

            return fList;
        }

        public static List<Facility> SearchFacilities(Faculties fac, int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            List<Facility> fList = SearchFacilities(minCap, maxCap, rtype, hasWebcast, hasFlexiSeat,
            hasVidConf, hasMIC, hasProjector, hasVisualizer);

            List<Facility> filteredByFacList = (from faci in fList
                                                where faci.Faculty == fac
                                                select faci).ToList<Facility>();

            return filteredByFacList;

        }

        public static List<Faculties> SearchFacilitiesFacs(int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            List<Facility> fList = SearchFacilities(minCap, maxCap, rtype, hasWebcast, hasFlexiSeat,
            hasVidConf, hasMIC, hasProjector, hasVisualizer);

            List<Faculties> facList = new List<Faculties>();

            foreach (Facility f in fList)
            {
                if (!facList.Contains(f.Faculty))
                {
                    facList.Add(f.Faculty);
                }
                if (facList.Count == System.Enum.GetValues(typeof(Faculties)).Length)
                {
                    break;
                }
            }

            return facList;

        }
    }
}