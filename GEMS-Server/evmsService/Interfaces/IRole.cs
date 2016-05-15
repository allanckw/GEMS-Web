using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IRole
{
    #region "RoleTemplate Management"
    [OperationContract]
    [FaultContract(typeof(SException))]
    int AddRoleTemplate(User user, Events evnt, string RolePost, string RoleDescription, List<EnumFunctions> functionID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteRoleTemplate(User user, int RoleID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditRightsTemplate(User user, int RoleID, string RolePost, string RoleDescription, List<EnumFunctions> functionID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<RoleTemplate> ViewTemplateRole(User user, Events evnt);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<RightTemplate> GetTemplateRight(int RoleTemplateID);
    #endregion

    #region "Role Management"

    [OperationContract]
    [FaultContract(typeof(SException))]
    int AddRole(User user, string RoleUserID, int eventID, string RolePost, string RoleDescription, List<EnumFunctions> functionID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteRole(User user, int RoleID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditRole(User user, string RoleUserID, int RoleID, string RolePost, string RoleDescription, List<EnumFunctions> FunctionList);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<EnumFunctions> GetRights(int eventID, string UserID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Role> ViewRole(User user, Events evnt);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<RoleWithUser> ViewEventRoles(User user, Events evnt);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Function> ViewFunction();

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Role> ViewUserEventRoles(string userID, int eventID);//here

    [OperationContract]
    [FaultContract(typeof(SException))]
    bool isEventFacilitator(string userid, int eventID);//here

    [OperationContract]
    [FaultContract(typeof(SException))]
    bool haveRightsTo(int eventID, string userID, EnumFunctions fx);
    #endregion
}

