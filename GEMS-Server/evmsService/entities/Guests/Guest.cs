using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Guests")]
    public class Guest
    {
        private int guestID;
        private string name;
        private string contact;
        private string description;
        private int dayID;

        public Guest(string GuestName, string GuestContact, string GuestDescription, int DayID)
        {
            name = GuestName;
            contact = GuestContact;
            description = GuestDescription;
            dayID = DayID;
        }
        public Guest()
        {
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "GuestID")]
        public int GuestId
        {
            get { return guestID; }
            set { guestID = value; }
        }

        [DataMember]
        [Column(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        [Column(Name = "Contact")]
        public string Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        [DataMember]
        [Column(Name = "DayID")]
        public int DayID
        {
            get { return dayID; }
            set { dayID = value; }
        }

        [DataMember]
        public int EventID
        {
            private set { }
            get
            {
                return  DayController.GetEventID(this);
            }
        }

    }
}