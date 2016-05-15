using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "RequestLog")]
    public class RequestLog
    {
        private int requestLogID;
        private int requestID;
        private DateTime logDateTime;
        private string description;
        private string remark;
        private RequestStatus status;//an enum
        private string uRL;


        public RequestLog()
        { }

        public RequestLog(int RequestID, string Description, string Remark, RequestStatus Requeststatus, string URL)
        {
            this.uRL = URL;
            this.requestID = RequestID;
            this.description = Description;
            this.remark = Remark;
            this.status = Requeststatus;
            this.logDateTime = DateTime.Now;

        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "RequestLogID")]
        public int RequestLogID
        {
            get { return requestLogID; }
            set { requestLogID = value; }
        }

        [DataMember]
        [Column(Name = "LogDatetime")]
        public DateTime LogDateTime
        {
            get { return logDateTime; }
            set { logDateTime = value; }
        }

        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        [DataMember]
        [Column(Name = "RequestID")]
        public int RequestID
        {
            get { return requestID; }
            set { requestID = value; }
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
        [Column(Name = "URL")]
        public string URL
        {
            get { return uRL; }
            set { uRL = value; }
        }

    }
}