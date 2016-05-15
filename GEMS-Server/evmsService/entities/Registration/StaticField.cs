using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "StaticField")]
    public class StaticField
    {
        private int staticFieldID;
        private string fieldName;//pk? 
        private string fieldLabel;

        public StaticField()
        {
        }

        public StaticField(string fieldName, string fieldLabel)
        {
            this.FieldName = fieldName;
            this.FieldLabel = fieldLabel;
        }


        [DataMember]
        [Column(Name = "StaticFieldID", IsPrimaryKey = true, IsDbGenerated=true)]
        public int StaticFieldID
        {
            get { return staticFieldID; }
            set { staticFieldID = value; }
        }

        [DataMember]
        [Column(Name = "FieldName")]
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        [DataMember]
        [Column(Name = "FieldLabel")]
        public string FieldLabel
        {
            get { return fieldLabel; }
            set { fieldLabel = value; }
        }
    }
}