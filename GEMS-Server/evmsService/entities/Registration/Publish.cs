using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Publish")]
    public class Publish
    {
        private int eventID;//pk and link to event since its one to one
        private DateTime startDateTime;//start the publish from this date and also start the registration
        private DateTime endDateTime;//end the publish from this date and also end the registration
        private string remarks;//remarks the website can use to display toGether with this
        private Boolean isPayable;
        private decimal paymentAmount;


        public Publish() { }

        public Publish(int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, Boolean isPayable, decimal paymentAmount)
        {
            this.EventID = eventID;
            this.StartDateTime = startDateTime;
            this.EndDateTime = endDateTime;
            this.Remarks = remarks;
            this.IsPayable = isPayable;
            this.PaymentAMount = paymentAmount;
            
        }

        [DataMember]
        [Column(Name = "IsPayable")]//, CanBeNull = false
        public Boolean IsPayable
        {
            get { return isPayable; }
            set { isPayable = value; }
        }

        [DataMember]
        [Column(Name = "PaymentAmount")]//, CanBeNull = false
        public decimal PaymentAMount
        {
            get { return paymentAmount; }
            set { paymentAmount = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
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
        [Column(Name = "Remarks")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}