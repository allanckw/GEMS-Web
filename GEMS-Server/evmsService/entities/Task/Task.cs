using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "EventTasks")]
    public class Task
    {
        private int taskID;
        private string taskName;
        private string taskDesc;

        private DateTime dueDate;

        private int eventID;//FK

        //Default Constructor
        public Task()
        {
        }

        public Task(int eventID, string name, string desc, DateTime dueDate)
        {
            this.eventID = eventID;
            this.taskName = name;
            this.taskDesc = desc;
            this.dueDate = dueDate;
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "TaskID")]
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
        [Column(Name = "TaskName")]
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }

        [DataMember]
        [Column(Name = "TaskDesc")]
        public string TaskDesc
        {
            get { return taskDesc; }
            set { taskDesc = value; }
        }


        [DataMember]
        [Column(Name = "DueDate")]
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        private double countPercentageCompletion()
        {
            List<TaskAssignment> assignments = TaskAssignmentController.
                GetAllAssignments(this.taskID, this.eventID);

            int assignCount = assignments.Count;
            if (assignCount > 0)
            {
                double percentPerTask = 100 / assignCount;
                double completion = 0;
                foreach (TaskAssignment tAssn in assignments)
                {
                    if (tAssn.IsCompleted)
                        completion += percentPerTask;
                }
                return completion;
            }
            else
            {
                return 0;
            }
        }
        [DataMember]
        public double PercentageCompletion
        {
            private set { }
            get
            {
                return this.countPercentageCompletion();
            }
        }
        [DataMember]
        public List<TaskAssignment> TasksAssignments
        {
            get { return TaskAssignmentController.GetAllAssignments(this.taskID, this.eventID); }
            private set { }
        }

    }
}