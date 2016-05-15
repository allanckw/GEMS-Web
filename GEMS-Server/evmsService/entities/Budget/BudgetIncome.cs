using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;


namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "BudgetIncome")]
    public class BudgetIncome
    {
        private int incomeID;
        private int eventID;
        private string name;
        private string description;
        private bool gstLiable;
        private decimal incomeB4GST;
        private decimal gstValue;
        private string source;

        private DateTime dateReceived;

        public BudgetIncome() { }

        public BudgetIncome(int evID, string name, string desc, bool gstLiable, decimal incomeb4gst,
            decimal gstValue, string source, DateTime dateReceived)
        {
            this.eventID = evID;
            this.name = name;
            this.description = desc;
            this.gstLiable = gstLiable;
            this.incomeB4GST = incomeb4gst;
            this.gstValue = gstValue;
            this.source = source;
            this.dateReceived = dateReceived;
        }

        public BudgetIncome(int evID, string name, string desc, bool gstLiable, decimal incomeb4gst,
                            decimal gstValue, string source, DateTime dateReceived, int incomeID)
        {
            this.eventID = evID;
            this.name = name;
            this.description = desc;
            this.gstLiable = gstLiable;
            this.incomeB4GST = incomeb4gst;
            this.gstValue = gstValue;
            this.source = source;
            this.dateReceived = dateReceived;
            this.incomeID = incomeID;
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "IncomeID")]
        public int IncomeID
        {
            get { return incomeID; }
            set { incomeID = value; }
        }

        [DataMember]
        [Column(Name = "ReceivedDate")]
        public DateTime DateReceived
        {
            get { return dateReceived; }
            set { dateReceived = value; }
        }

        [DataMember]
        [Column(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        [Column(Name = "isGSTLiable")]
        public bool IsGstLiable
        {
            get { return gstLiable; }
            set { gstLiable = value; }
        }

        [DataMember]
        [Column(Name = "IncomeB4GST")]
        public decimal IncomeBeforeGST
        {
            get { return incomeB4GST; }
            set { incomeB4GST = value; }
        }

        [DataMember]
        [Column(Name = "GSTValue")]
        public decimal GstValue
        {
            get { return gstValue; }
            set { gstValue = value; }
        }

        [DataMember]
        [Column(Name = "Source")]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        [DataMember]       
        public decimal IncomeAfterGST
        {
            get { return this.incomeB4GST - this.gstValue;  }
            private  set {  }
        }
    }
}