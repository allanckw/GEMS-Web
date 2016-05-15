using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IFacility
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    void RemoveFacility(User user, string venue, Faculties fac);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateFacility(User user, string venue, Faculties faculty, string loc,
            string bookingCon, string techCon, int cap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Faculties GetFacilityAdmin(string userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    string GetFacilityAdminFaculty(Faculties userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<User> GetFacilityAdmins();

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Facility> SearchFacilities(Faculties fac, int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Faculties> SearchFacilitiesFac(int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer);

    //[OperationContract]
    //[FaultContract(typeof(SException))]
    //List<Facility> GetVenues(int minCap = 0, int maxCap = int.MaxValue);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Facility> GetVenuesByFaculty(Faculties fac);
}

