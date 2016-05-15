using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "EventItems")]
    public class Items
    {
        int eventID;
        string typeName;
        string itemName;
        int satisfaction;
        decimal estimatedPrice;
        decimal actualPrice;

        public Items()
        {
        }

        public Items(ItemTypes t, string itemName, int sat, decimal est)
        {
            this.eventID = t.EventID;
            this.typeName = t.typeString;
            this.itemName = itemName;
            this.satisfaction = sat;
            this.estimatedPrice = est;
            this.actualPrice = 0;
        }

        public Items(int eventID,string typeString, string itemName, int sat, decimal est)
        {
            this.eventID = eventID;
            this.typeName = typeString;
            this.itemName = itemName;
            this.satisfaction = sat;
            this.estimatedPrice = est;
            this.actualPrice = 0;
        }

        [DataMember]
        [Column(Name = "eItemType", IsPrimaryKey = true, DbType = "NVarChar(100)")]
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
        [Column(Name = "EventID", IsPrimaryKey = true, DbType = "int")]
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
        [Column(Name = "eItemName", IsPrimaryKey = true, DbType = "NVarchar(200)")]
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
        [Column(Name = "Satisfaction")]
        public int Satisfaction
        {
            get
            {
                return this.satisfaction;
            }
            set
            {
                this.satisfaction = value;
            }
        }

        [DataMember]
        [Column(Name = "EstimatedPrice", DbType = "NUMERIC(18,2)")]
        public decimal EstimatedPrice
        {
            get
            {
                return this.estimatedPrice;
            }
            set
            {
                this.estimatedPrice = value;
            }
        }

        [DataMember]
        [Column(Name = "ActualPrice", DbType = "NUMERIC(18,2)")]
        public decimal ActualPrice
        {
            get
            {
                return this.actualPrice;
            }
            set
            {
                this.actualPrice = value;
            }
        }
    }
}