using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Transactions;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class TaskAssignmentController
    {

        public static void AddSingleAssignment(User user, int eventID, int roleID, Task task)
        {
            //TODO: Put in after roles management for task up
            if (!user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Assign_Task)
                || !user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Add_Task))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Assign Tasks!"));

            DAL dalDataContext = new DAL();
            TaskAssignment taskAssn = new TaskAssignment(eventID, task.TaskID, roleID);
            Table<TaskAssignment> assignmentTable = dalDataContext.taskAssignments;

            assignmentTable.InsertOnSubmit(taskAssn);
            assignmentTable.Context.SubmitChanges();

        }

        public static void AssignTasks(User user, int eventID, int roleID, List<Task> taskList)
        {
            //TODO: Put in after roles management for task up
            if (!user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Assign_Task)
                || !user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Add_Task))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Assign Tasks!"));

            if (taskList.Count == 0)
            {
                RemoveAllTasksFromRole(eventID, roleID);
                string msg = "Your task(s) assigned by " + user.Name + " for the Event: "
                + EventController.GetEvent(eventID).Name + " has been removed";

                NotificationController.sendNotification(user.UserID, RoleController.GetRole(roleID).UserID,
                                          "Task(s) Allocated Removed", msg);
                return;
            }
            else
            {
                DAL dalDataContext = new DAL();
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        List<TaskAssignment> taskAssignmentList = new List<TaskAssignment>();

                        RemoveAllTasksFromRole(eventID, roleID);

                        foreach (Task t in taskList)
                        {
                            if (!IsAssignmentCompleted(t.EventID, roleID, t.TaskID)){
                                 TaskAssignment tAssn = new TaskAssignment(t.EventID, t.TaskID, roleID);
                                taskAssignmentList.Add(tAssn);
                            }
                        }

                        RemoveAllTasksFromRole(eventID, roleID);

                        Table<TaskAssignment> taskAssns = dalDataContext.taskAssignments;
                        taskAssns.InsertAllOnSubmit(taskAssignmentList);
                        taskAssns.Context.SubmitChanges();

                        string msg = "You were allocated some tasks by " + user.Name + " for the Event: "
                            + EventController.GetEvent(eventID).Name;

                        NotificationController.sendNotification(user.UserID, RoleController.GetRole(roleID).UserID,
                                                  "New Task Allocated", msg);
                        tScope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw new FaultException<SException>(new SException(ex.Message),
                          new FaultReason("An Error occured While Adding Assigning Tasks: " + ex.Message));
                    }
                }
            }
        }

        private static void RemoveAllTasksFromRole(int eventID, int roleID)
        {
            DAL dalDataContext = new DAL();

            List<TaskAssignment> taskAssigned = (from tAssn in dalDataContext.taskAssignments
                                                 where tAssn.AssignedRoleID == roleID &&
                                                 tAssn.EventID == eventID
                                                 && tAssn.IsCompleted == false
                                                 select tAssn).ToList<TaskAssignment>();

            dalDataContext.taskAssignments.DeleteAllOnSubmit(taskAssigned);
            dalDataContext.SubmitChanges();

        }

        private static bool IsAssignmentCompleted(int eventID, int roleID, int taskID)
        {
            DAL dalDataContext = new DAL();

            TaskAssignment completedTask = (from tAssn in dalDataContext.taskAssignments
                                            where tAssn.AssignedRoleID == roleID &&
                                            tAssn.EventID == eventID && tAssn.TaskID == taskID
                                            && tAssn.IsCompleted == true
                                            select tAssn).SingleOrDefault<TaskAssignment>();

            return completedTask != null;
        }

        public static List<TaskAssignment> GetUserTaskAssignment(User user, int eventID)
        {
            try
            {
                List<Role> userRoles = RoleController.ViewUserRolesForEvent
                    (user, EventController.GetEvent(eventID));

                DAL dalDataContext = new DAL();
                List<TaskAssignment> allAssignments = new List<TaskAssignment>();

                foreach (Role r in userRoles)
                {
                    List<TaskAssignment> taskAssigned = (from taskAssn in dalDataContext.taskAssignments
                                                         where taskAssn.EventID == eventID
                                                         && taskAssn.AssignedRoleID == r.RoleID
                                                         select taskAssn).ToList<TaskAssignment>();

                    allAssignments.AddRange(taskAssigned);
                }
                return allAssignments;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Task Data, Please Try Again!"));
            }
        }

        public static List<TaskAssignment> GetAllAssignments(int taskID, int eventID)
        {
            DAL dalDataContext = new DAL();
            try
            {
                List<TaskAssignment> taskAssigned = (from tAssn in dalDataContext.taskAssignments
                                                     where tAssn.TaskID == taskID &&
                                                     tAssn.EventID == eventID
                                                     select tAssn).ToList<TaskAssignment>();
                return taskAssigned;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

        //Set Task to read
        public static void SetToRead(Task readTask, int roleID)
        {
            DAL dalDataContext = new DAL();
            try
            {
                TaskAssignment taskAssigned = (from tAssn in dalDataContext.taskAssignments
                                               where tAssn.TaskID == readTask.TaskID &&
                                                     tAssn.EventID == readTask.EventID &&
                                                     tAssn.AssignedRoleID == roleID
                                               select tAssn).SingleOrDefault();
                taskAssigned.IsRead = true;

                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

        //Set task to completed
        public static void SetCompleted(Task completedTask, int roleID, string remarks)
        {

            DAL dalDataContext = new DAL();
            try
            {
                TaskAssignment taskAssigned = (from tAssn in dalDataContext.taskAssignments
                                               where tAssn.TaskID == completedTask.TaskID &&
                                               tAssn.EventID == completedTask.EventID &&
                                               tAssn.AssignedRoleID == roleID
                                               select tAssn).SingleOrDefault();

                taskAssigned.IsCompleted = true;
                taskAssigned.CompletedDateTime = DateTime.Now;
                taskAssigned.Remarks = remarks;
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

        //Set task to not completed however need rights not anyone can do this
        //to prevent sabo
        public static void SetInComplete(User authorizedPerson, Task completedTask, int roleID, string remarks)
        {
            if (!authorizedPerson.isAuthorized(EventController.GetEvent(completedTask.EventID), EnumFunctions.Update_Task))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Update Tasks!"));
            DAL dalDataContext = new DAL();
            try
            {
                TaskAssignment taskAssigned = (from tAssn in dalDataContext.taskAssignments
                                               where tAssn.TaskID == completedTask.TaskID &&
                                               tAssn.EventID == completedTask.EventID &&
                                               tAssn.AssignedRoleID == roleID
                                               select tAssn).SingleOrDefault();

                taskAssigned.IsCompleted = false;
                taskAssigned.Remarks = remarks;
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

        public static TaskAssignment GetTaskAssignment(int taskID, int eventID, int roleID)
        {
            DAL dalDataContext = new DAL();
            try
            {
                TaskAssignment matchedAssignment = (from tAssn in dalDataContext.taskAssignments
                                                    where tAssn.TaskID == taskID &&
                                                    tAssn.EventID == eventID &&
                                                    tAssn.AssignedRoleID == roleID
                                                    select tAssn).SingleOrDefault();
                return matchedAssignment;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

    }
}