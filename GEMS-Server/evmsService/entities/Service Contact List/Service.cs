using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Service")]
    public class Service
    {
        private int serviceID;//pk
        private string address;//Address of the service
        private string name;//name of the service
        private string url;
        private string notes;//descript the service provided

        public Service() { }
        public Service(string Address, string name, string url, string notes)
        {
            this.Address = Address;
            this.Name = name;
            this.Url = url;
            this.Notes = notes;
        }
        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "serviceID")]
        public int ServiceID
        {
            get { return serviceID; }
            set { serviceID = value; }
        }

        [DataMember]
        [Column(Name = "Address")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        [Column(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        [Column(Name = "Url")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        [DataMember]
        [Column(Name = "Notes")]
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

    }
}