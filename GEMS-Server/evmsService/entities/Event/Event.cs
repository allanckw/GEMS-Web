using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Events")]
    public class Events
    {
        private int eventID;
        private string name;
        private DateTime startDateTime;
        private DateTime endDateTime;
        private string description;
        private string website;

        private string organizerID;

        private string tag;

        public Events()
        {

        }

        public Events(string uid, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag)
        {
            //Bug fix on server
            this.organizerID = uid;
            this.name = EventName;
            this.startDateTime = EventStartDateTime;
            this.endDateTime = EventEndDatetime;
            this.description = EventDescription;
            this.tag = EventTag;
            if (EventWebsite.Length > 0)
            {
                if (EventWebsite.StartsWith("http://"))
                    this.website = EventWebsite;
                else
                    this.website = "http://" + EventWebsite;
            }
            else
            {
                this.website = "";
            }

        }

        //[DataMember]
        //public User Organizer
        //{
        //    get
        //    {
        //        return UserController.searchUsers("", organizerID).Single<User>();
        //    }

        //    set
        //    {
        //        if (value != null)
        //            organizerID = value.UserID;
        //    }
        //}

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "OrganizerID")]
        public string Organizerid
        {
            get { return organizerID; }
            set { organizerID = value; }
        }

        [DataMember]
        [Column(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        [Column(Name = "StartDateTime")]
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { startDateTime = value; }
        }
        [DataMember]
        [Column(Name = "EndDateTime")]
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { endDateTime = value; }
        }
        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        [Column(Name = "Type")]
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        [DataMember]
        [Column(Name = "Website")]
        public string Website
        {
            get
            {
                return website;
            }
            set
            {
                if (value.Length > 0)
                {
                    if (value.ToLower().StartsWith("http://"))
                    {
                        website = value.ToLower();
                    }
                    else
                    {
                        website = "http://" + value.ToLower();
                    }
                }
                else
                {
                    website = "";
                }
            }
        }

        [DataMember]
        public List<int> DaysID
        {
            private set { }
            get
            {
                List<EventDay> days = DayController.GetDays(this.EventID);
                List<int> daysID = new List<int>();
                foreach (EventDay d in days)
                {
                    daysID.Add(d.DayID);
                }

                return daysID;
            }
        }
    }
}