using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface ITasks
{

    #region "Tasks Allocation"

    [OperationContract]
    [FaultContract(typeof(SException))]
    void CreateTask(User user, int eventID, string taskName,
            string taskDesc, DateTime DueDate);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteTask(User user, int eventID, int taskID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateTask(User user, int eventID, int taskID, string taskName, string taskDesc, DateTime dueDate);


    [OperationContract]
    [FaultContract(typeof(SException))]
    void SetTaskRead(Task t, int roleID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SetTaskCompleted(Task task, int roleID, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void SetTaskIncomplete(User user, Task task, int roleID, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Task> GetTasksByEvent(string userid, int eventID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Task> GetTaskByRole(int eventID, int roleID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    TaskAssignment GetTaskAssignment(int taskID, int eventID, int roleID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void AssignTask(User user, int eventID, int roleID, List<Task> taskList);

    [OperationContract]
    [FaultContract(typeof(SException))]
    Task GetTask(int taskID);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<Task> GetEventTasks(int eventID);
    #endregion

}




