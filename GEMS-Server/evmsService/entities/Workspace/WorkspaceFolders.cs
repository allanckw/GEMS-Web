using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    //To create a folder, user must be given rights by the event organizer
    [DataContract]
    [Table(Name = "ArtefactWSFolders")]
    public class WorkspaceFolders
    {
        string userID;
        int eventID;

        string folderName;
        string folderDesc;

        DateTime createdOn;

        string remarks;

        public WorkspaceFolders()
        {
        }

        public WorkspaceFolders(string uid, int eid, string folderName, string folderDesc, string remarks)
        {
            this.userID = uid;
            this.eventID = eid;
            this.folderName = folderName;
            this.folderDesc = folderDesc;
            this.remarks = remarks;
            this.createdOn = DateTime.Now;
        }

        [DataMember]
        [Column(Name = "Creator")]
        public string CreatedBy
        {
            get { return userID; }
            set { userID = value; }
        }

        [DataMember]
        [Column(Name = "EventID", IsPrimaryKey=true)]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "FolderName" , IsPrimaryKey=true)]
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        }

        [DataMember]
        [Column(Name = "FolderDesc")]
        public string FolderDescription
        {
            get { return folderDesc; }
            set { folderDesc = value; }
        }


        [DataMember]
        [Column(Name = "CreatedOn")]
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        [DataMember]
        [Column(Name = "Remarks")]
        public string Remarks
        {
            get { return this.remarks; }
            set { this.remarks = value; }
        }
    }
}


