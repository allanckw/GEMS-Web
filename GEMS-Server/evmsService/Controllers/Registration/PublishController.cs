using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using System.Runtime.Serialization;


namespace evmsService.Controllers
{
    public class PublishController
    {
        public static void AddPublish(Events evnt, DateTime startDateTime, DateTime endDateTime, string remarks, Boolean isPayable, decimal paymentAmount, DAL dalDataContext)
        {
            
            if (endDateTime.CompareTo(startDateTime) < 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Error, start date must be ealier then end date"));

            if (evnt.EndDateTime.CompareTo(startDateTime) < 0 || evnt.EndDateTime.CompareTo(endDateTime) < 0)
                throw new FaultException<SException>(new SException(),
                       new FaultReason("Error, start or end date/time must not be after the event's date/time"));

            try
            {

                FieldController.AddDefaultFeids(evnt.EventID, dalDataContext);
                Table<Publish> publishs = dalDataContext.publishs;
                Publish creatingpublish = new Publish(evnt.EventID, startDateTime, endDateTime, remarks, isPayable, paymentAmount);

                publishs.InsertOnSubmit(creatingpublish);
                publishs.Context.SubmitChanges();
            }
            catch (InvalidOperationException ex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("invalid operation, event already have paid participant and cannot be an unpaid event anymore!"));
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Publishing Event, Please Try Again!"));
            }
        }

        public static void AddPublish(User user, int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, Boolean isPayable, decimal paymentAmount)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (!user.isAuthorized(evnt))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Publish this Event!"));

