using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;


namespace evmsService.Controllers
{
    public class GuestController
    {

        public static List<int> GetEventGuestCount(int eventID)
        {
            DAL dalDataContext = new DAL();

            Events ev = EventController.GetEvent(eventID);

            List<int> guestCountList = new List<int>();

            foreach (int dayid in ev.DaysID)
            {
                List<Guest> guestsList = (from guests in dalDataContext.guests
                                          where guests.DayID== dayid
                                          select guests).ToList<Guest>();
                
                guestCountList.Add(guestsList.Count);
            }
            return guestCountList;
        }

        public static void AddGuest(int dayID, string GuestName, string GuestContact, string GuestDescription, DAL dalDataContext)
        {

            try
            {

                Table<Guest> guests = dalDataContext.guests;
                Guest creatingguest = new Guest(GuestName, GuestContact, GuestDescription, dayID);

                guests.InsertOnSubmit(creatingguest);
                guests.Context.SubmitChanges();

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Guest, Please Try Again!"));
            }
        }

        public static int AddGuest(User user, int dayID, string GuestName, string GuestContact, string GuestDescription)
        {
            if (!user.isAuthorized(EventController.GetEvent(DayController.GetDay(dayID).EventID), EnumFunctions.Add_Guest))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Guests!"));
            try
            {
                DAL dalDataContext = new DAL();
                Table<Guest> guests = dalDataContext.guests;
                Guest creatingguest = new Guest(GuestName, GuestContact, GuestDescription, dayID);

                guests.InsertOnSubmit(creatingguest);
                guests.Context.SubmitChanges();

                return creatingguest.GuestId;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Guest, Please Try Again!"));
            }
        }
        public static Guest GetGuest(int GuestID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Guest existingGuest = (from guest in dalDataContext.guests
                                       where guest.GuestId == GuestID
                                       select guest).FirstOrDefault();

                if (existingGuest == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Guest"));
                }
                return existingGuest;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Guest Data, Please Try Again!"));
            }
        }

        public static void DeleteGuest(User user, int GuestID)
        {
            //chk if user can do this anot
            Guest g = GetGuest(GuestID);

            if (!user.isAuthorized(EventController.GetEvent(g.EventID), EnumFunctions.Delete_Guest))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Guest!"));

            DAL dalDataContext = new DAL();

            try
            {
                Guest matchedguest = (from guests in dalDataContext.guests
                                      where guests.GuestId == g.GuestId
                                      select guests).FirstOrDefault();


                dalDataContext.guests.DeleteOnSubmit(matchedguest);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Guest, Please Try Again!"));
            }
        }
        public static void EditGuest(User user, int GuestID, string GuestName, string GuestDescription, string GuestContact)
        {
            Guest g = GetGuest(GuestID);

            if (!user.isAuthorized(EventController.GetEvent(g.EventID), EnumFunctions.Edit_Guest))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit this Guest!"));
            try
            {
                DAL dalDataContext = new DAL();

                Guest matchedguest = (from guests in dalDataContext.guests
                                      where guests.GuestId == g.GuestId
                                      select guests).FirstOrDefault();

                if (matchedguest == null)
                {

                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Guest"));
                }
                else
                {
                    matchedguest.Name = GuestName;
                    matchedguest.Description = GuestDescription;
                    matchedguest.Contact = GuestContact;
                    dalDataContext.SubmitChanges();

                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Guest, Please Try Again!"));
            }
        }
        public static List<Guest> ViewGuest(int dayID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Guest> Guests = (from guests in dalDataContext.guests
                                      where guests.DayID == dayID
                                      select guests).ToList<Guest>();

                return Guests;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Guest Data, Please Try Again!"));
            }
        }
    }
}