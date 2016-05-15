using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "NUSFacilities")]
    public class Facility
    {
        private string facilityID;
        private Faculties fac;
        private string location;
        private string bookingContact;
        private string techContact;
        private int capacity;


        //Added in V0.3
        private RoomType rType;
        private bool webcast, flexibleSeating, videoConferencing, mic, projector, visualizer;

        public Facility(string id, Faculties fac, string loc, string bookingCon, string techCon, int cap,
            RoomType rType, bool hasWebCast, bool flexiSeat, bool vidConf, bool mic, bool projector, bool visualizer)
        {
            this.bookingContact = bookingCon;
            this.facilityID = id;
            this.fac = fac;
            this.location = loc;
            this.techContact = techCon;
            this.bookingContact = bookingCon;
            this.capacity = cap;
            this.rType = rType;
            this.webcast = hasWebCast;
            this.flexibleSeating = flexiSeat;
            this.videoConferencing = vidConf;
            this.mic = mic;
            this.projector = projector;
            this.visualizer = visualizer;
        }

        public Facility()
        {
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "FacilityID")]
        public string FacilityID
        {
            get { return facilityID; }
            set { facilityID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "Faculty", DbType = "NVarChar(150)")]
        public Faculties Faculty
        {
            get { return fac; }
            set { fac = value; }
        }

        [DataMember]
        [Column(Name = "RoomType", DbType = "INT")]
        public RoomType RoomType
        {
            get { return rType; }
            set { rType = value; }
        }

        [DataMember]
        [Column(Name = "Location")]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        [DataMember]
        [Column(Name = "BookingContact")]
        public string BookingContact
        {
            get { return bookingContact; }
            set { bookingContact = value; }
        }

        [DataMember]
        [Column(Name = "TechContact")]
        public string TechContact
        {
            get { return techContact; }
            set { techContact = value; }
        }

        [DataMember]
        [Column(Name = "Capacity")]
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        [DataMember]
        [Column(Name = "HasWebCast")]
        public bool HasWebCast
        {
            get { return webcast; }
            set { webcast = value; }
        }

        [DataMember]
        [Column(Name = "HasFlexibleSeating")]
        public bool HasflexibleSeating
        {
            get { return flexibleSeating; }
            set { flexibleSeating = value; }
        }

        [DataMember]
        [Column(Name = "HasVideoConferencing")]
        public bool HasVideoConferencing
        {
            get { return videoConferencing; }
            set { videoConferencing = value; }
        }

        [DataMember]
        [Column(Name = "HasMicrophone")]
        public bool HasMicrophone
        {
            get { return mic; }
            set { mic = value; }
        }

        [DataMember]
        [Column(Name = "Hasprojector")]
        public bool HasProjector
        {
            get { return projector; }
            set { projector = value; }
        }

        [DataMember]
        [Column(Name = "HasVisualizer")]
        public bool HasVisualizer
        {
            get { return visualizer; }
            set { visualizer = value; }
        }
    }
}