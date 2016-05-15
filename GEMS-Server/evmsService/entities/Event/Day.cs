using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Days")]
    public class EventDay
    {
        private int dayID;
        private int eventID;
        private int dayNumber;


        public EventDay(int EventID, int EventDayNumber)
        {
            this.EventID = EventID;
            this.DayNumber = EventDayNumber;
        }

        public EventDay()
        {

        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "DayNumber")]
        public int DayNumber
        {
            get { return dayNumber; }

            set { dayNumber = value; }
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "DayID")]
        public int DayID
        {
            get { return dayID; }
            set { dayID = value; }
        }

        //[DataMember]
        public DateTime DayDate
        {
            get { return EventController.GetEvent(EventID).StartDateTime.Date.AddDays(this.DayNumber - 1); }
            private set { }
        }

        [DataMember]
        public DateTime StartDateTime
        {
            get
            {
                if (this.DayDate.Equals(EventController.GetEvent(EventID).StartDateTime.Date))
                    return EventController.GetEvent(EventID).StartDateTime;
                return DayDate;

            }
            private set { }
        }

        [DataMember]
        public DateTime EndDateTime
        {
            get
            {
                if (this.DayDate.Equals(EventController.GetEvent(EventID).EndDateTime.Date))
                    return EventController.GetEvent(EventID).EndDateTime;
                return DayDate.AddDays(1);
            }
            private set { }
        }


        //[DataMember]
        //public List<Guest> Guests
        //{
        //    private set { }
        //    get { return Controllers.GuestController.ViewGuest(this.dayID); }
        //}

        //[DataMember]
        //public List<Program> Programs
        //{
        //    private set { }
        //    get { return Controllers.ProgramController.ViewProgram(this.dayID); }
        //}

        //[DataMember]
        //public List<FacilityBookingConfirmed> ConfirmedFacilityBooking
        //{
        //    private set { }
        //    get { return Controllers.FaciConfirmedBookingController.GetEventConfirmedDetail(this.eventID, this.DayDate); }
        //}
    }
}