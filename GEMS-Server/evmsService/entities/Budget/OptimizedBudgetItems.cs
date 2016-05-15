using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "OptimizedBudgetItems")]
    public class OptimizedBudgetItems
    {
        private int budgetID; //PK Autonumber 
        private int eventID; //PK cum FK
        private DateTime generatedDate;
        private string generator;
        private int totalSatisfaction;
        private decimal totalEstimatedPrice;

        public OptimizedBudgetItems() { }

        public OptimizedBudgetItems(int eventID, string user, int totalSat, decimal totalPrice)
        {
            this.eventID = eventID;
            this.generator = user;
            this.totalEstimatedPrice = totalPrice;
            this.totalSatisfaction = totalSat;
            this.generatedDate = DateTime.Today;

        }


        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "BudgetID")]
        public int BudgetID
        {
            get { return budgetID; }
            set {budgetID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "GeneratedDate")]
        public DateTime GeneratedDate
        {
            get { return generatedDate; }
            set { generatedDate = value; }
        }

        [DataMember]
        [Column(Name = "Generator")]
        public string Generator
        {
            get { return generator; }
            set { generator = value; }
        }

        [DataMember]
        [Column(Name = "totalSatisfaction")]
        public int TotalSatisfaction
        {
            get { return totalSatisfaction; }
            set { totalSatisfaction = value; }
        }

        [DataMember]
        [Column(Name = "totalPrice")]
        public decimal TotalEstimatedPrice
        {
            get { return totalEstimatedPrice; }
            set { totalEstimatedPrice = value; }
        }

        [DataMember]
        public List<OptimizedBudgetItemsDetails> BudgetItemsList
        {
            private set { }
            get { return Controllers.BudgetDetailsController.GetBudgetItems(this); }
        }

    }
}