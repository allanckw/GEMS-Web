using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "FieldAnswer")]
    public class FieldAnswer
    {
        private int participantID;//pk
        private int fieldID;
        private string answer;//answer the participant input

        public FieldAnswer() { }

        public FieldAnswer(int participantID,  int fieldID, string answer) {

            this.ParticipantID = participantID;
            this.FieldID = fieldID;
            this.Answer = answer;
        }

  
        [DataMember]
        [Column(IsPrimaryKey = true, Name = "FieldID")]
        public int FieldID
        {
            get { return fieldID; }
            set { fieldID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "ParticipantID")]
        public int ParticipantID
        {
            get { return participantID; }
            set { participantID = value; }
        }

   
        [DataMember]
        [Column(Name = "Answer")]
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
    }
}