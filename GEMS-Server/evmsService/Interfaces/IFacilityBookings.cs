using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IFacilityBookings
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    bool AddFacilityBookingRequest(User user, EventDay evntDay, Faculties faculty, DateTime reqstart,
        DateTime reqEnd, List<FacilityBookingRequestDetails> reqDetails);

    [OperationContract]
    [FaultContract(typeof(SException))]
    bool CheckRequestExist(int eventid, DateTime start, DateTime end);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<FacilityBookingRequest> GetFacBookingRequestList(User FacAdmin);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void ApproveFacilityBooking(User approver, int reqID, int evID, int reqDetailID,
            string remarks, string purpose);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void RejectFacilityBooking(User rejecter, int reqID, int evID, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DropConfirmedRequest(User user, int reqID, int evID, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void CancelFacilityBooking(User userser, int reqID, int evID, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<FacilityBookingRequest> ViewFacilityBookingRequestsByEventDay(User user, int evID,
           BookingStatus status, bool viewAllStatus, bool ViewAllEventDays, DateTime day);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<FacilityBookingConfirmed> GetActivitiesForDay(User user, DateTime day,
        Faculties faculty, string venue);

    [OperationContract]
    [FaultContract(typeof(SException))]
    FacilityBookingConfirmed GetConfirmedBooking(int reqID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Events> ViewAuthorizedEventsForFacBookings(User sender);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List< FacilityBookingConfirmed> GetConfirmedFacBookings(int eventID, DateTime dayDate);

    [OperationContract]
    [FaultContract(typeof(SException))]
    int GetConfirmedFacBookingsCount(int eventID, DateTime dayDate);
}
