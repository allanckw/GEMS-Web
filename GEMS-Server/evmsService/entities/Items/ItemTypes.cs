using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "EventItemTypes")]
    public class ItemTypes
    {
        int eventID;
        string itemType;
        bool isImpt;

        public ItemTypes()
        {
        }

        public ItemTypes(int eid, string itemType, bool impt)
        {
            this.eventID = eid;
            this.itemType = itemType;
            this.isImpt = impt;
        }

        [DataMember]
        [Column(Name = "EventID", IsPrimaryKey = true)]
        public int EventID
        {
            get
            {
                return eventID;
            }
            set
            {
                eventID = value;
            }
        }

        [DataMember]
        [Column(Name = "ItemType", IsPrimaryKey = true)]
        public string typeString
        {
            get
            {
                return this.itemType;
            }
            set
            {
                this.itemType = value;
            }
        }

        [DataMember]
        [Column(Name = "isImportant")]
        public bool IsImportantType
        {
            get
            {
                return this.isImpt;
            }
            set
            {
                this.isImpt = value;
            }
        }

        [DataMember]
        public List<Items> Items
        {
            private set { }
            get
            {
                return ItemsController.GetItemsList(this);
            }
        }
    }
}