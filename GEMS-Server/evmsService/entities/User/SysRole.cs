using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "sysRoles")]
    public class SysRole
    {
        private string userID;
        private EnumRoles role;
        private string remarks;

        public SysRole() { }

        public SysRole(string uid, EnumRoles role, string remarks)
        {
            this.userID = uid;
            this.role = role;
            this.remarks = remarks;
        }

        [Column(IsPrimaryKey = true, Name = "userID")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [DataMember]
        [Column(Name = "Role")]
        public EnumRoles RoleLevel
        {
            get { return role; }
            set { role = value; }
        }

        [DataMember]
        [Column(Name = "Remark")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }


    }
}