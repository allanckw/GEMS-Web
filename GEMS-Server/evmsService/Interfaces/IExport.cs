using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IExport
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    ExportData GetExportData(User u, int eventID, Boolean NeedFacilities, Boolean NeedPrograms, Boolean NeedIncome, Boolean NeedOptItems, Boolean NeedTasks
        , Boolean NeedGuest, Boolean NeedParticipant);
}

