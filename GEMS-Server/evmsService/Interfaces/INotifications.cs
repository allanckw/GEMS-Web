using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface INotifications
{

    [OperationContract]
    [FaultContract(typeof(SException))]
    string GetNewMessage(User user, string rid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    int GetUnreadMessageCount(User user, string rid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Notifications> GetUnreadMessages(User user, string rid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Notifications> GetAllMessages(User user, string rid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteNotifications(User user, Notifications msg);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteAllNotificationsOfUser(User user, string uid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SetNotificationRead(User user, Notifications msg);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SendNotification(string sender, string receiver, string title, string msg);
}

