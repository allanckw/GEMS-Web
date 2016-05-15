﻿using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract(Name = "EnumFunctions")]
    public enum EnumFunctions
    {
        [EnumMember]
        Create_Programmes = 0,
        [EnumMember]
        Delete_Programmes = 1,
        [EnumMember]
        Edit_Programmes = 2,
        [EnumMember]
        Add_Guest = 3,
        [EnumMember]
        Delete_Guest = 4,
        [EnumMember]
        Edit_Guest = 5,
        [EnumMember]
        //roles and rights i put as 1 logic
        Add_Role = 6,
        [EnumMember]
        Delete_Role = 7,
        [EnumMember]
        Edit_Role = 8,
        [EnumMember]
        View_Role = 9,
        [EnumMember]
        Manage_ItemTypes = 10,
        [EnumMember]
        Manage_Items = 11,
        [EnumMember]
        OptimizeItemList = 12,
        [EnumMember]
        Manage_Income = 13,
        [EnumMember]
        Add_Task = 14,
        [EnumMember]
        Update_Task = 15,
        [EnumMember]
        Delete_Task = 16,
        [EnumMember]
        Assign_Task = 17,
        [EnumMember]
        View_Budget_Report = 18,
        [EnumMember]
        Manage_Participant = 19,
        [EnumMember]
        Manage_Requests = 20,
        [EnumMember]
        Manage_Artefacts = 21,
        [EnumMember]
        Manage_Facility_Bookings = 22
    }
}