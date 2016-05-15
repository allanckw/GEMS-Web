using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IGuest
{
    #region "Guests Management"

    [OperationContract]
    [FaultContract(typeof(SException))]
    int AddGuest(User user, int dayID, string GuestName, string GuestContact, string GuestDescription);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Guest> ViewGuest(int dayID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditGuest(User user, int GuestID, string GuestName, string GuestDescription, string GuestContact);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteGuest(User user, int GuestID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<int> GetEventGuestCount(int eventID);
    #endregion
}

