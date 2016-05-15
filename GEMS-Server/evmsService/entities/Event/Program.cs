using System;
using System.Linq;
using evmsService.entities;
using evmsService.DataAccess;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Collections.Generic;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Programs")]
    public class Program
    {
        private int programID;
        private string name;
        private DateTime startDateTime;
        private DateTime endDateTime;
        private string description;
        private int dayID;
        private string location;

                public Program()
        {
        }

        public Program(string ProgramName, DateTime ProgramStartDateTime, 
            DateTime ProgramEndDateTime, string ProgramDescription
        , int DayID, string ProgramLocation)
        {

            location = ProgramLocation;
            name = ProgramName;
            startDateTime = ProgramStartDateTime;
            endDateTime = ProgramEndDateTime;
            description = ProgramDescription;
            dayID = DayID;
        }


        [DataMember]
        [Column(Name = "DayID")]
        public int DayID
        {
            get { return dayID; }
            set { dayID = value; }
        }


        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "ProgramID")]
        public int ProgramID
        {
            get { return programID; }
            set { programID = value; }
        }

        [DataMember]
        [Column(Name = "Location")]
        public string Location
        {
            get { return location; }
            set { location = value; }
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
        public int EventID
        {
            private set { }
            get { return DayController.GetEventID(this); }
        }
        
    }
}