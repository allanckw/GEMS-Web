using System;
using System.Linq;
using evmsService.entities;
using evmsService.DataAccess;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Roles")]
    public class Role
    {
        private int roleID;
        private string post;
        private string description;
        private int eventID;
        private string userID;


        public Role(string RolePost, string RoleDescription, int EventID, string UserID)
        {

            post = RolePost;
            description = RoleDescription;
            eventID = EventID;
            userID = UserID;

        }
        public Role()
        {
        }




        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "RoleID")]
        public int RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }

        [DataMember]
        [Column(Name = "Post")]
        public string Post
        {
            get { return post; }
            set { post = value; }
        }

        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "UserID")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [DataMember]
        public List<Task> AssignedTasks
        {
            get
            {
                return TaskController.GetTasksByEvent(this.eventID);
            }
            private set { }
        }
    }
}