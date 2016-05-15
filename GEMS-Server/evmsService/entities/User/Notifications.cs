using System;
using System.Linq;
using evmsService.entities;
using evmsService.DataAccess;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Collections.Generic;
using evmsService.Controllers;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Notifications")]
    public class Notifications
    {
        private string sender, receiver, title, msg;
        private bool read;
        private DateTime sendDateTime;

        public Notifications() { }

        public Notifications(string sender, string receiver, string title, string msg)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.title = title;
            this.msg = msg;
            this.read = false;
            sendDateTime = DateTime.Now;
        }

        [DataMember]
        [Column(Name="isRead")]
        public bool isRead
        {
            get { return read; }
            set { read = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "Sender")]
        public string Sender
        {
            get { return this.sender; }
            set { this.sender = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "Receiver")]
        public string Receiver
        {
            get { return this.receiver; }
            set { this.receiver = value; }
        }

        [DataMember]
        [Column(Name = "Title")]
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        [DataMember]
        [Column(Name = "Message")]
        public string Message
        {
            get { return this.msg; }
            set { this.msg = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "SendDateTime")]
        public DateTime SendDateTime
        {
            get { return this.sendDateTime; }
            set { this.sendDateTime = value; }
        }

    }
}