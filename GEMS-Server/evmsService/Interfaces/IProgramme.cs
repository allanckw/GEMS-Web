using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IProgramme
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    int AddProgram(User user, string ProgramName, DateTime ProgramStartDateTime, DateTime ProgramEndDatetime,
        string ProgramDescription, int ProgramDayID, string ProgramLocaton);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Program> ViewProgram(int dayID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditProgram(User user, int ProgramID, string ProgramName, DateTime ProgramStartDateTime
        , DateTime ProgramEndDatetime, string ProgramDescription, string ProgramLocaton);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteProgram(User user, int ProgramID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    bool ValidateProgramTime(int dayID, DateTime segmentStart, DateTime segmentEnd);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<int> GetEventProgCount(int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SwapProgram(User user, int ProgramID1, int ProgramID2);
}

