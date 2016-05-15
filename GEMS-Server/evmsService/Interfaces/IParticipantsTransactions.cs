using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IParticipantsTransactions
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    void SaveTransaction(string transID, List<ParticipantTransaction> trans);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<ParticipantTransaction> ViewParticipantsTransactions(string email, DateTime fromDate, DateTime toDate);
}

