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

    public class EventWithDays
    {
        public Events e;
        public List<EventDay> ds;

        public EventWithDays()
        {
            ds = new List<EventDay>();
        }
    }
    public class EventDayController
    {
        public static void AddEvent(User user, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag)
        {
            if (!user.isEventOrganizer)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Create Events!"));

            if (EventStartDateTime.CompareTo(EventEndDatetime) >= 0)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Date Entry, End Date Must be at a Later Date Then Start Date"));
            }
            if (EventName.Trim() == "")
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Event Name"));
            }

            try
            {
                DAL dalDataContext = new DAL();

                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Events e = EventController.AddEvent(user, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag, dalDataContext);

                    DateTime current_date = EventStartDateTime.Date;
                    current_date = current_date.AddHours(EventStartDateTime.Hour);
                    current_date = current_date.AddMinutes(EventStartDateTime.Minute);

                    DateTime end_date = EventEndDatetime.Date;
                    end_date = end_date.AddHours(EventEndDatetime.Hour);
                    end_date = end_date.AddMinutes(EventEndDatetime.Minute);

                    int day = 1;
                    do
                    {
                        DayController.AddDay(e.EventID, day, dalDataContext);
                        day++;

                        current_date = current_date.Date;
                        current_date = current_date.AddDays(1);
                    } while (current_date < end_date);


                    tScope.Complete();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Event, Please Try Again!"));
            }
        }

        public static EventWithDays AddEvent(User user, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag, DAL dalDataContext)
        {
            if (!user.isEventOrganizer)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Create Events!"));

            if (EventStartDateTime.CompareTo(EventEndDatetime) >= 0)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Date Entry, End Date Must be at a Later Date Then Start Date"));
            }
            if (EventName.Trim() == "")
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Event Name"));
            }

            try
            {
                Events e = EventController.AddEvent(user, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag, dalDataContext);

                DateTime current_date = EventStartDateTime.Date;
                current_date = current_date.AddHours(EventStartDateTime.Hour);
                current_date = current_date.AddMinutes(EventStartDateTime.Minute);


                DateTime end_date = EventEndDatetime.Date;
                end_date = end_date.AddHours(EventEndDatetime.Hour);
                end_date = end_date.AddMinutes(EventEndDatetime.Minute);

                int day = 1;

                EventWithDays ed = new EventWithDays();
                ed.e = e;


                do
                {
                    EventDay d = DayController.AddDay(e.EventID, day, dalDataContext);
                    day++;

                    ed.ds.Add(d);
                    current_date = current_date.Date;
                    current_date = current_date.AddDays(1);
                } while (current_date < end_date);



                return ed;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Event, Please Try Again!"));
            }
        }


        public static void EditEvent(User user, Events evnt, string EventName,
            DateTime newStartDateTime, DateTime newEndDateTime, string EventDescription, string EventWebsite, string EventTag)
        {
            if (!user.isAuthorized(evnt))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit this Events!"));
            //chk if user can do this anot

            Events matchedevent = EventController.GetEvent(evnt.EventID);
            DateTime oldEventStartDate = matchedevent.StartDateTime.Date;

            if (matchedevent == null)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Event"));
            }

            try
            {


                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                {

                    DAL dalDataContext = new DAL();
                    //old - new
                    //if position = number of days to add
                    TimeSpan start_diff = matchedevent.StartDateTime.Date - newStartDateTime.Date;

                    int start_day_diff = start_diff.Days;

                    DateTime end_old = matchedevent.EndDateTime;
                    if (!end_old.Date.Equals(end_old))
                        end_old = end_old.Date.AddDays(1);

                    DateTime end_new = newEndDateTime;
                    if (!end_new.Date.Equals(end_new))
                        end_new = end_new.Date.AddDays(1);

                    //new-old
                    //if position = number of days to add
                    TimeSpan end_diff = end_new - end_old;
                    int end_day_diff = end_diff.Days;

                    int actualydaydiff = start_day_diff + end_day_diff;

                    EventController.EditEvent(user, evnt, EventName, newStartDateTime, newEndDateTime, EventDescription, EventWebsite, EventTag, dalDataContext);

                    List<EventDay> days = DayController.GetDays(evnt.EventID);

                    TimeSpan temp = end_new - newStartDateTime.Date;
                    int total_days = temp.Days;


                    //need to know if old date is got cross path with new date
                    bool isWithin = false;

                    //DateTime oldEventStartDate = days[0].StartDateTime.Date;
                    //DateTime oldEventEndDate = oldEventStartDate.AddDays(days.Count);

                    //if (newStartDateTime <= oldEventStartDate && newEndDateTime >= oldEventEndDate)
                    //    isWithin = true;

                    for (int i = 0; i < days.Count(); i++)
                    {
                        DateTime t = oldEventStartDate.AddDays(i);
                        if (newStartDateTime <= t && newEndDateTime >= t)
                        {
                            isWithin = true;
                        }
                    }

                    if (!isWithin)
                    {

                        //first delete all
                        for (int i = 0; i < days.Count(); i++)
                        {
                            DayController.DeleteDay(days[i], dalDataContext);
                        }

                        //add all new

                        int day = 1;
                        DateTime EventStartDateTime = newStartDateTime;
                        DateTime EventEndDatetime = newEndDateTime;

                        DateTime current_date = EventStartDateTime.Date;
                        current_date = current_date.AddHours(EventStartDateTime.Hour);
                        current_date = current_date.AddMinutes(EventStartDateTime.Minute);


                        DateTime end_date = EventEndDatetime.Date;
                        end_date = end_date.AddHours(EventEndDatetime.Hour);
                        end_date = end_date.AddMinutes(EventEndDatetime.Minute);

                        do
                        {
                            EventDay d = DayController.AddDay(evnt.EventID, day, dalDataContext);
                            day++;


                            current_date = current_date.Date;
                            current_date = current_date.AddDays(1);
                        } while (current_date < end_date);


                    }
                    else
                    {
                        for (int i = start_day_diff; i < 0; i++)
                        {

                            DayController.DeleteDay(days[0], dalDataContext);
                            days.RemoveAt(0);
                            start_day_diff++;
                        }

                        for (int i = end_day_diff; i < 0; i++)
                        {
                            DayController.DeleteDay(days[days.Count - 1], dalDataContext);
                            days.RemoveAt(days.Count - 1);
                            end_day_diff++;
                        }

                        int data = start_day_diff + end_day_diff + days.Count;
                        //total days must = data or means programming something wrong, or manaully edited
                        //throw error
                        if (!data.Equals(total_days))
                            throw new FaultException<SException>(new SException(),
                                    new FaultReason("days code may have been edited"));
                        //maybe add chk procedure to make sure the db always in series?
                        //add code here to add back the days
                        //mod first

                        int temp_count = days.Count + start_day_diff;
                        for (int i = start_day_diff; i < temp_count; i++)
                        {
                            DayController.EditDay(days[0].DayID, i + 1, dalDataContext);

                            days.RemoveAt(0);
                        }

                        for (int i = 0; i < start_day_diff; i++)
                        {
                            if (start_day_diff > 0)
                            {
                                DayController.AddDay(evnt.EventID, i + 1, dalDataContext);
                                continue;
                            }
                        }
                        start_day_diff = 0;
                        for (int i = total_days - end_day_diff; i < total_days; i++)
                        {

                            if (end_day_diff > 0)
                            {
                                //add new
                                DayController.AddDay(evnt.EventID, i + 1, dalDataContext);
                                end_day_diff--;
                                continue;
                            }


                            throw new FaultException<SException>(new SException(),
                                    new FaultReason("days code may have been edited"));


                        }

                        if (days.Count != 0 || end_day_diff != 0)
                            throw new FaultException<SException>(new SException(),
                                new FaultReason("days code may have been edited"));
                    }
                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {

                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing New Event, Please Try Again!"));
            }
        }
    }
}