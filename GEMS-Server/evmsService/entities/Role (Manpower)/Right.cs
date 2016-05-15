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
    [Table(Name = "Rights")]
    public class Right
    {
        private int roleID;//pk
        private EnumFunctions functionEnum; //pk

        public Right()
        { }
        public Right(int RoleID, EnumFunctions FunctionEnum)
        {
            roleID = RoleID;
            functionEnum = FunctionEnum;
        }
        [DataMember]
        [Column(IsPrimaryKey = true, Name = "RoleID")]
        public int RoleID
        {
            get { return roleID; }
            set { roleID = value; }
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