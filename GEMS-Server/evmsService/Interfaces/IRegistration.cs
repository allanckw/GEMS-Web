using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;


[ServiceContract]
interface IRegistration
{
    #region "Registration"
    [OperationContract]
    [FaultContract(typeof(SException))]
    void RegisterParticipant(int eventID, List<QuestionIDWithAnswer> answers);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteParticipant(User user, int eventID, int participantID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Participant> ViewEventParticipant(User user, int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<FieldAnswer> GetParticipantFieldAnswer(User user, int eventID, int ParticipantID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AddField(User user, int eventID, List<Field> ListField);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Field> ViewField(int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<StaticField> ViewStaticField();

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<ParticipantWithName> ViewEventParticipantWithName(User user, int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Participant EditParticipant(int ParticipantID, bool paid);
    #endregion

    #region "Publish"

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AddPublish(User user, int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, Boolean isPayable, decimal paymentAmount);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeletePublish(User user, int eventID);

 

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditPublish(User user, int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, bool isPayable, decimal amount);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Publish ViewPublish(int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewEventForPublish(DateTime start, DateTime end);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewEventForPublishByDateAndTag(DateTime start, DateTime end, String tag);


    
    [OperationContract]
    [FaultContract(typeof(SException))]
    List<ParticipantEvent> ParticipantViewEvents(string participantEmail, DateTime start, DateTime end, bool Paid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SetPaid(string Email, int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    bool isRegistered(string Email);

    [OperationContract]
    [FaultContract(typeof(SException))]
    bool isEventRegistered(string Email, int EventID);

    #endregion
}

