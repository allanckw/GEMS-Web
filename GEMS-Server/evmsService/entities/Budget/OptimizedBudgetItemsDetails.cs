using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "OptimizedBudgetItemsDetails")]
    public class OptimizedBudgetItemsDetails
    {
        private  int budgetDetailID; //PK

        private int budgetID; //FK to Budget
        private int eventID; //FK To Budget And Items
        
        private string typeName; //FK To Items
        private string itemName; //FK To Items

        private bool isBought;

   

        public OptimizedBudgetItemsDetails() { }

        public OptimizedBudgetItemsDetails(int BudgetID, int eventID, string typeName, string itemName)
        {
            this.BudgetID = BudgetID;
            this.eventID = eventID;
            this.typeName = typeName;
            this.itemName = itemName;
            isBought = false;
        }


        [DataMember]
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "BudgetDetailID")]
        public int BudgetDetailID
        {
            get { return budgetDetailID; }
            set { budgetDetailID = value; }
        }

        [DataMember]
        [Column(Name = "BudgetID")]
        public int BudgetID
        {
            get { return budgetID; }
            set { budgetID = value; }
        }

        [DataMember]
        [Column(Name = "eItemType", DbType = "NVarChar(100)")]
        public string typeString
        {
            get
            {
                return this.typeName;
            }
            set
            {
                this.typeName = value;
            }
        }

        [DataMember]
        [Column(Name = "EventID",  DbType = "int")]
        public int EventID
        {
            get
            {
                return this.eventID;
            }
            set
            {
                this.eventID = value;
            }
        }

        [DataMember]
        [Column(Name = "eItemName",  DbType = "NVarchar(200)")]
        public string ItemName
        {
            get
            {
                return this.itemName;
            }
            set
            {
                this.itemName = value;
            }
        }

        [DataMember]
        [Column(Name = "isBought")]
        public bool IsBought
        {
            get { return isBought; }
            set { isBought = value; }
        }
    }
}