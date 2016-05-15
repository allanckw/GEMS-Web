using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Configuration;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class MobileController
    {

        private static string ConvertDate(DateTime dt)
        {
            string str = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
            return str;
        }
        private static string ConvertTime(DateTime dt)
        {
            string min = "";
            if (dt.Minute.ToString().Count() == 1)
            {
                min = dt.Minute.ToString() + "0";
            }
            else
                min = dt.Minute.ToString();

            string str = dt.Hour.ToString() + ":" + min;
            return str;
        }

        public static List<M_ShortEvent> GetEventsWithTag(DateTime fromDate, DateTime toDate, string tag)
        {
            List<Events> evnts = PublishController.ViewEventDateAndTag(fromDate, toDate, tag);

            List<M_ShortEvent> ls = new List<M_ShortEvent>();

            foreach (Events e in evnts)
            {
                M_ShortEvent ev = new M_ShortEvent();
                ev.Key = e.EventID;
                ev.StartDate = ConvertDate(e.StartDateTime);
                ev.Value = e.Name;

                ls.Add(ev);
            }
            return ls;
        }

        public static List<M_ShortEvent> GetEvents(DateTime fromDate, DateTime toDate)
        {
            List<Events> evnts = PublishController.ViewEvent(fromDate, toDate);

            List<M_ShortEvent> ls = new List<M_ShortEvent>();

            foreach (Events e in evnts)
            {
                M_ShortEvent ev = new M_ShortEvent();
                ev.Key = e.EventID;
                ev.StartDate = ConvertDate(e.StartDateTime);
                ev.Value = e.Name;

                ls.Add(ev);
            }
            return ls;
        }

        public static M_Event GetEvent(int eid)
        {
            Events e = EventController.GetEvent(eid);

            M_Event me = new M_Event();

            me.Name = e.Name;
            me.StartDate = ConvertDate(e.StartDateTime);
            me.StartTime = ConvertTime(e.StartDateTime);

            me.EndDate = ConvertDate(e.EndDateTime);
            me.EndTime = ConvertTime(e.EndDateTime);

            me.Description = e.Description;
            me.Website = e.Website;

            Publish p = PublishController.GetPublish(eid);
            if (p != null)
                me.PublicationRemarks = p.Remarks;
            else
                me.PublicationRemarks = "";

            //For Registration Link la -.- 
            me.Registration = WebConfigurationManager.AppSettings["regLink"] + eid.ToString();


            return me;
        }

        public static List<M_ShortProgram> GetEventProgramme(int eid)
        {
            Events e = EventController.GetEvent(eid);

            List<M_ShortProgram> ls = new List<M_ShortProgram>();


            foreach (int dayID in e.DaysID)
            {
                List<Program> programs = ProgramController.ViewProgram(dayID);
                foreach (Program p in programs)
                {
                    M_ShortProgram sp = new M_ShortProgram();
                    sp.Key = p.ProgramID;
                    sp.Value = p.Name;
                    sp.StartDtae = ConvertDate(p.StartDateTime);
                    ls.Add(sp);
                }
            }

            return ls;
        }

        public static M_Program GetProgram(int pid)
        {
            M_Program mp = new M_Program();
            Program p = ProgramController.GetPrograms(pid);

            mp.Name = p.Name;
            mp.StartDate = ConvertDate(p.StartDateTime);
            mp.StartTime = ConvertTime(p.StartDateTime);
            mp.EndDate = ConvertDate(p.EndDateTime);
            mp.EndTime = ConvertTime(p.EndDateTime);

            return mp;
        }



        public static List<M_ShortGuest> GetEventGuests(int eid)
        {
            Events e = EventController.GetEvent(eid);

            List<M_ShortGuest> ls = new List<M_ShortGuest>();


            foreach (int dayID in e.DaysID)
            {
                List<Guest> guests = GuestController.ViewGuest(dayID);
                foreach (Guest g in guests)
                {
                    M_ShortGuest sg = new M_ShortGuest();
                    sg.Key = g.GuestId;
                    sg.Value = g.Name;
                    sg.Date = ConvertDate(DayController.GetDay(dayID).DayDate);
                    ls.Add(sg);

                }
            }

            return ls;
        }

        public static M_Guest GetGuest(int gid)
        {
            M_Guest mg = new M_Guest();

            Guest g = GuestController.GetGuest(gid);

            mg.Name = g.Name;
            mg.Description = g.Description;

            return mg;

        }

        public static string Authenticate(string userid, string pwd)
        {
            User user = null;
            DAL dalDataContext = new DAL();
            user = (from users in dalDataContext.users
                    where users.UserID == userid
                    select users).FirstOrDefault<User>();
            if (user == null)
            {
                throw new FaultException<SException>(new SException(),
                    new FaultReason("Invalid User, Please try again"));
            }
            else if (user.User_Password.CompareTo(MobileEncryptor.decrypt(pwd)) != 0)
            {
                throw new FaultException<SException>(new SException(),
               new FaultReason("Wrong Password!, please try again"));
            }
            else
            {
                //user.GetSystemRole();
                return MobileEncryptor.encrypt(userid + "@" + DateTime.Now.Date.Ticks.ToString());
            }

        }

        public static User isValidKey(string key)
        {
            key = key.Replace(' ', '+');
            string decrytedKey = MobileEncryptor.decrypt(key);
            string userID = decrytedKey.Split('@')[0];
            long ticks = long.Parse(decrytedKey.Split('@')[1]);
            DateTime d = new DateTime(ticks);

            try
            {
                if (d.Date != DateTime.Now.Date)
                {
                    throw new FaultException<SException>(new SException(),
                      new FaultReason("Your passkey has expired, please log in again!"));
                }
                else
                {
                    return UserController.GetUser(userID);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                      new FaultReason("An error has occured when trying to validate key with error " + ex.Message));
            }
        }

        public static List<M_ShortTask> ViewUserTask(string userid, int eventID)
        {
            List<Role> userRoles = RoleController.ViewUserEventRoles(userid, eventID);
            List<M_ShortTask> myTask = new List<M_ShortTask>();
            foreach (Role r in userRoles)
            {
                List<Task> tasks = TaskController.ViewTasksByRole(eventID, r.RoleID);

                foreach (Task t in tasks)
                {
                    M_ShortTask st = new M_ShortTask();

                    st.Name = t.TaskName;
                    st.Date = ConvertDate(t.DueDate);
                    st.Key = t.TaskID;

                    myTask.Add(st);
                }

            }

            return myTask;
        }

        public static M_Task GetTask(string userID, int taskID)
        {
            Task t = TaskController.GetTask(taskID);

            bool canview = CanViewTask(userID, t.EventID);//got rights as in function rights

            if (canview == false)//this assignement is for me
            {
                foreach (TaskAssignment ta in t.TasksAssignments)
                {
                    if (ta.RoleUserID == userID)
                    {
                        canview = true;
                        break;
                    }
                }
            }

            if (canview == false)
            {
                throw new FaultException<SException>(new SException(),
                      new FaultReason("Error, no rights to view task"));
            }
            else
            {
                M_Task mt = new M_Task();

                mt.Name = t.TaskName;
                mt.PercentageCompleted = t.PercentageCompletion;
                mt.Description = t.TaskDesc;
                mt.DueDate = ConvertDate(t.DueDate);
                mt.DueTime = ConvertTime(t.DueDate);

                List<TaskAssignment> lta = TaskAssignmentController.GetAllAssignments(t.TaskID, t.EventID);
                Events evnt = EventController.GetEvent(t.EventID);
                // 
                //List<EnumFunctions> fx = RoleLogicController.GetRights(t.EventID, userID);




                foreach (TaskAssignment ta in lta)
                {
                    //but can the guy view the assignment

                    //if (ta.RoleUserID == userID || evnt.Organizerid == userID || fx.Contains(EnumFunctions.Assign_Task) || fx.Contains(EnumFunctions.Update_Task))
                    //{
                    M_TaskAssignment mta = new M_TaskAssignment();
                    //mt.Assignment;
                    mta.IsCompleted = ta.IsCompleted;
                    mta.IsRead = ta.IsRead;
                    mta.RoleName = ta.RoleName;
                    mta.RoleUserID = ta.RoleUserID;
                    mta.RoleUserName = ta.RoleUserName;
                    //}
                }
                return mt;
            }
        }

       
        public static List<M_ShortTask> ViewEventTask(string userid, int eventID)
        {

            if (!CanViewTask(userid, eventID))
                throw new FaultException<SException>(new SException(),
                      new FaultReason("Error, no rights to view task"));

            List<M_ShortTask> myTask = new List<M_ShortTask>();
            List<Task> tasks = TaskController.GetTasksByEvent(userid, eventID);

            foreach (Task t in tasks)
            {
                M_ShortTask st = new M_ShortTask();

                st.Name = t.TaskName;
                st.Date = ConvertDate(t.DueDate);
                st.Key = t.TaskID;

                myTask.Add(st);
            }



            return myTask;
        }

        public static bool CanViewTask(string userID, int eventID)
        {
            List<EnumFunctions> fx = RoleLogicController.GetRights(eventID, userID);
            Events evnt = EventController.GetEvent(eventID);
            if (evnt.Organizerid == userID)
                return true;
            return (
                    fx.Contains(EnumFunctions.Assign_Task) || fx.Contains(EnumFunctions.Add_Task) ||
                    fx.Contains(EnumFunctions.Update_Task) || fx.Contains(EnumFunctions.Delete_Task)
                    );
        }


    }
}