 using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IRequest
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    string CreateNewRequest(User user, int eventid, string targetEmail, string description, string url, string title);
 
    [OperationContract]
    [FaultContract(typeof(SException))]
    Request GetRequest(int requestID);
  
    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditRequest(User user, int requestID, string description, string url);


    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Request> ViewRequests(Requestee requestee, DateTime fromDate, DateTime toDate, RequestStatus status, bool viewAllStatus);
  
    [OperationContract]
    [FaultContract(typeof(SException))]
    void ChangeStatus(Requestee requestee, int requestID, RequestStatus status, string remark);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Requestee ValidateRequestee(string targetEmail, string OTP);
     
    [OperationContract]
    [FaultContract(typeof(SException))]
    string GetOtp(string targetEmail);


    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Request> ViewRequestViaRequester(int eventID, User user, DateTime fromDate, DateTime toDate,
                            RequestStatus status, bool viewAllStatus);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void CancelRequest(User user, int requestID);

}

