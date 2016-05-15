using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "FacilityAdmins")]
    public class FacilityAdmin
    {

        private string userid;

        private Faculties fac;

        public FacilityAdmin(string userid, Faculties fac)
        {
            this.userid = userid;
            this.fac = fac;
        }
        
        public FacilityAdmin()
        {

        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "Faculty", DbType = "NVarChar(150)")]
        public Faculties Faculty
        {
            get { return fac; }
            set { fac = value; }
        }

        [DataMember]
        [Column(Name = "userid")]
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
    }
}