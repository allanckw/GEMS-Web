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
    public class RightController
    {
        public static void AddRight(User user, Role r, EnumFunctions f)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<Right> rights = dalDataContext.rights;

                Right creatingright = new Right(r.RoleID, f);
                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                rights.InsertOnSubmit(creatingright);
                rights.Context.SubmitChanges();
                //use this to Create rights //if error need to delete it
                //       throw new Exception();
                //tScope.Complete();
                // }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Right, Please Try Again!"));
            }
        }

        public static void AddRight(User user, List<Right> r, Role role)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();
                Table<Right> rights = dalDataContext.rights;

                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].RoleID = role.RoleID;
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
                   new FaultReason("An Error occured While Adding New Right, Please Try Again!"));
            }
        }

        public static void AddRight(User user, int roleID, List<EnumFunctions> functionID)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();
                Table<Right> rights = dalDataContext.rights;
                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                for (int i = 0; i < functionID.Count; i++)
                {
                    rights.InsertOnSubmit(new Right(roleID, functionID[i]));
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
                   new FaultReason("An Error occured While Adding New Right, Please Try Again!"));
            }
        }
        public static void AddRight(int roleID, List<EnumFunctions> functionID, DAL dalDataContext)
        {
            //chk if user can do this anot
            try
            {
                Table<Right> rights = dalDataContext.rights;
                //using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                //{
                for (int i = 0; i < functionID.Count; i++)
                {
                    rights.InsertOnSubmit(new Right(roleID, functionID[i]));
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
                   new FaultReason("An Error occured While Adding New Right, Please Try Again!"));
            }
        }

        public static void DeleteRight(int roleID, List<Right> Roles, DAL dalDataContext)
        {
           
            try
            {
                for (int i = 0; i < Roles.Count; i++)
                {
                    Right matchedright = (from rights in dalDataContext.rights
                                          where rights.RoleID == roleID &&
                                          rights.FunctionEnum == Roles[i].FunctionEnum
                                          select rights).FirstOrDefault();

                    dalDataContext.rights.DeleteOnSubmit(matchedright);
                }
                
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Right, Please Try Again!"));
            }
        }

        public static void DeleteRight(User user, Right r)
        {
            //chk if user can do this anot
            try
            {
                DAL dalDataContext = new DAL();
                Right matchedright = (from rights in dalDataContext.rights
                                      where rights.RoleID == r.RoleID &&
                                      rights.FunctionEnum == r.FunctionEnum
                                      select rights).FirstOrDefault();

                dalDataContext.rights.DeleteOnSubmit(matchedright);
                dalDataContext.SubmitChanges();
            }
            catch 
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Right, Please Try Again!"));
            }
        }

        public static List<Right> GetRight(int RoleID)
        {
            //chk weather got rights
            try
            {
                DAL dalDataContext = new DAL();

                List<Right> Rights = (from rights in dalDataContext.rights
                                      where rights.RoleID == RoleID
                                      select rights).ToList<Right>();

                return Rights;
            }
            catch (Exception)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Retrieving Rights, Please Try Again!"));
            }
        }
    }
}