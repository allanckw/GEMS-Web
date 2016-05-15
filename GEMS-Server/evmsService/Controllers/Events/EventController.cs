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
    public class EventController
    {

        public static Boolean isEventExist(int eventID)
        {
            DAL dalDataContext = new DAL();

            Events existingEvent = (from events in dalDataContext.events
                                    where events.EventID == eventID
                                    //   where events.Organizerid == user.userID
                                    select events).FirstOrDefault();

            if (existingEvent == null)
                return false;

            return true;
        }

        public static Events AddEvent(User user, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag, DAL dalDataContext)
        {
            try
            {
                //DAL dalDataContext = new DAL();
                Table<Events> events = dalDataContext.events;
                //seperate into diff day here

                Events creatingevent;

                creatingevent = new Events(user.UserID, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag);

                events.InsertOnSubmit(creatingevent);

                events.Context.SubmitChanges();
                return creatingevent;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Event, Please Try Again!"));
            }

        }



        public static void DeleteEvent(User user, Events evnt)
        {
            //chk if user can do this anot
            if (!user.isAuthorized(evnt))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete this Events!"));

            DAL dalDataContext = new DAL();

            try
            {
                Events matchedevent = (from events in dalDataContext.events
                                       where events.EventID == evnt.EventID
                                       select events).FirstOrDefault();


                dalDataContext.events.DeleteOnSubmit(matchedevent);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Deleting Event, Please Try Again!"));
            }
        }

        public static Events GetEvent(int EventID)
        {
            DAL dalDataContext = new DAL();

            Events existingEvent = (from events in dalDataContext.events
                                    where events.EventID == EventID
                                    //   where events.Organizerid == user.userID
                                    select events).FirstOrDefault();

            if (existingEvent == null)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Event!"));

            return existingEvent;
        }

        public static Events GetEvent(int EventID, DAL dalDataContext)
        {
            // DAL dalDataContext = new DAL();

            Events existingEvent = (from events in dalDataContext.events
                                    where events.EventID == EventID
                                    //   where events.Organizerid == user.userID
                                    select events).FirstOrDefault();

            if (existingEvent == null)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Event!"));

            return existingEvent;
        }

        public static void DeleteEvent(User user, int EventID)
        {
            //chk if user can do this anot
            Events evnt = GetEvent(EventID);

            if (!user.isAuthorized(evnt))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete this Events!"));

            DAL dalDataContext = new DAL();

            try
            {
                Events matchedevent = (from events in dalDataContext.events
                                       where events.EventID == evnt.EventID
                                       //events.Organizerid == user.userID
                                       select events).FirstOrDefault();


                dalDataContext.events.DeleteOnSubmit(matchedevent);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Event, Please Try Again!"));
            }
        }

        public static void EditEvent(User user, Events evnt, string EventName,
            DateTime newStartDateTime, DateTime newEndDateTime, string EventDescription, string EventWebsite, string EventTag, DAL dalDataContext)
        {

            Events matchedevent = (from events in dalDataContext.events
                                   where events.EventID == evnt.EventID
                                   select events).FirstOrDefault();

            if (matchedevent == null)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Event"));
            }
            else
            {
                if (newEndDateTime <= newStartDateTime)
                {
                    throw new FaultException<SException>(new SException("Error!"),
                      new FaultReason("Event's end time must be after its start time."));
                }

                if ((matchedevent.StartDateTime != newStartDateTime) ||
                    (matchedevent.EndDateTime != newEndDateTime))
                {
                    matchedevent.StartDateTime = newStartDateTime;
                    matchedevent.EndDateTime = newEndDateTime;
                    FaciRequestController.CancelRequestDueToEventTimeChange(matchedevent.EventID, newStartDateTime, newEndDateTime);
                }

                matchedevent.Description = EventDescription;

                //Bug Fix
                if (string.Compare(EventWebsite.ToLower(), "http://", true) == 0)
                    matchedevent.Website = "";
                else
                    matchedevent.Website = EventWebsite;

                matchedevent.Name = EventName;
                matchedevent.Tag = EventTag;

                dalDataContext.SubmitChanges();

            }
        }

        public static List<Events> ViewAllEvents(string userid)
        {
            User user = UserController.GetUser(userid);

            if ((!user.isFacilityAdmin) && (!user.isSystemAdmin))
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To View such requests!"));
            }
            DAL dalDataContext = new DAL();

            List<Events> existingEvents = (from events in dalDataContext.events
                                           where events.StartDateTime.Date >= DateTime.Now.Date
                                           orderby events.StartDateTime ascending
                                           select events).Distinct().ToList<Events>();
            return existingEvents;
        }

        public static List<Events> ViewOrganizerEvent(User user)
        {
            DAL dalDataContext = new DAL();

            DateTime today = DateTime.Today;
            if (user.isSystemAdmin)
            {
                return (from events in dalDataContext.events
                        where events.StartDateTime >= today
                        orderby events.StartDateTime ascending
                        select events).ToList<Events>();
            }
            else
            {


                var existingEvents = (from events in dalDataContext.events
                                    //  join roles in dalDataContext.roles on
                                      //events.EventID equals roles.EventID
                                      where events.StartDateTime >= today
                                      && events.Organizerid == user.UserID
               // roles.UserID == user.UserID  && 
                                      
                                      select events).Distinct();
                                    

                return existingEvents.OrderBy(e => e.StartDateTime).ToList<Events>();
            }

        }

        public static List<Events> ViewEventsByDateTag(User user, DateTime start, DateTime end, string tag)
        {
            end = end.AddDays(1).AddMilliseconds(-1);
            DAL dalDataContext = new DAL();
            IQueryable<Events> existingEvents;
            if (user.isSystemAdmin)
            {
                existingEvents = (from events in dalDataContext.events
                                  where events.StartDateTime >= start
                                  && events.EndDateTime <= end
                                  orderby events.StartDateTime ascending
                                  select events);
            }
            else
            {
                existingEvents = (from events in dalDataContext.events
                                  join roles in dalDataContext.roles on
                                  events.EventID equals roles.EventID
                                  where roles.UserID == user.UserID
                                  && events.StartDateTime >= start
                                  && events.StartDateTime <= end
                                  select events).Union(
                                            from events in dalDataContext.events
                                            where events.Organizerid == user.UserID &&
                                            events.StartDateTime >= start
                                            && events.EndDateTime <= end
                                            orderby events.StartDateTime ascending
                                            select events).Distinct();
            }

            if (tag.Trim().Length > 0)
                existingEvents = existingEvents.Where(e => e.Tag.Contains(tag));

            return existingEvents.OrderBy(e => e.StartDateTime).ToList<Events>();
        }

        public static List<Events> ViewUserAssociatedEvent(User user)
        {
            DateTime today = DateTime.Today;
            DAL dalDataContext = new DAL();
            IQueryable<Events> existingEvents;
            if (user.isSystemAdmin)
            {
                existingEvents = (from events in dalDataContext.events
                                  where events.StartDateTime >= today
                                  orderby events.StartDateTime ascending
                                  select events);
            }
            else
            {
                existingEvents = (from events in dalDataContext.events
                                  join roles in dalDataContext.roles on
                                  events.EventID equals roles.EventID
                                  where
                                  roles.UserID == user.UserID
                                  select events).Union(
                                            from events in dalDataContext.events
                                            where events.Organizerid == user.UserID
                                            && events.StartDateTime >= today
                                            orderby events.StartDateTime ascending
                                            select events).Distinct();
            }
           
            return existingEvents.OrderBy(e => e.StartDateTime).ToList<Events>();
        }

        public static List<Events> ViewEventsByTag(User user, string tag)
        {

            DAL dalDataContext = new DAL();
            IQueryable<Events> existingEvents;
            if (user.isSystemAdmin)
            {
                existingEvents = (from events in dalDataContext.events
                                  orderby events.StartDateTime ascending
                                  select events);
            }
            else
            {
                existingEvents = (from events in dalDataContext.events
                                  join roles in dalDataContext.roles on
                                  events.EventID equals roles.EventID
                                  where
                                  roles.UserID == user.UserID
                                  select events).Union(
                                            from events in dalDataContext.events
                                            where events.Organizerid == user.UserID
                                            orderby events.StartDateTime ascending
                                            select events).Distinct();
            }
            if (tag.Trim().Length > 0)
                existingEvents = existingEvents.Where(e => e.Tag.Contains(tag));

            return existingEvents.OrderBy(e => e.StartDateTime).ToList<Events>();
        }

    }
}