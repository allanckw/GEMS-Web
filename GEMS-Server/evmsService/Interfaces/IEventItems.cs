using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IEventItems
{
    #region "Item Type Repository"
    [OperationContract]
    [FaultContract(typeof(SException))]
    List<string> GetItemsTypes();

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AddItemsTypes(string itemType);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateItemTypes(string itemType, string newItemType);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteItemType(string itemType);
    #endregion

    #region Event Specific Item Type
    [OperationContract]
    [FaultContract(typeof(SException))]
    ItemTypes AddEventItemType(User user, int evid, string type, bool isImpt);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteEventItemType(User user, ItemTypes type);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SetItemTypeImportance(User user, ItemTypes type, Boolean isImpt);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<ItemTypes> GetEventSpecificItemType(int eventid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    ItemTypes GetEventItemType(int eventID, string typeName);

    #endregion

    #region Event Specific Item
    [OperationContract]
    [FaultContract(typeof(SException))]
    Items AddEventItem(User user, ItemTypes type, string name, int sat, decimal est);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteEventItem(User user, Items iten);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateSatifactionAndEstPrice(User user, Items iten, int s, decimal est);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Items> GetItemsByEvent(int eventID);

    #endregion
}

