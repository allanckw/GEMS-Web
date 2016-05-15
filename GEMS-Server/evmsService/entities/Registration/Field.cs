using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Field")]
    public class Field
    {
        private int fieldID;
        private string remarks;//can put this as extra feature such that user mouse over can see this info
        private string fieldLabel;//the display text
        private string fieldName;//the purpose of this field //alternatively maybe this can be PK?
        private int eventID;//fk
        private bool isRequired;
        

        public Field()
        {
            remarks = "";
        }

        public Field(string remarks, string fieldLabel, string fieldName, int eventID, bool isRequired)
        {
            this.Remarks = remarks;
            this.FieldLabel = fieldLabel;
            this.FieldName = fieldName;
            this.EventID = eventID;
            this.isRequired = isRequired;
        }



        [DataMember]
        [Column(Name = "FieldID", IsPrimaryKey = true, IsDbGenerated=true)]
        public int FieldID
        {
            get { return fieldID; }
            set { fieldID = value; }
        }

        [DataMember]
        [Column(Name = "Remark")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        [DataMember]
        [Column(Name = "FieldLabel")]
        public string FieldLabel
        {
            get { return fieldLabel; }
            set { fieldLabel = value; }
        }

        [DataMember]
        [Column(Name = "FieldName")]
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        [DataMember]
        [Column(Name = "EventID")]
        public int EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        [DataMember]
        [Column(Name = "IsRequired")]
        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }
    }
}