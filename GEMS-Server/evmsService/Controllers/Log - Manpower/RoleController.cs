using System;
using System.Collections.Generic;
using System.Linq;
using evmsService.entities;
using evmsService.DataAccess;
using System.Data.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace evmsService.Controllers
{
    public class RoleController
    {
        public static Role AddRole(User user, string RoleUserID, int EventID, string RolePost, string RoleDescription)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<Role> roles = dalDataContext.roles;

                Role creatingRole = new Role(RolePost, RoleDescription, EventID, RoleUserID);

                roles.InsertOnSubmit(creatingRole);

                roles.Context.SubmitChanges();

                return creatingRole;

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role, Please Try Again!"));
            }
        }

        public static Role GetRole(int RoleID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Role existingRole = (from roles in dalDataContext.roles
                                     where roles.RoleID == RoleID
                                     select roles).FirstOrDefault();

                if (existingRole == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Role"));
                }
                return existingRole;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Role Data, Please Try Again!"));
            }
        }

        public static Role AddRole(string RoleUserID, int EventID, string RolePost, string RoleDescription, DAL dalDataContext)
        {
            try
            {
                Table<Role> roles = dalDataContext.roles;

                Role creatingRole = new Role(RolePost, RoleDescription, EventID, RoleUserID);
                roles.InsertOnSubmit(creatingRole);

                roles.Context.SubmitChanges();

                return creatingRole;

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role, Please Try Again!"));
            }
        }


        public static void DeleteRole(User user, int RoleID)
        {

            DAL dalDataContext = new DAL();

            try
            {
                Role matchedrole = (from roles in dalDataContext.roles
                                    where roles.RoleID == RoleID
                                    select roles).FirstOrDefault();


                dalDataContext.roles.DeleteOnSubmit(matchedrole);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Role, Please Try Again!"));
            }
        }

        public static void EditRole(int RoleID, string RoleUserID, string RolePost, string RoleDescription, DAL dalDataContext)
        {


            try
            {


                Role matchedrole = (from roles in dalDataContext.roles
                                    where roles.RoleID == RoleID
                                    select roles).FirstOrDefault();

                if (matchedrole == null)
                {

                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Role"));
                }
                else
                {
                    matchedrole.Description = RoleDescription;
                    matchedrole.Post = RolePost;
                    matchedrole.UserID = RoleUserID;

                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Role, Please Try Again!"));
            }
        }

        public static void EditRole(int RoleID, User user, string RoleUserID, int EventID, string RolePost, string RoleDescription)
        {


            try
            {
                DAL dalDataContext = new DAL();

                Role matchedrole = (from roles in dalDataContext.roles
                                    where roles.RoleID == RoleID
                                    select roles).FirstOrDefault();

                if (matchedrole == null)
                {

                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Role"));
                }
                else
                {
                    matchedrole.Description = RoleDescription;
                    matchedrole.Post = RolePost;
                    matchedrole.UserID = RoleUserID;

                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Role, Please Try Again!"));
            }
        }

        public static List<Role> ViewUserRolesForEvent(User user, Events evnt)
        {
            //chk weather got rights
            DAL dalDataContext = new DAL();

            List<Role> Roles = (from roles in dalDataContext.roles
                                where roles.EventID == evnt.EventID
                                select roles).ToList<Role>();

            return Roles;
        }

        public static List<Role> ViewUserEventRoles(string userID, int eventID)
        {

            DAL dalDataContext = new DAL();

            List<Role> allEventRoles = (from roles in dalDataContext.roles
                                        where roles.EventID == eventID &&
                                        roles.UserID == userID
                                        select roles).ToList<Role>();

            return allEventRoles;
        }

        public static List<RoleWithUser> ViewEventRoles(User user, Events evnt)
        {

            DAL dalDataContext = new DAL();
            List<RoleWithUser> eventRolesWithUsers = new List<RoleWithUser>();
            
            List<Role> allEventRoles = (from roles in dalDataContext.roles
                                where roles.EventID == evnt.EventID
                                select roles).ToList<Role>();
            
            for (int i = 0; i < allEventRoles.Count; i++)
            {
                string userName =  UserController.GetUserName(allEventRoles[i].UserID);

                eventRolesWithUsers.Add(new RoleWithUser(allEventRoles[i], userName + Environment.NewLine
                    + "Position: " + allEventRoles[i].Post));
            }
            
            return eventRolesWithUsers;
        }

        
    }

}