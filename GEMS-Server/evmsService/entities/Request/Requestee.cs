using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Requestee")]
    public class Requestee
    {
        string targetEmail;
        string otp;

        

        public Requestee()
        {
        }

        public Requestee(string TargetEmail, string Otp)
        {
            this.TargetEmail = TargetEmail;
            this.Otp = Otp;
        }

        [DataMember]
        [Column(Name = "TargetEmail", IsPrimaryKey = true)]
        public string TargetEmail
        {
            get { return targetEmail; }
            set { targetEmail = value; }
        }

        [DataMember]
        [Column(Name = "OTP")]
        public string Otp
        {
            get { return otp; }
            set { otp = value; }
        }

    }
}