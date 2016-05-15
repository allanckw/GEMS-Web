using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using evmsService.entities;
using evmsService.DataAccess;
using System.Data.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Transactions;

namespace evmsService.Controllers
{
    public class RoleLogicController
    {
        //not prefered as assumed user can make object.. unless use from previously returned

        public static bool isEventFacilitator(int eventID, string userid)
        {

            if (UserController.GetUser(userid).isAuthorized(EventController.GetEvent(eventID)))
            {
                return true;
            }
            else
            {
                try
                {
                    DAL dalDataContext = new DAL();

                    Events e = (from events in dalDataContext.events
                                join roles in dalDataContext.roles on
                                events.EventID equals roles.EventID
                                where
                                events.EventID == eventID &&
                                roles.UserID == userid
                                select events).Union(
                                from events in dalDataContext.events
                                where
                                events.EventID == eventID &&
                                events.Organizerid == userid
                                orderby events.StartDateTime ascending
                                select events).Distinct().SingleOrDefault();

                    if (e == null)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured when checking if user is a facilitator, Please Try Again!"));
                }
            }

        }

        public static void DeleteRole(User user, int RoleID)
        {
            if (!user.isAuthorized(EventController.GetEvent(RoleController.GetRole(RoleID).EventID), EnumFunctions.Add_Role))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Delete This Role!"));

            RoleController.DeleteRole(user, RoleID);
        }

        public static int AddRoleAndRights(User user, string RoleUserID, int EventID, string RolePost, string RoleDescription, List<EnumFunctions> functionID)
        {
            if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Add_Role))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add New Role!"));

            try
            {

                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL dalDataContext = new DAL();

                    Role role = RoleController.AddRole(RoleUserID, EventID, RolePost, RoleDescription, dalDataContext);
                    int roleid = role.RoleID;
                    role = null;

                    RightController.AddRight(roleid, functionID, dalDataContext);

                    NotificationController.sendNotification(user.UserID, RoleUserID, "Rights changed",
                        "Your Rights Have been changed for the Event " + EventController.GetEvent(EventID).Name);

                    t.Complete();
                    return roleid;
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role, Please Try Again!"));
            }
        }
        public static bool HaveRights(Events evnt, User user, EnumFunctions FunctionEnum)
        {
            if (evnt.Organizerid == user.UserID)
                return true;
            List<EnumFunctions> rights = GetRights(evnt.EventID, user.UserID);
            for (int i = 0; i < rights.Count; i++)
            {
                if (rights[i] == FunctionEnum)
                    return true;
            }
            return false;
        }

        public static void EditRole(User user, string RoleUserID, int RoleID,
            string RolePost, string RoleDescription, List<EnumFunctions> FunctionEnums)
        {
            if (!user.isAuthorized(EventController.GetEvent(RoleController.GetRole(RoleID).EventID), EnumFunctions.Add_Role))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Edit Role!"));
            try
            {
                List<Right> RoleUserRights = RightController.GetRight(RoleID);
                List<EnumFunctions> RightsToAdd = new List<EnumFunctions>();
                List<Right> RightsToDelete = new List<Right>();

                for (int i = 0; i < RoleUserRights.Count; i++)
                {
                    for (int j = 0; j < FunctionEnums.Count; j++)
                    {
                        if (RoleUserRights[i].FunctionEnum == FunctionEnums[j])
                        {
                            goto delbreakouter;
                        }
                    }
                    RightsToDelete.Add(RoleUserRights[i]);
                    continue;
                delbreakouter:
                    continue;
                }

                for (int i = 0; i < FunctionEnums.Count; i++)
                {
                    for (int j = 0; j < RoleUserRights.Count; j++)
                    {
                        if (FunctionEnums[i] == RoleUserRights[j].FunctionEnum)
                        {
                            goto Addbreakouter;
                        }
                    }
                    RightsToAdd.Add(FunctionEnums[i]);
                    continue;
                Addbreakouter:
                    continue;
                }

                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL dalDataContext = new DAL();

                    RoleController.EditRole(RoleID, RoleUserID, RolePost, RoleDescription, dalDataContext);

                    //delete rights
                    RightController.DeleteRight(RoleID, RightsToDelete, dalDataContext);

                    //Add new rights
                    RightController.AddRight(RoleID, RightsToAdd, dalDataContext);

                    NotificationController.sendNotification(user.UserID, RoleUserID, "Rights changed",
                        "Your Rights Have been changed for the Event " +
                        EventController.GetEvent(RoleController.GetRole(RoleID).EventID).Name);

                    t.Complete();
                }
            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Right, Please Try Again!"));
            }
        }

        public static List<EnumFunctions> GetRights(int eventID, string userID)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<Right> rights = dalDataContext.rights;


                List<Right> matchedrights = (from role in dalDataContext.roles
                                             from right in dalDataContext.rights
                                             where role.RoleID == right.RoleID
                                             && role.EventID == eventID
                                             && role.UserID == userID
                                             select right).ToList<Right>();
                List<EnumFunctions> enumfuns = new List<EnumFunctions>();
                for (int i = 0; i < matchedrights.Count; i++)
                {
                    enumfuns.Add(matchedrights[i].FunctionEnum);
                }
                return enumfuns;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Retrieving Right, Please Try Again!"));
            }

        }

    }
}