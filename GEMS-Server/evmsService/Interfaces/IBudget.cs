using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IBudget
{
    [OperationContract]
    [FaultContract(typeof(SException))]
    void SaveBudgetList(User sender, int eventID, int totalSat, decimal totalPrice,
            List<Items> itemList);

    [OperationContract]
    [FaultContract(typeof(SException))]
    OptimizedBudgetItems GetBudgetItem(int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Items GetItemDetail(OptimizedBudgetItemsDetails bItem);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateActualPrice(User user, Items iten, decimal price);

    [OperationContract]
    [FaultContract(typeof(SException))]
    int GetGST();

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateGST(User user, int newPercentage);


    [OperationContract]
    [FaultContract(typeof(SException))]
    int AddIncome(User user, int eventID, BudgetIncome bIncome);


    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteBudgetIncome(User user, int BudgetIncomeID, int eventID);


    [OperationContract]
    [FaultContract(typeof(SException))]
    void EditBudgetIncome(User user, int eventID, int incomeID, BudgetIncome bIncome);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<BudgetIncome> ViewBudgetIncome(User user, int eventID);
}

