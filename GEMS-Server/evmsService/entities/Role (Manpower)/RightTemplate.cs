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
    [Table(Name = "RightsTemplate")]
    public class RightTemplate
    {
        private int roleTemplateID;//pk
        private EnumFunctions functionEnum; //pk

        public RightTemplate()
        { }
        public RightTemplate(int RoleTemplateID, EnumFunctions FunctionEnum)
        {
            roleTemplateID = RoleTemplateID;
            functionEnum = FunctionEnum;
        }
        [DataMember]
        [Column(IsPrimaryKey = true, Name = "RoleTemplateID")]
        public int RoleTemplateID
        {
            get { return roleTemplateID; }
            set { roleTemplateID = value; }
        }

        [DataMember]
        [Column(IsPrimaryKey = true, Name = "FunctionEnum")]
        public EnumFunctions FunctionEnum
        {
            get { return functionEnum; }
            set { functionEnum = value; }
        }
    }
}