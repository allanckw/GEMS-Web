using System;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.DataAccess;


namespace evmsService.entities
{
    public class SystemAdmin : User
    {
        public SystemAdmin(User user)
            : base(user)
        {

        }

        public bool AddRole(SysRole sr)
        {
            return SysAdminController.AddRole(this, sr);
        }

        public void RemoveRole(string userID)
        {
            SysAdminController.RemoveAllRoles(userID);
        }

        public bool AddFacilityAdmin(string useridToAssign, Faculties f)
        {
            
            return SysAdminController.AddFacilityAdmin(this, useridToAssign, f);
        }

        public bool RemoveFacilityAdmin(string userid)
        {
            return SysAdminController.RemoveFacilityAdmin(userid);
        }

    }

    public class SysAdminController
    {
        public static bool AddRole(SystemAdmin assigner, SysRole sr)
        {
            if (assigner.UserID == sr.UserID)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("You cannot Remove your own role!"));
            }

            DAL dalDataContext = new DAL();

            try
            {
                SysRole x = (from userRole in dalDataContext.sRole
                             where userRole.UserID == sr.UserID
                             select userRole).FirstOrDefault<SysRole>();

                if (x == null)
                {
                    Table<SysRole> sTable = dalDataContext.sRole;
                    sTable.InsertOnSubmit(sr);
                    sTable.Context.SubmitChanges();
                }
                else
                {
                    x.RoleLevel = sr.RoleLevel;
                    x.Remarks = sr.Remarks;
                    dalDataContext.SubmitChanges();
                }

                string msg = assigner.Name + " has assigned you to the role of " +
                    ((EnumRoles)sr.RoleLevel).ToString().Replace("_", " ") + Environment.NewLine + Environment.NewLine +
                    "Remarks: " + sr.Remarks;

                string title = "You have been Added to the System Group - " +
                    ((EnumRoles)sr.RoleLevel).ToString().Replace("_", " ");

                NotificationController.sendNotification(assigner.UserID, sr.UserID, title, msg);

                return true;
            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured while the system is Adding the user's role, Please Try Again!"));

            }
        }

        public static bool RemoveRole(string userID)
        {

            DAL dalDataContext = new DAL();

            try
            {
                SysRole role = (from roles in dalDataContext.sRole
                                where roles.UserID == userID
                                select roles).FirstOrDefault();

                if (role != null)
                {
                    dalDataContext.sRole.DeleteOnSubmit(role);
                    dalDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured while the system is removing the user's role, Please Try Again!"));

            }

        }

        public static bool AddFacilityAdmin(SystemAdmin assigner, string useridToAssign, Faculties f)
        {
            if (assigner.UserID == useridToAssign)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("You cannot Remove your own role!"));
            }
            else
            {
                RemoveAllRoles(useridToAssign); //Remove existing role 
                DAL dalDataContext = new DAL();
                try
                {
                    FacilityAdmin fa = (from facAdm in dalDataContext.facAdmins
                                        where facAdm.Faculty == f
                                        select facAdm).FirstOrDefault<FacilityAdmin>();

                    if (fa == null)
                    {
                        Table<FacilityAdmin> sTable = dalDataContext.facAdmins;
                        fa = new FacilityAdmin(useridToAssign, f);
                        sTable.InsertOnSubmit(fa);
                        sTable.Context.SubmitChanges();
                    }
                    else
                    {
                        fa.UserID = useridToAssign;
                        dalDataContext.SubmitChanges();
                    }

                    string msg = assigner.Name + " has assigned you to the role of Facility Admin for " +
                         f.ToString().Replace("_", " ");

                    string title = "You have been Added to the System Group - Facility Administrator";


                    NotificationController.sendNotification(assigner.UserID, useridToAssign, title, msg);

                    return true;
                }
                catch (Exception)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured while the system is Adding the user's role, Please Try Again!"));
                }
            }
        }

        public static bool RemoveFacilityAdmin(string userid)
        {
            DAL dalDataContext = new DAL();

            try
            {
                FacilityAdmin fa = (from facAdm in dalDataContext.facAdmins
                                    where facAdm.UserID == userid
                                    select facAdm).FirstOrDefault<FacilityAdmin>();

                if (fa != null)
                {
                    fa.UserID = "";
                    dalDataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured while the system is removing the user's role, Please Try Again!"));

            }
        }

        public static void RemoveAllRoles(string userid)
        {

            RemoveFacilityAdmin(userid);
            RemoveRole(userid);
        }
    }
}