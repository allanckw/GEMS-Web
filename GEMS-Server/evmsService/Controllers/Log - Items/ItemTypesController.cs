using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class ItemTypesController
    {

        public static ItemTypes AddItemType(int evid, string type, bool isImpt, DAL dalDataContext)
        {
           
            Table<ItemTypes> typeTable = dalDataContext.itemTypes;
            ItemTypes newItemType = new ItemTypes(evid, type, isImpt);


            typeTable.InsertOnSubmit(newItemType);
            typeTable.Context.SubmitChanges();
            return newItemType;
            
        }

        public static ItemTypes AddItemType(User user, int evid, string type, bool isImpt)
        {
            if (!user.isAuthorized( EventController.GetEvent(evid), EnumFunctions.Manage_ItemTypes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Items!"));
           
                ItemTypes iType = GetItemType(evid, type);
                if (iType == null)
                {
                    DAL dalDataContext = new DAL();
                    Table<ItemTypes> typeTable = dalDataContext.itemTypes;
                    ItemTypes newItemType = new ItemTypes(evid, type, isImpt);

                    typeTable.InsertOnSubmit(newItemType);
                    typeTable.Context.SubmitChanges();
                    return newItemType;
                }
                else
                {
                    throw new FaultException<SException>(new SException(),
                   new FaultReason("Item type already exist!"));
                }
        }

        public static ItemTypes GetItemType(int eventID, string typeName)
        {
            try
            {
                DAL dalDataContext = new DAL();

                ItemTypes matchedItem = (from iten in dalDataContext.itemTypes
                                         where iten.EventID == eventID &&
                                         iten.typeString == typeName
                                         select iten).FirstOrDefault<ItemTypes>();

                return matchedItem;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Getting Item Type, Please Try Again!"));
            }
        }

        public static void deleteItemType(User user, ItemTypes type)
        {

            if (!user.isAuthorized( EventController.GetEvent(type.EventID), EnumFunctions.Manage_ItemTypes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Guest!"));

            DAL dalDataContext = new DAL();

            try
            {
                ItemTypes itemType = (from iType in dalDataContext.itemTypes
                                      where iType.EventID == type.EventID
                                      && iType.typeString == type.typeString
                                      select iType).FirstOrDefault<ItemTypes>();


                dalDataContext.itemTypes.DeleteOnSubmit(itemType);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Item Type, Please Try Again!"));
            }
        }

        public static List<ItemTypes> GetExistingItemTypes(int eventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<ItemTypes> matchedItem = (from iten in dalDataContext.itemTypes
                                               where iten.EventID == eventID
                                               select iten).ToList<ItemTypes>();

                return matchedItem;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Getting Item Type, Please Try Again!"));
            }
        }

        public static void setItemTypeImportance(User user, ItemTypes type, bool isImpt)
        {

            if (!user.isAuthorized( EventController.GetEvent(type.EventID), EnumFunctions.Manage_ItemTypes))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit this Guest!"));
            try
            {
                DAL dalDataContext = new DAL();

                ItemTypes matchedItem = (from iten in dalDataContext.itemTypes
                                         where iten.EventID == type.EventID
                                         && iten.typeString == type.typeString
                                         select iten).FirstOrDefault<ItemTypes>();

                if (matchedItem == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Item Type"));
                }
                else
                {
                    matchedItem.IsImportantType = isImpt;
                    dalDataContext.SubmitChanges();

                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Updating Item Type, Please Try Again!"));
            }
        }

        public static int GetItemTypeCount(int eid)
        {
            DAL dalDataContext = new DAL();

            int itemTypeCount = dalDataContext.itemTypes.Count(s => s.EventID == eid);

            return itemTypeCount;
        }
    }
}
