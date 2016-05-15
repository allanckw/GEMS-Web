using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    [Table(Name = "Functions")]
    public class Function
    {

        private string description;
        private string grouping;
        private EnumFunctions functionEnum;


        [DataMember]
        [Column(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        [Column(Name = "Grouping")]
        public string Grouping
        {
            get { return grouping; }
            set { grouping = value; }
        }

        [DataMember]
        [Column(Name = "FunctionEnum")]
        public EnumFunctions FunctionEnum
        {
            get { return functionEnum; }
            set { functionEnum = value; }
        }

    }
}