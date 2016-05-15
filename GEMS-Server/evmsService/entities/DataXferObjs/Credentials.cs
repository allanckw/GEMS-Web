
using System.Runtime.Serialization;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    public class Credentials
    {
        private string uid;
        private string u_password;

        [DataMember]
        public string UserID
        {
            get { return uid; }
            set { uid = value; }
        }

         [DataMember]
        public string Password
        {
            get { return u_password; }
            set { u_password = value; }
        }
    }
}