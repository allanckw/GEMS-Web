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
    public class BudgetDetailsController
    {
        public static List<OptimizedBudgetItemsDetails> GetBudgetItems(OptimizedBudgetItems Budget)
        {
            DAL dalDataContext = new DAL();
            List<OptimizedBudgetItemsDetails> BudgetItems;

            BudgetItems = (from BudgetDetails in dalDataContext.optimizedBudgetItemDetails
                              where BudgetDetails.BudgetID == Budget.BudgetID
                              && BudgetDetails.EventID == Budget.EventID
                              orderby BudgetDetails.typeString ascending
                              select BudgetDetails).ToList<OptimizedBudgetItemsDetails>();

            return BudgetItems;
        }

        public static void AddBudgetItems(int BudgetId, List<Items> itemList)
        {
            DAL dalDataContext = new DAL();

            List<OptimizedBudgetItemsDetails> bItemList = new List<OptimizedBudgetItemsDetails>();
            try
            {
                foreach (Items item in itemList)
                {
                    OptimizedBudgetItemsDetails bItem = new OptimizedBudgetItemsDetails(
                        BudgetId, item.EventID, item.typeString, item.ItemName);
                    bItemList.Add(bItem);
                }

                Table<OptimizedBudgetItemsDetails> budItems = dalDataContext.optimizedBudgetItemDetails;
                budItems.InsertAllOnSubmit(bItemList);
                budItems.Context.SubmitChanges();
            }
            catch (TransactionAbortedException tAbortedex)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Saving Optimal Budget List: " + tAbortedex.Message));
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason("An Error occured While Saving Optimal Budget List: " + ex.Message));
            }
        }

        public static void SetBought(User user, Items iten)
        {
            //if (!user.isAuthorized(user, EventController.GetEvent(iten.EventID), EnumFunctions.Manage_Items))
            //    throw new FaultException<SException>(new SException(),
            //       new FaultReason("Invalid User, User Does Not Have Rights To Update Items properties!"));
            try
            {
                DAL dalDataContext = new DAL();

                OptimizedBudgetItemsDetails matchedItem = (from item in dalDataContext.optimizedBudgetItemDetails
                                     where item.typeString == iten.typeString
                                     && item.EventID == iten.EventID
                                    && item.ItemName == iten.ItemName
                                     select item).FirstOrDefault<OptimizedBudgetItemsDetails>();

                if (matchedItem == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Item "));
                }
                else
                {
                    matchedItem.IsBought = true;
                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Updating Item, Please Try Again!"));
            }
        }

    }
}