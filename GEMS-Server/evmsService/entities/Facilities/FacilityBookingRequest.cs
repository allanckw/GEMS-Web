using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "FacRequests")]
    public class FacilityBookingRequest
    {
        private int requestID; //(AutoNumber) 
        private int eventID; //FK
        private string requestorID; //Userid FK
        private DateTime requestStartDateTime;
        private DateTime requestEndDateTime;
        private BookingStatus status;
        private DateTime booking;
        private Faculties fac;
        private string remarks;


        public FacilityBookingRequest()
        {
        }

        public FacilityBookingRequest(int eventid,
            string requestorid, DateTime reqStart, DateTime reqEnd, Faculties fac)
        {
            this.eventID = eventid;
            this.requestStartDateTime = reqStart;
            this.requestEndDateTime = reqEnd;
            this.requestorID = requestorid;
            this.fac = fac;
            this.status = BookingStatus.Pending;
            BookingTime = DateTime.Now;
            remarks = "";
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "eventID")]
        public int EventID
        {
            get { return this.eventID; }
            set { this.eventID = value; }
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "requestID")]
        public int RequestID
        {
            get { return requestID; }
            private set { requestID = value; }
        }

        [DataMember]
        [Column(Name = "RequestorID")]
        public string RequestorID
        {
            get { return requestorID; }
            set { requestorID = value; }
        }

        [DataMember]
        [Column(Name = "ReqStartDateTime")]
        public DateTime RequestStartDateTime
        {
            get { return requestStartDateTime; }
            set { requestStartDateTime = value; }
        }

        [DataMember]
        [Column(Name = "ReqEndDateTime")]
        public DateTime RequestEndDateTime
        {
            get { return requestEndDateTime; }
            set { requestEndDateTime = value; }
        }

        [DataMember]
        [Column(Name = "Faculty", DbType = "NVarChar(150)")]
        public Faculties Faculty
        {
            get { return fac; }
            set { fac = value; }
        }

        [DataMember]
        public List<FacilityBookingRequestDetails> RequestDetails
        {
            private set { }
            get { return Controllers.FacilityBookingController.GetBookingRequestDetails(this); }
        }

        [DataMember]
        [Column(Name = "Status", DbType = "NVarChar(30)")]
        public BookingStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        [Column(Name = "BookingTime")]
        public DateTime BookingTime
        {
            get { return booking; }
            set { booking = value; }
        }

        [DataMember]
        [Column(Name = "Remarks")]
        public string Remarks
        {
            get { return this.remarks; }
            set { this.remarks = value; }
        }
    }
}