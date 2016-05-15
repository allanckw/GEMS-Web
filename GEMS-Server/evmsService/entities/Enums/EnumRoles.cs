using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract(Name = "EnumRoles")]
    public enum EnumRoles
    {
        [EnumMember]
        System_Admin = 0,
        [EnumMember]
        Facility_Admin = 1,
        [EnumMember]
        Event_Organizer = 2,
        [EnumMember]
        Nil = 3
    }
}