            if (endDateTime.CompareTo(startDateTime) < 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Error, start date must be ealier then end date"));

            if (evnt.EndDateTime.CompareTo(startDateTime) < 0 || evnt.EndDateTime.CompareTo(endDateTime) < 0)
                throw new FaultException<SException>(new SException(),
                       new FaultReason("Error, start or end date/time must not be after the event's date/time"));

            try
            {
                int fieldsize = FieldController.ViewField(eventID).Count();

                if (isPayable == false && ParticipantController.isParticipantPaid(eventID) == true)
                {
                    throw new InvalidOperationException();
                }


                
                DAL dalDataContext = new DAL();

                
                if(fieldsize == 0)
                    FieldController.AddDefaultFeids(evnt.EventID, dalDataContext);
                Table<Publish> publishs = dalDataContext.publishs;
                Publish creatingpublish = new Publish(eventID, startDateTime, endDateTime, remarks, isPayable, paymentAmount);

                publishs.InsertOnSubmit(creatingpublish);
                publishs.Context.SubmitChanges();
            }
            catch (InvalidOperationException ex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("invalid operation, event already have paid participant and cannot be an unpaid event anymore!"));
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Publishing Event, Please Try Again!"));
            }
        }
        public static Publish GetPublish(int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Publish existingPublish = (from publish in dalDataContext.publishs
                                           where publish.EventID == EventID
                                           select publish).FirstOrDefault();

                //if (existingPublish == null)
                //{
                //    throw new FaultException<SException>(new SException(),
                //       new FaultReason("Invalid Publish"));
                //}
                return existingPublish;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Publish Data, Please Try Again!"));
            }
        }

        public static void DeletePublish(User user, int EventID)
        {
            //chk if user can do this anot

            if (!user.isAuthorized(EventController.GetEvent(EventID)))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Publish!"));



            try
            {
                DAL dalDataContext = new DAL();

                //List<Participant> part = ParticipantController.ViewParticipant(EventID);



                Publish p = (from publish in dalDataContext.publishs
                             where publish.EventID == EventID
                             select publish).FirstOrDefault();

                //if (p.IsPayable && part.Count > 0)
                //{

                //    throw new InvalidOperationException();
                //}


                //GetPublish(EventID);

                dalDataContext.publishs.DeleteOnSubmit(p);
                dalDataContext.SubmitChanges();
            }
            //catch (InvalidOperationException e)
            //{
            //    throw new FaultException<SException>(new SException(),
            //       new FaultReason("An Error occured While Deleting Publish, Please Try Again!"));
            //}
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Publish, Please Try Again!"));
            }
        }
        public static void EditPublish(User user, int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, bool isPayable, decimal amount)
        {

            if (!user.isAuthorized(EventController.GetEvent(eventID)))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit this Publish!"));
            try
            {
                DAL dalDataContext = new DAL();

                Publish p = (from publish in dalDataContext.publishs
                             where publish.EventID == eventID
                             select publish).FirstOrDefault();

                if (p == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Publish"));
                }
                else
                {
                    //List<Participant> part = ParticipantController.ViewParticipant(eventID);


                    if (p.IsPayable && isPayable == false && ParticipantController.isParticipantPaid(eventID) == true)
                    {
                        throw new InvalidOperationException();
                    }

                    p.IsPayable = isPayable;
                    p.PaymentAMount = amount;
                    p.StartDateTime = startDateTime;
                    p.EndDateTime = endDateTime;
                    p.Remarks = remarks;

                    dalDataContext.SubmitChanges();
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Operation, you cant change an payable event to an no payable event after participant have paid!"));
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Publish, Please Try Again!"));
            }
        }
        public static List<Events> ViewEvent(DateTime start, DateTime end)
        {
            end = end.AddDays(1).AddMilliseconds(-1);
            DAL dalDataContext = new DAL();
            List<Events> existingEvents;

            existingEvents = (from events in dalDataContext.events
                              join publish in dalDataContext.publishs on
                                 events.EventID equals publish.EventID
                              where events.StartDateTime >= start
                                && events.EndDateTime <= end
                              orderby events.StartDateTime ascending
                              select events).ToList<Events>();

            existingEvents = existingEvents.OrderBy(s => s.StartDateTime).ToList<Events>();


            return existingEvents;
        }

        public static List<ParticipantEvent> ParticipantViewEvents(string participantEmail, DateTime start, DateTime end, bool paid)
        {
            end = end.AddDays(1).AddMilliseconds(-1);
            DAL dalDataContext = new DAL();
            List<Events> existingEvents;

            //DateTime now = DateTime.Now;

            existingEvents = (from events in dalDataContext.events
                              join publish in dalDataContext.publishs on
                                 events.EventID equals publish.EventID
                              where events.StartDateTime >= start
                                && events.EndDateTime <= end
                              // && now >= publish.StartDateTime
                              // && now <= publish.EndDateTime
                              orderby events.StartDateTime ascending
                              select events).ToList<Events>();

            dalDataContext.Dispose();
            existingEvents = existingEvents.OrderBy(s => s.StartDateTime).ToList<Events>();

            List<ParticipantEvent> lstpe = new List<ParticipantEvent>();
            foreach (Events e in existingEvents)
            {
                Publish p = PublishController.GetPublish(e.EventID);

                Participant part = ParticipantController.GetParticipant(e.EventID, participantEmail);
                if (part != null)
                {
                    if (part.Paid == paid)
                    {
                        ParticipantEvent pe = new ParticipantEvent(e.EventID, e.Name, e.StartDateTime, e.EndDateTime, p.PaymentAMount, part.Paid);
                        lstpe.Add(pe);
                    }
                }
                
            }
            return lstpe;
        }

        public static List<Events> ViewEventDateAndTag(DateTime start, DateTime end, String tag)
        {
            end = end.AddDays(1).AddMilliseconds(-1);
            DAL dalDataContext = new DAL();
            List<Events> existingEvents;

            existingEvents = (from events in dalDataContext.events
                              join publish in dalDataContext.publishs on
                                 events.EventID equals publish.EventID
                              where events.StartDateTime >= start
                                && events.EndDateTime <= end
                                && events.Tag.IndexOf(tag) >= 0
                              orderby events.StartDateTime ascending
                              select events).ToList<Events>();

            existingEvents = existingEvents.OrderBy(s => s.StartDateTime).ToList<Events>();


            return existingEvents;
        }

        public static Publish ViewPublish(int EventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                DateTime now = DateTime.Now;

                //Publish p = (from ps in dalDataContext.publishs
                //             where ps.EventID == EventID &&
                //             ps.StartDateTime <= now &&
                //             ps.EndDateTime >= now
                //             select ps).SingleOrDefault();
                //dalDataContext.Dispose();

                Publish p = (from ps in dalDataContext.publishs
                             where ps.EventID == EventID
                             select ps).SingleOrDefault();
                dalDataContext.Dispose();
                return p;

            }
            catch
            {

                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Publish Data, Please Try Again!"));
            }
        }
    }

    [DataContract]
    public class ParticipantEvent
    {

        private int eventID;
        private string eventName;
        private DateTime eventStartDate;
        private DateTime eventEndDate;
        private decimal eventRegistrationCost;
        private bool participantPaid;

        [DataMember]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }


        [DataMember]
        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        [DataMember]
        public DateTime EventStartDate
        {
            get { return eventStartDate; }
            set { eventStartDate = value; }
        }

        [DataMember]
        public DateTime EventEndDate
        {
            get { return eventEndDate; }
            set { eventEndDate = value; }
        }

        [DataMember]
        public decimal EventCost
        {
            get { return eventRegistrationCost; }
            set { eventRegistrationCost = value; }
        }

        [DataMember]
        public bool isPaid
        {
            get { return participantPaid; }
            set { participantPaid = value; }
        }

        public ParticipantEvent(int eventID, string eventName, DateTime eventStartDate,
            DateTime eventEndDate, decimal eventRegistrationCost, bool participantPaid)
        {
            this.eventID = eventID;
            this.eventName = eventName;
            this.eventStartDate = eventStartDate;
            this.eventEndDate = eventEndDate;
            this.eventRegistrationCost = eventRegistrationCost;

            if (eventRegistrationCost == 0)
            {
                this.participantPaid = true;
            }
            else
            {
                this.participantPaid = participantPaid;
            }
                        
        }
    }
}