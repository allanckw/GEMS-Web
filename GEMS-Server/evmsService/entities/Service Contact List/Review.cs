using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Review")]
    public class Review
    {
        private int serviceID;//pk
        private string userName;//incase user is deleted

        private string userID;//pk , however if wanna allow user to Add more the one may need to use reviewID instead
        //private int reviewID;
        private int rating;
        private DateTime reviewDate;//date of rating creation, but should rating be editable?
        private string reviewDescription;//text for review description

        public Review() { }

        public Review(int serviceID, string userID,string userName, int rating, DateTime reviewDate, string reviewDescription) {
            this.ServiceID = serviceID;
            this.UserID = userID;
            this.Rating = rating;
            this.ReviewDate = reviewDate;
            this.ReviewDescription = reviewDescription;
            this.UserName = userName;
        }


        [DataMember]
        [Column(Name = "UserName")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "ServiceID")]
        public int ServiceID
        {
            get { return serviceID; }
            set { serviceID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "userID")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [DataMember]
        [Column(Name = "Rating")]
        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        [DataMember]
        [Column(Name = "ReviewDate")]
        public DateTime ReviewDate
        {
            get { return reviewDate; }
            set { reviewDate = value; }
        }

        [DataMember]
        [Column(Name = "ReviewDescription")]
        public string ReviewDescription
        {
            get { return reviewDescription; }
            set { reviewDescription = value; }
        }
        
    }
}