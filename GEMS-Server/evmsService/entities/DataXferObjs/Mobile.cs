using System.Runtime.Serialization;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    public class M_ShortTask
    {
        private int key;
        private string name;
        private string date;

        [DataMember]
        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }


    }

    [DataContract]
    public class M_Task
    {
        private string dueDate;
        private string dueTime;
        private string name;
        private string description;
        private double percentageCompleted;
        private List<M_TaskAssignment> assignment;

       
        public M_Task()
        {
            assignment = new List<M_TaskAssignment>();
        }

        [DataMember]
        public double PercentageCompleted
        {
            get { return percentageCompleted; }
            set { percentageCompleted = value; }
        }
        [DataMember]
        public string DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        [DataMember]
        public string DueTime
        {
            get { return dueTime; }
            set { dueTime = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }



        [DataMember]
        public List<M_TaskAssignment> Assignment
        {
            get { return assignment; }
            set { assignment = value; }
        }
    }


    [DataContract]
    public class M_TaskAssignment
    {
        private string roleName;
        private string roleUserID;
        private string roleUserName;
        private bool isRead;
        private bool isCompleted;

        [DataMember]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        [DataMember]
        public string RoleUserID
        {
            get { return roleUserID; }
            set { roleUserID = value; }
        }

        [DataMember]
        public string RoleUserName
        {
            get { return roleUserName; }
            set { roleUserName = value; }
        }
        [DataMember]
        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }
        [DataMember]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

    }

    [DataContract]
    public class M_ShortEvent
    {
        private int key;
        private string value;
        private string startDate;

        [DataMember]
        public int Key
        {
            get { return key; }
            set { key = value; }
        }
        [DataMember]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        [DataMember]
        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
    }

    [DataContract]
    public class M_Event
    {
        private string name;
        private string startDate;
        private string startTime;
        private string endDate;
        private string endTime;
        private string description;
        private string website;
        private string publicationRemarks;
        private string registration;


        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        [DataMember]
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        [DataMember]
        public string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        [DataMember]
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        [DataMember]
        public string Website
        {
            get { return website; }
            set { website = value; }
        }
        [DataMember]
        public string PublicationRemarks
        {
            get { return publicationRemarks; }
            set { publicationRemarks = value; }
        }
        [DataMember]
        public string Registration
        {
            get { return registration; }
            set { registration = value; }
        }

    }

    [DataContract]
    public class M_ShortProgram
    {
        private string startDtae;
        private int key;
        private string value;

        [DataMember]
        public string StartDtae
        {
            get { return startDtae; }
            set { startDtae = value; }
        }
        [DataMember]
        public int Key
        {
            get { return key; }
            set { key = value; }
        }
        [DataMember]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }


    }


    [DataContract]
    public class M_Program
    {
        private string name;
        private string startDate;
        private string startTime;
        private string endDate;
        private string endTime;
        private string description;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        [DataMember]
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        [DataMember]
        public string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        [DataMember]
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }

    [DataContract]
    public class M_ShortGuest
    {
        private string date;
        private int key;
        private string value;

        [DataMember]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        [DataMember]
        public int Key
        {
            get { return key; }
            set { key = value; }
        }
        [DataMember]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }

    [DataContract]
    public class M_Guest
    {
        private string name;
        private string description;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}