using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    //Anyone as long as in the event (i.e. any facilitators)
    //can upload and download files
    [DataContract]
    [Table(Name = "ArtefactWSFiles")]
    public class WorkspaceFiles
    {
        string userID;
        int eventID;

        string folderName;
        string fileName;
        string fileDesc;
        string fileURL;

        DateTime uploadedOn;

        public WorkspaceFiles()
        {
        }

        public WorkspaceFiles(string uid, int eid, string folderName, string fileName, string fileDesc, string fileURL)
        {
            this.userID = uid;
            this.eventID = eid;
            this.folderName = folderName;
            this.fileName = fileName;
            this.fileDesc = fileDesc;
            this.uploadedOn = DateTime.Now;
            this.fileURL = fileURL;
        }

        [DataMember]
        [Column(Name = "Uploader")]
        public string UploadedBy
        {
            get { return userID; }
            set { userID = value; }
        }


        [DataMember]
        [Column(Name = "FileURL")]
        public string FileURL
        {
            get { return fileURL; }
            set { fileURL = value; }
        }


        [DataMember]
        [Column(Name = "EventID", IsPrimaryKey = true)]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "FolderName", IsPrimaryKey = true)]
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        }

        [DataMember]
        [Column(Name = "FileName", IsPrimaryKey = true)]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        [DataMember]
        [Column(Name = "FileDesc")]
        public string FileDescription
        {
            get { return fileDesc; }
            set { fileDesc = value; }
        }


        [DataMember]
        [Column(Name = "UploadedOn")]
        public DateTime UploadedOn
        {
            get { return uploadedOn; }
            set { uploadedOn = value; }
        }

    }
}

