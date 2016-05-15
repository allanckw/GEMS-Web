using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Participant")]
    public class Participant
    {
        private int participantID;//pk
        private int eventID;//fk to publish and event
        private Boolean paid;


        public Participant() { }

        public Participant(int eventID)
        {
            this.EventID = eventID;
            this.Paid = false;
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "ParticipantID")]
        public int ParticipantID
        {
            get { return participantID; }
            set { participantID = value; }
        }

        [DataMember]
        [Column(Name = "Paid")]
        public Boolean Paid
        {
            get { return paid; }
            set { paid = value; }
        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        public List<FieldAnswer> Answer
        {
            get { return FieldAnswerController.ViewFieldAnswer(participantID); }
            set {  }
        }

        //Please include a data member for me to retrieve email from fields - Allan
        [DataMember]
        public string Email
        {
            get 
            {
                //fill in the blank 
                //List<Field> fields = FieldController.ViewField(eventID);

                Field f = FieldController.ViewField(eventID, "Email");

                FieldAnswer fs = FieldAnswerController.GetFieldAnswer(participantID, f.FieldID);

                return fs.Answer;
            }
            set
            { 
                //empty, cannot set to readonly as WCF does not allow that. 
            }
        }
    }
}