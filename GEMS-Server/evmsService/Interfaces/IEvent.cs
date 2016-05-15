using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IEvent
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    void CreateEvent(User user, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewUserAssociatedEvent(User user);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewOrganizerEvent(User user);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditEvent(User user, Events evnt, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag);


    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteEvent(User user, Events evnt);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewEventsByDateAndTag(User user, DateTime start, DateTime end, string tag);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewEventsByTag(User user, string tag);

    [OperationContract]
    [FaultContract(typeof(SException))]
    string GetEventName(int eventid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewAllEvents(User user);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Events GetEvent(int eventid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    EventDay GetDay(int DayID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<EventDay> GetDays(int EventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Boolean isEventExist(int eventID);
}

