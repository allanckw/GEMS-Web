using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class UserController
    {

        public static User authenticate(Credentials credentials)
        {

            User user = null;
            DAL dalDataContext = new DAL();

            try
            {
                user = (from users in dalDataContext.users
                        where users.UserID == credentials.UserID
                        select users).FirstOrDefault<User>();
                if (user == null)
                {
                    throw new FaultException<SException>(new SException(),
                  new FaultReason("Invalid User, Please try again"));
                }
                else if (user.User_Password.CompareTo(KeyGen.Decrypt(credentials.Password)) != 0)
                {
                    throw new FaultException<SException>(new SException(),
                   new FaultReason("Wrong Password!, please try again"));
                }
                else
                {
                    user.GetSystemRole();
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An error had occured: " + ex.Message));
            }
            return user;
        }

        public static List<User> searchUsers(string name, string userid)
        {
            //where SqlMethods.Like(c.City, "L_n%")
            DAL dalDataContext = new DAL();
            List<User> uList;
            if (userid == null || userid.Length > 0)
            {
                uList = (from user in dalDataContext.users
                         where SqlMethods.Like(user.Name, "%" + name + "%") && user.UserID == userid
                         select user).ToList<User>();
            }
            else
            {
                uList = (from user in dalDataContext.users
                         where SqlMethods.Like(user.Name, "%" + name + "%")
                         select user).ToList<User>();
            }
            return uList;
        }

        public static List<User> searchUser(string name, string userid, EnumRoles r)
        {
            DAL dalDataContext = new DAL();
            List<User> uList;
            if (userid == null || userid.Length > 0)
            {
                if (r == EnumRoles.Nil)
                {
                    uList = (from user in dalDataContext.users
                             where SqlMethods.Like(user.Name, "%" + name + "%") && user.UserID == userid
                             && !(from role in dalDataContext.sRole select role.UserID).Contains(user.UserID)
                             && !(from facAdm in dalDataContext.facAdmins select facAdm.UserID).Contains(user.UserID)
                             select user).ToList<User>();
                }
                else if (r == EnumRoles.Facility_Admin)
                {
                    uList = (from user in dalDataContext.users
                             where SqlMethods.Like(user.Name, "%" + name + "%") && user.UserID == userid
                             && (from facAdm in dalDataContext.facAdmins select facAdm.UserID).Contains(user.UserID)
                             select user).ToList<User>();
                }
                else
                {
                    uList = (from user in dalDataContext.users
                             from sysRole in dalDataContext.sRole
                             where SqlMethods.Like(user.Name, "%" + name + "%") && user.UserID == userid
                             && user.UserID == sysRole.UserID && sysRole.RoleLevel == r
                             select user).ToList<User>();
                }
            }
            else
            {
                if (r == EnumRoles.Nil)
                {
                    uList = (from user in dalDataContext.users
                             where SqlMethods.Like(user.Name, "%" + name + "%")
                             && !(from role in dalDataContext.sRole select role.UserID).Contains(user.UserID)
                              && !(from facAdm in dalDataContext.facAdmins select facAdm.UserID).Contains(user.UserID)
                             select user).ToList<User>();
                }
                else if (r == EnumRoles.Facility_Admin)
                {
                    uList = (from user in dalDataContext.users
                             from facAdm in dalDataContext.facAdmins
                             where SqlMethods.Like(user.Name, "%" + name + "%")
                             && user.UserID == facAdm.UserID
                             select user).ToList<User>();
                }
                else
                {
                    uList = (from user in dalDataContext.users
                             from sysRole in dalDataContext.sRole
                             where SqlMethods.Like(user.Name, "%" + name + "%")
                             && user.UserID == sysRole.UserID && sysRole.RoleLevel == r
                             select user).ToList<User>();
                }
            }
            return uList;
        }

        public static string GetUserName(string userid)
        {
            return GetUser(userid).Name;
        }

        public static string GetUserMail(string userid)
        {
            return GetUser(userid).Email;
        }

        public static User GetUser(string userid)
        {
            DAL dalDataContext = new DAL();

            User user = (from users in dalDataContext.users
                      where users.UserID == userid
                      select users).FirstOrDefault<User>();

            return user;
        }
    }


}