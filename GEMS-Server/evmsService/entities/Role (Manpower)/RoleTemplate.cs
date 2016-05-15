using System;
using System.Linq;
using evmsService.entities;
using evmsService.DataAccess;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "RolesTemplate")]
    public class RoleTemplate
    {
        private int roleTemplateID;
        private string post;
        private string description;
        private Nullable<int> eventID;

        public RoleTemplate(string RolePost, string RoleDescription, Events evnt)
        {

            post = RolePost;
            description = RoleDescription;
            eventID = evnt == null ? (int?)null : evnt.EventID;
        }

        public RoleTemplate()
        {
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "RoleTemplateID")]
        public int RoleTemplateID
        {
            get { return roleTemplateID; }
            set { roleTemplateID = value; }
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
        [Column(Name = "EventID")]
        [DataMember]
        public Nullable<int> EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        //[Column(Name = "EventID")]
        //public object EventObj
        //{
        //    get {
        //        if(eventID ==0)
        //        return null;
        //        else
        //        return eventID; 
        //    }
        //    set { eventID = (int)value; }
        //}


    }
}