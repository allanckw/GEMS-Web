using System;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class SysRoleController
    {

        public static EnumRoles GetRole(string uid)
        {
            SysRole uRole = new SysRole();

            if (isFacilityAdmin(uid))
            {
                return EnumRoles.Facility_Admin;
            }
            DAL dalDataContext = new DAL();

            uRole = (from userRole in dalDataContext.sRole
                     where userRole.UserID == uid
                     select userRole).FirstOrDefault<SysRole>();

            if (uRole == null)
            {
                return EnumRoles.Nil;
            }
            else
            {
                return (EnumRoles)uRole.RoleLevel;
            }
        }

        public static void AddRole(User assigner, SysRole sr)
        {
            if (assigner.isSystemAdmin)
            {
                SystemAdmin admin = new SystemAdmin(assigner);
                admin.AddRole(sr);
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("You do not have the permission to perform this task!"));

            }
        }

        public static void RemoveRole(User assigner, string userid)
        {
            if (assigner.UserID == userid)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("You cannot Remove your own role!"));
            }
            else  if (assigner.isSystemAdmin)
            {
                SystemAdmin admin = new SystemAdmin(assigner);
                admin.RemoveRole(userid);
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("You do not have the permission to perform this task!"));
            }
        }

        public static void AddFacilityAdmin(User assigner, string userid, Faculties fac)
        {
            if (assigner.isSystemAdmin)
            {
                SystemAdmin admin = new SystemAdmin(assigner);
                admin.AddFacilityAdmin(userid, fac);
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("You do not have the permission to perform this task!"));
            }
        }

        public static void RemoveFacilityAdmin(User assigner, string userid)
        {
            if (assigner.isSystemAdmin)
            {
                SystemAdmin admin = new SystemAdmin(assigner);

                admin.RemoveFacilityAdmin(userid);
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("You do not have the permission to perform this task!"));
            }
        }

        public static bool isFacilityAdmin(string userID)
        {
            DAL dalDataContext = new DAL();

            FacilityAdmin fa = (from facAdm in dalDataContext.facAdmins
                                where facAdm.UserID == userID
                                select facAdm).FirstOrDefault<FacilityAdmin>();

            return (fa != null);
        }

        public static string GetFacilityAdmin(Faculties f)
        {
            DAL dalDataContext = new DAL();
            FacilityAdmin fa = (from facAdm in dalDataContext.facAdmins
                                where facAdm.Faculty == f
                                select facAdm).FirstOrDefault<FacilityAdmin>();

            if (fa == null || fa.UserID.Trim().Length == 0)
            {
                //if faculty does not have an admin, sys admin take over
                User user = (from users in dalDataContext.users
                          from sysRole in dalDataContext.sRole
                          where users.UserID == sysRole.UserID && 
                          sysRole.RoleLevel == (int)EnumRoles.System_Admin
                          select users).FirstOrDefault<User>();
                return user.UserID;
            }
            else
            {
                //Return the faculty admin
                return fa.UserID;
            }
        }

        public static Faculties GetFaculty(string userid)
        {

            DAL dalDataContext = new DAL();
            FacilityAdmin fa = (from facAdm in dalDataContext.facAdmins
                                where facAdm.UserID == userid
                                select facAdm).FirstOrDefault<FacilityAdmin>();

            if (fa == null || fa.UserID.Trim().Length == 0)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("User is not a facility administrator!"));
            }
            else
            {
                //Return the faculty admin
                return fa.Faculty;
            }
        }

        public static string GetFacultyAdmin(Faculties fac)
        {
            DAL dalDataContext = new DAL();
            FacilityAdmin fa = (from facAdm in dalDataContext.facAdmins
                                where facAdm.Faculty == fac
                                select facAdm).FirstOrDefault<FacilityAdmin>();

            if (fa == null || fa.UserID.Trim().Length == 0)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("There is no facility admin for the defined faculty!"));
            }
            else
            {
                //Return the faculty admin
                return fa.UserID;
            }
        }



    }
}