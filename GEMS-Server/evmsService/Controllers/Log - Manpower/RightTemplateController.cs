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
    public class RightTemplateController
    {
        public static void AddRight(User user, RoleTemplate r, Function f)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<RightTemplate> rights = dalDataContext.rightTemplate;

                RightTemplate creatingright = new RightTemplate(r.RoleTemplateID, f.FunctionEnum);

                rights.InsertOnSubmit(creatingright);
                rights.Context.SubmitChanges();

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Right Template, Please Try Again!"));
            }
        }

        public static void AddRight(User user, List<RightTemplate> r, RoleTemplate role)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();
                Table<RightTemplate> rights = dalDataContext.rightTemplate;

                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].RoleTemplateID = role.RoleTemplateID;
                    rights.InsertOnSubmit(r[i]);
                }

                rights.Context.SubmitChanges();
                //use this to Create rights //if error need to delete it
                //       throw new Exception();
                //tScope.Complete();
                // }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Right Template, Please Try Again!"));
            }
        }

        public static void AddRight(User user, int roleTemplateID, List<EnumFunctions> functionID)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();
                Table<RightTemplate> rights = dalDataContext.rightTemplate;
                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                for (int i = 0; i < functionID.Count; i++)
                {
                    rights.InsertOnSubmit(new RightTemplate(roleTemplateID, functionID[i]));
                }

                rights.Context.SubmitChanges();
                //use this to Create rights //if error need to delete it
                //       throw new Exception();
                //tScope.Complete();
                // }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Right Template, Please Try Again!"));
            }
        }
        public static void AddRight(int roleTemplateID, List<EnumFunctions> functionID, DAL dalDataContext)
        {

            try
            {
                Table<RightTemplate> rights = dalDataContext.rightTemplate;
                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                for (int i = 0; i < functionID.Count; i++)
                {
                    rights.InsertOnSubmit(new RightTemplate(roleTemplateID, functionID[i]));
                }

                rights.Context.SubmitChanges();
                //use this to Create rights //if error need to delete it
                //       throw new Exception();
                //tScope.Complete();
                // }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Right Template, Please Try Again!"));
            }
        }

        public static void DeleteRight(int roleTemplateID, List<RightTemplate> Roles, DAL dalDataContext)
        {

            try
            {
                for (int i = 0; i < Roles.Count; i++)
                {
                    RightTemplate matchedright = (from rights in dalDataContext.rightTemplate
                                                  where rights.RoleTemplateID == roleTemplateID &&
                                                rights.FunctionEnum == Roles[i].FunctionEnum
                                                  select rights).FirstOrDefault();

                    dalDataContext.rightTemplate.DeleteOnSubmit(matchedright);
                }

                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Right Template, Please Try Again!"));
            }
        }

        public static void DeleteRight(User user, RightTemplate r)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();
                RightTemplate matchedright = (from rights in dalDataContext.rightTemplate
                                              where rights.RoleTemplateID == r.RoleTemplateID &&
                                            rights.FunctionEnum == r.FunctionEnum
                                              select rights).FirstOrDefault();

                dalDataContext.rightTemplate.DeleteOnSubmit(matchedright);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Right Template, Please Try Again!"));
            }
        }

        public static List<RightTemplate> GetTemplateRights(int RoleTemplateID)
        {
            //chk weather got rights
            try
            {
                DAL dalDataContext = new DAL();

                List<RightTemplate> Rights = (from rights in dalDataContext.rightTemplate
                                              where rights.RoleTemplateID == RoleTemplateID
                                              select rights).ToList<RightTemplate>();

                return Rights;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Retrieving Rights Template, Please Try Again!"));
            }
        }
    }
}