using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;


[ServiceContract]
interface IServiceContact
{

    #region "Manage Services"
    [OperationContract]
    [FaultContract(typeof(SException))]
    void AddService(User user, int eventID, string Address, string name, string url, string notes);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditService(int ServiceID, User user, int eventID, string Address, string name, string url, string notes);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteService(User user, int serviceID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Service> ViewService(string SearchString);

    #endregion

    #region "Manage Point Of Contact"
    [OperationContract]
    [FaultContract(typeof(SException))]
    void AddPointOfContact(User user, int eventID, int serviceID, string name, string position, string phone, string email);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditPointOfContact(User user, int eventID, int PointOfContactID, string name, string position, string phone, string email);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeletePointOfContact(User user, int PointOfContactID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<PointOfContact> ViewPointOfContact(int PointOfContactID);

    #endregion

    #region "Manage Review"
    [OperationContract]
    [FaultContract(typeof(SException))]
    void Review(User user, int eventID, int serviceID, int rating, DateTime reviewDate, string reviewDescription);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteReview(User user, string UserID, int ServiceID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Review> ViewReview(int ServiceID);

    #endregion
}

