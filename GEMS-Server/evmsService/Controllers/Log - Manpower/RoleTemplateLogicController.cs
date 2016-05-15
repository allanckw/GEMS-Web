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
    public class RoleTemplateLogicController
    {
        //not prefered as assumed user can make object.. unless use from previously returned

        public static void DeleteRoleTemplate(User user, int RoleTemplateID)
        {//for now i use Add role enum may need to make template for Add_Role_Template
            if (!user.isSystemAdmin)
            {
                if (!user.isAuthorized(EventController.GetEvent(RoleTemplateController.GetRoleTemplate(RoleTemplateID).EventID.Value), EnumFunctions.Add_Role))
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Delete This Role Template!"));
            }

            RoleTemplateController.DeleteRoleTemplate(user, RoleTemplateID);
        }

        public static int AddRightsTemplate(User user, Events evnt, string RoleTemplatePost, string RoleTemplateDescription, List<EnumFunctions> functionID)
        {
            if (!user.isSystemAdmin)
            {
                if (!user.isAuthorized(evnt, EnumFunctions.Add_Role))
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Add New Role Template!"));
            }
            try
            {

                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL dalDataContext = new DAL();

                    RoleTemplate role = RoleTemplateController.AddRoleTemplate(evnt, RoleTemplatePost, RoleTemplateDescription, dalDataContext);
                    int roleid = role.RoleTemplateID;
                    role = null;

                    RightTemplateController.AddRight(roleid, functionID, dalDataContext);
                    t.Complete();
                    return roleid;
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role Template, Please Try Again!"));
            }
        }


        public static void EditRightsTemplate(User user, int RoleTemplateID, string RoleTemplatePost, string RoleTemplateDescription, List<EnumFunctions> functionID)
        {
            if (!user.isSystemAdmin)
                if (!user.isAuthorized( EventController.GetEvent(RoleTemplateController.GetRoleTemplate(RoleTemplateID).EventID.Value), EnumFunctions.Add_Role))
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Add Edit Role Template!"));
            try
            {
                List<RightTemplate> RoleUserRights = RightTemplateController.GetTemplateRights(RoleTemplateID);
                List<EnumFunctions> RightsToAdd = new List<EnumFunctions>();
                List<RightTemplate> RightsToDelete = new List<RightTemplate>();

                for (int i = 0; i < RoleUserRights.Count; i++)
                {
                    for (int j = 0; j < functionID.Count; j++)
                    {
                        if (RoleUserRights[i].FunctionEnum == functionID[j])
                        {
                            goto delbreakouter;
                        }
                    }
                    RightsToDelete.Add(RoleUserRights[i]);
                    continue;
                delbreakouter:
                    continue;
                }

                for (int i = 0; i < functionID.Count; i++)
                {
                    for (int j = 0; j < RoleUserRights.Count; j++)
                    {
                        if (functionID[i] == RoleUserRights[j].FunctionEnum)
                        {
                            goto Addbreakouter;
                        }
                    }
                    RightsToAdd.Add(functionID[i]);
                    continue;
                Addbreakouter:
                    continue;
                }


                using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL dalDataContext = new DAL();

                    RoleTemplateController.EditRoleTemplate(RoleTemplateID, RoleTemplatePost, RoleTemplateDescription, dalDataContext);
                    //Role role = RoleController.AddRole(u, RoleUserID, EventID, RolePost, RoleDescription, dalDataContext);
                    //int roleid = role.RoleID;
                    //role = null;

                    //delete rights
                    RightTemplateController.DeleteRight(RoleTemplateID, RightsToDelete, dalDataContext);

                    //Add new rights
                    RightTemplateController.AddRight(RoleTemplateID, RightsToAdd, dalDataContext);
                    t.Complete();

                }


            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Right Template, Please Try Again!"));
            }
        }
        //public static List<RightTemplate> GetRights(int EventID)
        //{
        //    try
        //    {
        //        DAL dalDataContext = new DAL();
        //        Table<Right> rights = dalDataContext.rights;


        //        List<RightTemplate> matchedrights = (from role in dalDataContext.roleTemplate
        //                                             from right in dalDataContext.rightTemplate
        //                                             where role.RoleTemplateID == right.RoleTemplateID
        //                                            && role.EventID == EventID

        //                                             select right).ToList<RightTemplate>();



        //        return matchedrights;

        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("An Error occured While Retrieving Right Template, Please Try Again!"));
        //    }

        //}
        //public static List<Right> GetRights(Event evnt, User user)
        //{
        //    try
        //    {
        //        DAL dalDataContext = new DAL();
        //        Table<Right> rights = dalDataContext.rights;


        //        List<Right> matchedrights = (from role in dalDataContext.roles
        //                                     from right in dalDataContext.rights
        //                                     where role.RoleID == right.RoleID
        //                                     && role.EventID == evnt.EventID
        //                                     && role.UserID == user.userID
        //                                     select right).ToList<Right>();



        //        return matchedrights;

        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("An Error occured While Retrieving Right, Please Try Again!"));
        //    }

        //}
    }
}