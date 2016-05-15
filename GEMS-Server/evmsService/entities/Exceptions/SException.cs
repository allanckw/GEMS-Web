using System;
using System.Text;
using System.Runtime.Serialization;

namespace evmsService.Controllers
{
    [DataContract]
    public class SException
    {
        public SException(string reason)
        {
            this.reason = reason;
        }
        public SException()
        {
        }

        private string reason;

        [DataMember]
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
    }
}
