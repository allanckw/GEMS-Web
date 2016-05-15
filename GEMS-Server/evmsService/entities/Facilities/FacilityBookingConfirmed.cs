using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "FacConfirmedBookings")]
    public class FacilityBookingConfirmed
    {
        private int confirmedID;

        private int eventID;  //FK To Request Table      
        private int requestid; //FK To Request Table

        private int requestDetailID;

        private string requestor; //FK to NUSNET table

        Faculties fac; //FK To NUSFacilities
        string facilityID; //FK To NUSFacilities

        private DateTime requestStartDateTime;
        private DateTime requestEndDateTime;

        private string remarks;
        private string purpose;

        public FacilityBookingConfirmed()
        {
        }

        public FacilityBookingConfirmed(FacilityBookingRequest fbr, 
            FacilityBookingRequestDetails fbrd, string remarks, string purpose)
        {
            this.eventID = fbr.EventID;
            this.requestid = fbr.RequestID;
            this.RequestorID = fbr.RequestorID;

            this.Faculty = fbrd.Faculty;
            this.facilityID = fbrd.FacilityID;

            this.requestDetailID = fbrd.RequestDetailsID;

            this.requestStartDateTime = fbr.RequestStartDateTime;
            this.requestEndDateTime = fbr.RequestEndDateTime;

            this.remarks = remarks;
            this.purpose = purpose;
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "ConfirmedID")]
        public int ConfirmedID
        {
            get { return confirmedID; }
            private set {confirmedID = value; }
        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "RequestID")]
        public int RequestID
        {
            get { return requestid; }
            set { requestid = value; }
        }

        [DataMember]
        [Column(Name = "RequestorID")]
        public string RequestorID
        {
            get { return requestor; }
            set { requestor = value; }
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
        [Column(Name = "FacilityID")]
        public string Venue
        {
            get { return facilityID; }
            set { facilityID = value; }
        }

        [DataMember]
        [Column(Name = "ReqDetailID")]
        public int RequestDetailID
        {
            get { return requestDetailID; }
            set { requestDetailID = value; }
        }

        [DataMember]
        [Column(Name = "Remarks")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        [DataMember]
        [Column(Name = "Purpose")]
        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }

    }
}