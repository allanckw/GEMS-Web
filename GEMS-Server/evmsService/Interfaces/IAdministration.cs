using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IAdministration
{
    // ADSS Sub System - Kok Wei & Ji Kai
    #region "Administrative sub system"
    //[OperationContract]
    //[FaultContract(typeof(SException))]
    //User Authenticate(string userid, string password);

    [OperationContract]
    [FaultContract(typeof(SException))]
    User SecureAuthenticate(Credentials credentials);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AssignLocationAdmin(User assigner, string userid, Faculties fac);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AssignEventOrganizer(User assigner, string userid, string description);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AssignSystemAdmin(User assigner, string userid, string description);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<User> SearchUser(string name, string userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<User> SearchUserByRole(string name, string userid, EnumRoles r);

    [OperationContract]
    [FaultContract(typeof(SException))]
    EnumRoles ViewUserRole(string userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UnAssignRole(User assigner, string userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    string GetUserName(string userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    string GetUserEmail(string userid);

    [OperationContract]
    [FaultContract(typeof(SException))]
    int GetClientTimeOut();

    #endregion
}

