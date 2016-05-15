using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "NUSNET")]
    public class User
    {
        private string name;
        private string email;
        private string uid;
        private string u_password;

        private Faculties faculty;


        private EnumRoles role;

        public User(User user)
        {
            this.name = user.name;
            this.email = user.email;
            this.u_password = user.u_password;
            this.uid = user.UserID;
        }

        public User()
        {
        }

        [DataMember]
        [Column(Name = "Name")]
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        [DataMember]
        [Column(Name = "email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "userid")]
        public string UserID
        {
            get { return uid; }
            private set { uid = value; }
        }


        [Column(Name = "userpwd")]
        public string User_Password
        {
            get { return u_password; }
            private set { u_password = value; }
        }

        [DataMember]
        [Column(Name = "Faculty", DbType = "NVarChar(150)")]
        public Faculties UserFaculty
        {
            get { return faculty; }
            private set { faculty = value; }
        }

        public EnumRoles Role
        {
            get { return role; }
            set { role = value; }
        }


        [DataMember]
        public bool isSystemAdmin
        {
            get
            {
                this.GetSystemRole();
                return (role == EnumRoles.System_Admin);
            }
            private set { }
        }

        [DataMember]
        public bool isFacilityAdmin
        {
            get
            {
                this.GetSystemRole();
                if (role == EnumRoles.System_Admin)
                    return true;
                else
                    return SysRoleController.isFacilityAdmin(this.UserID);
            }
            private set { }
        }

        [DataMember]
        public bool isEventOrganizer
        {
           
            get
            {
                this.GetSystemRole();
                return (role == EnumRoles.Event_Organizer);
            }
            private set { }
        }

        [DataMember]
        public bool isNormalUser
        {
            get
            {
                this.GetSystemRole();
                return (role == EnumRoles.Nil);
            }
            private set { }
        }

        public void GetSystemRole()
        {
            this.Role = SysRoleController.GetRole(UserID);

        }

        public bool isAuthorized(Events evnt, EnumFunctions functionENum)
        {
            if (isSystemAdmin)
                return true;
            else if (evnt.Organizerid.Equals(this.UserID, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return RoleLogicController.HaveRights(evnt, this, functionENum);

        }

        public bool isAuthorized(Events evnt)
        {
            if (isSystemAdmin)
                return true;
            else if (evnt.Organizerid.Equals(this.UserID, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }

        
    }
}