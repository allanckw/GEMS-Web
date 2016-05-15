using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class BudgetItemController
    {
        public static void SaveBudgetItemList(User saver, int eventID, int totalSat,
            decimal totalPrice, List<Items> itemList)
        {
            //Check if user has rights to manage Budget
            //if no throw exception

            try
            {
                DAL dalDataContext = new DAL();
                Table<OptimizedBudgetItems> facReq = dalDataContext.optimizedBudgetItems;
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    RemoveExistingBudget(eventID); //Remove if it exist, causing overwrite

                    //Create a new one
                    OptimizedBudgetItems Budget = new OptimizedBudgetItems(eventID, saver.UserID, totalSat, totalPrice);

                    facReq.InsertOnSubmit(Budget);
                    facReq.Context.SubmitChanges();

                    Budget = (from bgt in dalDataContext.optimizedBudgetItems
                              where bgt.EventID == eventID &&
                              bgt.Generator == saver.UserID
                              orderby bgt.GeneratedDate descending
                              select bgt).FirstOrDefault<OptimizedBudgetItems>();

                    BudgetDetailsController.AddBudgetItems(Budget.BudgetID, itemList);

                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason("An Error occured While Saving Optimal Item List: " + ex.Message));
            }

        }

        private static void RemoveExistingBudget(int eventID)
        {
             DAL dalDataContext = new DAL();
            OptimizedBudgetItems matchedBudget = (from bgt in dalDataContext.optimizedBudgetItems
                                    where bgt.EventID == eventID
                                    orderby bgt.GeneratedDate descending
                                    select bgt).FirstOrDefault<OptimizedBudgetItems>();

            if (matchedBudget != null)
            {
                dalDataContext.optimizedBudgetItems.DeleteOnSubmit(matchedBudget);
                dalDataContext.SubmitChanges();
                //Child will automatically be deleted due to cascade from sql
            }
        }

        public static OptimizedBudgetItems GetBudget(int eventID)
        {
            //Check if user has rights to manage Budget
            //if no throw exception
            DAL dalDataContext = new DAL();
            OptimizedBudgetItems matchedBudget = (from bgt in dalDataContext.optimizedBudgetItems
                                    where bgt.EventID == eventID
                                    orderby bgt.GeneratedDate descending
                                    select bgt).FirstOrDefault<OptimizedBudgetItems>();

            return matchedBudget;
        }

       
    }
}
