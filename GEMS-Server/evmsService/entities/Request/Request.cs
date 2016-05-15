using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Request")]
    public class Request
    {
        private int requestID;
        private int eventID;
        private string targetEmail;
        private string remark;
        private RequestStatus status;//an enum

        private string uRL;
        private DateTime requestDate;
        private string requestor;

        private string description;

        private string title;

        [DataMember]
        [Column(Name = "Title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [DataMember]
        [Column(Name = "URL")]
        public string URL
        {
            get { return uRL; }
            set { uRL = value; }
        }


        [DataMember]
        public string EventName
        {
            get { return EventController.GetEvent(eventID).Name; }
            set {  }
        }

        [DataMember]
        [Column(Name = "RequestDate")]
        public DateTime RequestDate
        {
            get { return requestDate; }
            set { requestDate = value; }
        }
        [DataMember]
        [Column(Name = "Requestor")]
        public string Requestor
        {
            get { return requestor; }
            set { requestor = value; }
        }

        public Request()
        {
        }


        public Request(int EventID, string TargetEmail,string Description, string URL, DateTime RequestDate, string Requestor, string Title)
        {
            this.Title = Title;
            this.EventID = EventID;
            this.TargetEmail = TargetEmail;
            this.Remark = "";
            status = RequestStatus.Pending;

            this.Description = Description;

            this.URL = URL;
            this.RequestDate = RequestDate;
            this.Requestor = Requestor;
        }

       

        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "RequestID")]
        public int RequestID
        {
            get { return requestID; }
            set { requestID = value; }
        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "TargetEmail")]
        public string TargetEmail
        {
            get { return targetEmail; }
            set { targetEmail = value; }
        }

        [DataMember]
        [Column(Name = "Remark")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        [DataMember]
        [Column(Name = "Status")]
        public RequestStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public List<RequestLog> Logs
        {
            private set { }
            get 
            {
                return RequestLogController.ViewRequestLog(this.requestID);
            }
        }
    }
}

