using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;


namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "ParticipantsTrans")]
    public class ParticipantTransaction
    {
        private int eventID;
        private string email;
        private string transID;
        private DateTime transDateTime;
        private decimal amount;
        private string remark;

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "TransactionID")]
        public string TransactionID
        {
            get { return transID; }
            set { transID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "ParticipantMail")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [DataMember]
        [Column(Name = "TransDateTime")]
        public DateTime TransactionDateTime
        {
            get { return transDateTime; }
            set { transDateTime = value; }
        }

        [DataMember]
        [Column(Name = "Amount")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [DataMember]
        [Column(Name = "Remarks")]
        public string Remarks
        {
            get { return remark; }
            set { remark = value; }
        }

    }
}