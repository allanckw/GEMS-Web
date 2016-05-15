using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "EventTaskAssignments")]
    public class TaskAssignment
    {

        private int assignmentID; //PK

        private int taskID; //FK
        private int eventID; //FK

        private DateTime completedDateTime;
        private int assignedRoleID;

        private bool isRead;
        private bool isCompleted;

        private string remarks;

        public TaskAssignment()
        {
            this.completedDateTime = DateTime.Now;
            this.isRead = false;
            this.isCompleted = false;
            this.remarks = "";
        }

        public TaskAssignment(int eventID, int taskID, int roleID)
            : this()
        {
            this.eventID = eventID;
            this.taskID = taskID;
            this.assignedRoleID = roleID;
        }

        [DataMember]
        public string RoleName
        {
            get {
                Role r = RoleController.GetRole(AssignedRoleID);



                return r.Post;
            }
            set {  }
        }
        [DataMember]
        public string RoleUserID
        {
            get
            {
                Role r = RoleController.GetRole(AssignedRoleID);



                return r.UserID;
            }
            set { }
        }

        [DataMember]
        public string RoleUserName
        {
            get
            {
                Role r = RoleController.GetRole(AssignedRoleID);



                return UserController.GetUserName(r.UserID);
            }
            set { }
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "TaskAssignmentID")]
        public int TaskAssignmentID
        {
            get { return assignmentID; }
            set { assignmentID = value; }
        }

        [DataMember]
        [Column(Name = "TaskID")]
        public int TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "CompletedDateTime",CanBeNull=true)]
        public DateTime CompletedDateTime
        {
            get { return this.completedDateTime; }
            set { this.completedDateTime = value; }
        }

        [DataMember]
        [Column(Name = "AssignedRoleID")]
        public int AssignedRoleID
        {
            get { return assignedRoleID; }
            set { assignedRoleID = value; }
        }


        [DataMember]
        [Column(Name = "isRead")]
        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }

        [DataMember]
        [Column(Name = "isCompleted")]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        [DataMember]
        [Column(Name = "Remarks")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

   
    }
}