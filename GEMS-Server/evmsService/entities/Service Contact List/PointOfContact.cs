using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "PointOfContact")]
    public class PointOfContact
    {
        private int serviceID;
        private int pointOfContactID;
        private string name;
        private string position;
        private string phone;//maybe contact better name?, string coz some overseas
        private string email;

        public PointOfContact() { }

        public PointOfContact(int serviceID, string name, string position, string phone, string email)
        {
            this.ServiceID = serviceID;
            this.Name = name;
            this.Position = position;
            this.Phone = phone;
            this.Email = email;
        }

        [DataMember]
        [Column(Name = "ServiceID")]
        public int ServiceID
        {
            get { return serviceID; }
            set { serviceID = value; }
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "PointOfContactID")]
        public int PointOfContactID
        {
            get { return pointOfContactID; }
            set { pointOfContactID = value; }
        }

        [DataMember]
        [Column(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        [Column(Name = "Position")]
        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        [DataMember]
        [Column(Name = "Phone")]
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        [DataMember]
        [Column(Name = "Email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        //name position phone email
    }
}