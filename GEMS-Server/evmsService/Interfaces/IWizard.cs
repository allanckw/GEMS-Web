using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IWizard
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    void WizardAddEvent(User user, Events evnt, List<List<Program>> programs, List<List<Guest>> guests, List<ItemTypes> itemtypes, List<Items> items, Publish pub, List<Task> tasks);
}