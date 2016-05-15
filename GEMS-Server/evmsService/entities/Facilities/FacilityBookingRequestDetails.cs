using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "FacRequestsDetails")]
    public class FacilityBookingRequestDetails
    {
        int requestDetailsID;
        int requestid;
        int eventID;
        int priority;     
       
        Faculties fac;
        string facilityID;
        
        

        public FacilityBookingRequestDetails()
        {
        }
        public FacilityBookingRequestDetails(int reqid, int eventID, Faculties fac, string facilityID, int p)
        {
            this.requestid = reqid;
            this.priority = p;
            this.fac = fac;
            this.facilityID = facilityID;
            this.eventID = eventID;
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "ReqDetailID")]
        public int RequestDetailsID
        {
            get { return requestDetailsID; }
            private set { requestDetailsID = value; }
        }

        [DataMember]
        [Column(Name = "requestID")]
        public int Requestid
        {
            get { return requestid; }
            set { requestid = value; }
        }


        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return this.eventID; }
            set { this.eventID = value; }
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
        public string FacilityID
        {
            get { return facilityID; }
            set { facilityID = value; }
        }

        [DataMember]
        [Column(Name = "Priority")]
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

    }
}