using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class ItemsController
    {

        public static Items AddItem(int eventID, string type, string name, int sat, decimal est, DAL dalDataContext)
        {
            Table<Items> itemTable = dalDataContext.items;
            Items newItem = new Items(eventID,type, name, sat, est);

            itemTable.InsertOnSubmit(newItem);
            itemTable.Context.SubmitChanges();

            return newItem;
        }

        public static Items AddItem(User user, ItemTypes type, string name, int sat, decimal est)
        {
            if (!user.isAuthorized( EventController.GetEvent(type.EventID), EnumFunctions.Manage_Items))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Items!"));

            if (GetItem(type, name) == null)
            {
                DAL dalDataContext = new DAL();
                Table<Items> itemTable = dalDataContext.items;
                Items newItem = new Items(type, name, sat, est);

                itemTable.InsertOnSubmit(newItem);
                itemTable.Context.SubmitChanges();

                return newItem;
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Item already exist!"));
            }

        }

        public static void deleteItem(User user, Items iten)
        {
            if (!user.isAuthorized( EventController.GetEvent(iten.EventID), EnumFunctions.Manage_Items))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Item!"));

            DAL dalDataContext = new DAL();

            try
            {
                Items matchedItem = (from item in dalDataContext.items
                                     where item.typeString == iten.typeString
                                     && item.EventID == iten.EventID
                                     && item.ItemName == iten.ItemName
                                     select item).FirstOrDefault<Items>();

                dalDataContext.items.DeleteOnSubmit(matchedItem);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Item , Please Try Again!"));
            }
        }

        public static void UpdateActualPrice(User user, Items iten, decimal actual)
        {
            if (!user.isAuthorized( EventController.GetEvent(iten.EventID), EnumFunctions.Manage_ItemTypes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Update Price!"));
            try
            {
                DAL dalDataContext = new DAL();

                Items matchedItem = (from item in dalDataContext.items
                                     where item.typeString == iten.typeString
                                     && item.EventID == iten.EventID
                                    && item.ItemName == iten.ItemName
                                     select item).FirstOrDefault<Items>();

                if (matchedItem == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Item "));
                }
                else
                {
                    matchedItem.ActualPrice = actual;
                    dalDataContext.SubmitChanges();
                    BudgetDetailsController.SetBought(user, iten);
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Updating Item, Please Try Again!"));
            }
        }

        public static void UpdateSatifactionAndEstPrice(User user, Items iten, int sat, decimal est)
        {
            if (!user.isAuthorized( EventController.GetEvent(iten.EventID), EnumFunctions.Manage_Items))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Update this Item!"));
            try
            {
                DAL dalDataContext = new DAL();

                Items matchedItem = (from item in dalDataContext.items
                                     where item.typeString == iten.typeString
                                    && item.EventID == iten.EventID
                                     && item.ItemName == iten.ItemName
                                     select item).FirstOrDefault<Items>();

                if (matchedItem == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Item "));
                }
                else
                {
                    matchedItem.Satisfaction = sat;
                    matchedItem.EstimatedPrice = est;
                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Updating Item, Please Try Again!"));
            }
        }

        public static List<Items> GetItemsList(ItemTypes type)
        {
            DAL dalDataContext = new DAL();

            List<Items> itemList = (from iten in dalDataContext.items
                                    where iten.EventID == type.EventID
                                    && iten.typeString == type.typeString
                                    select iten).ToList<Items>();
            return itemList;
        }

        public static List<Items> GetItemsList(int eventID)
        {
            DAL dalDataContext = new DAL();

            List<Items> itemList = (from iten in dalDataContext.items
                                    where iten.EventID == eventID
                                    select iten).ToList<Items>();
            return itemList;
        }

        public static int GetItemCount(int eid)
        {
            DAL dalDataContext = new DAL();

            int itemTypeCount = dalDataContext.items.Count(s => s.EventID == eid);

            return itemTypeCount;
        }

        private static Items GetItem(ItemTypes type, string name)
        {
            DAL dalDataContext = new DAL();

            Items matchedItem = (from item in dalDataContext.items
                                 where item.typeString == type.typeString
                                 && item.EventID == type.EventID
                                 && item.ItemName == name
                                 select item).FirstOrDefault<Items>();
            return matchedItem;
        }

        public static Items GetItem(OptimizedBudgetItemsDetails bItem)
        {
            DAL dalDataContext = new DAL();

            Items matchedItem = (from item in dalDataContext.items
                                 where item.typeString == bItem.typeString
                                 && item.EventID == bItem.EventID
                                 && item.ItemName == bItem.ItemName
                                 select item).FirstOrDefault<Items>();
            return matchedItem;
        }
    }
}