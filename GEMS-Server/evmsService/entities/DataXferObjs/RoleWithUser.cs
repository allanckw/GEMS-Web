using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using evmsService.Controllers;
using System.Collections.Generic;

namespace evmsService.entities
{
    [DataContract]
    public class RoleWithUser
    {
        [DataMember]
        public Role role;
        [DataMember]
        public string user;

        public RoleWithUser(Role role, string user)
        {
            this.role = role;
            this.user = user;
        }
    }
}