using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class TaskController
    {

        public static void CreateTask(int eventID, string taskName,
            string taskDesc, DateTime DueDate, DAL dalDataContext)
        {
           
            try
            {
                
                Table<Task> taskTable = dalDataContext.tasks;
                Task task = new Task(eventID, taskName, taskDesc, DueDate);

                taskTable.InsertOnSubmit(task);
                taskTable.Context.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Task, Please Try Again!"));
            }
        }


        //Create a new task without assigning to anyone
        public static void CreateTask(User user, int eventID, string taskName,
            string taskDesc, DateTime DueDate)
        {
            //TODO: Put in after roles management for task up
            if (!user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Add_Task))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Tasks!"));
            try
            {
                DAL dalDataContext = new DAL();
                Table<Task> taskTable = dalDataContext.tasks;
                Task task = new Task(eventID, taskName, taskDesc, DueDate);

                taskTable.InsertOnSubmit(task);
                taskTable.Context.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Task, Please Try Again!"));
            }
        }

        //Delete the task
        public static void DeleteTask(User user, int TaskID, int eventID)
        {
            Task taskToDelete = GetTask(TaskID);
            //TODO: Put in after roles management for task up
            if (!user.isAuthorized(EventController.GetEvent(taskToDelete.EventID), EnumFunctions.Delete_Task))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Tasks!"));

            DAL dalDataContext = new DAL();

            try
            {
                Task matchedTask = (from tasks in dalDataContext.tasks
                                    where tasks.TaskID == taskToDelete.TaskID &&
                                    tasks.EventID == taskToDelete.EventID
                                    select tasks).FirstOrDefault();


                dalDataContext.tasks.DeleteOnSubmit(matchedTask);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Guest, Please Try Again!"));
            }

        }

        //Update The Task
        public static void UpdateTask(User user, int taskID, int eventID, string taskName,
            string taskDesc, DateTime dueDate)
        {
            Task taskToUpdate = GetTask(taskID);
            //TODO: Put in after roles management for task up
            if (!user.isAuthorized(EventController.GetEvent(taskToUpdate.EventID), EnumFunctions.Update_Task))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Update Tasks!"));

            DAL dalDataContext = new DAL();
            try
            {
                Task t = (from task in dalDataContext.tasks
                          where task.TaskID == taskToUpdate.TaskID &&
                          task.EventID == taskToUpdate.EventID
                          select task).SingleOrDefault();
                t.TaskName = taskName;
                t.TaskDesc = taskDesc;
                t.DueDate = dueDate;
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

           public static Task GetTask(int taskID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Task existingTasks = (from task in dalDataContext.tasks
                                      where task.TaskID == taskID
                                      select task).FirstOrDefault();

                if (existingTasks == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Task"));
                }
                return existingTasks;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Task Data, Please Try Again!"));
            }
        }

        public static List<Task> GetTasksByEvent(string userID, int eventID)
        {
            User user = UserController.GetUser(userID);
            Events evnt = EventController.GetEvent(eventID);
            if (!(user.isAuthorized(evnt, EnumFunctions.Add_Task) ||
               user.isAuthorized(evnt, EnumFunctions.Assign_Task) ||
               user.isAuthorized(evnt, EnumFunctions.Delete_Task) ||
               user.isAuthorized(evnt, EnumFunctions.Update_Task)))
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("Invalid User, User Does Not Have Rights To View Tasks!"));
            }
            try
            {
                DAL dalDataContext = new DAL();

                List<Task> existingTasks = (from task in dalDataContext.tasks
                                            where task.EventID == eventID
                                            select task).ToList<Task>();
                return existingTasks;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Task Data, Please Try Again!"));
            }
        }

        public static List<Task> GetEventTasks(int eventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Task> existingTasks = (from task in dalDataContext.tasks
                                            where task.EventID == eventID
                                            select task).ToList<Task>();
                return existingTasks;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Task Data, Please Try Again!"));
            }
        }

        public static List<Task> GetTasksByEvent(int eventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Task> existingTasks = (from task in dalDataContext.tasks
                                            where task.EventID == eventID
                                            select task).ToList<Task>();
                return existingTasks;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Task Data, Please Try Again!"));
            }
        }

        public static List<Task> ViewTasksByRole(int eventID, int roleID)
        {
            DAL dalDataContext = new DAL();
            List<Task> taskList = new List<Task>();
            List<TaskAssignment> taskAssigned = (from taskAssn in dalDataContext.taskAssignments
                                                 where taskAssn.AssignedRoleID == roleID
                                                 select taskAssn).ToList<TaskAssignment>();

            foreach (TaskAssignment ta in taskAssigned)
            {
                taskList.Add(TaskController.GetTask(ta.TaskID));
            }

            return taskList;

        }

        public static List<Task> GetUserAssignedTasks(User user, int eventID)
        {
            try
            {
                List<Role> userRoles = RoleController.ViewUserRolesForEvent
                    (user, EventController.GetEvent(eventID));

                DAL dalDataContext = new DAL();
                List<Task> taskList = new List<Task>();

                foreach (Role r in userRoles)
                {
                    List<TaskAssignment> taskAssigned = (from taskAssn in dalDataContext.taskAssignments
                                                         where taskAssn.EventID == eventID
                                                         && taskAssn.AssignedRoleID == r.RoleID
                                                         select taskAssn).ToList<TaskAssignment>();

                    foreach (TaskAssignment ta in taskAssigned)
                    {
                        taskList.Add(TaskController.GetTask(ta.TaskID));
                    }
                }
                return taskList.Distinct<Task>().ToList<Task>();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                       new FaultReason("An Error occured While Retrieving Task Data, Please Try Again!"));
            }
        }
    }
}