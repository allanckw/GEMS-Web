using System.Runtime.Serialization;


namespace evmsService.entities
{
    [DataContract(Name = "RequestStatus")]
    public enum RequestStatus
    {
        [EnumMember]
        Pending,
        [EnumMember]
        Approved,
        [EnumMember]
        Rejected,
        [EnumMember]
        Cancelled
    }
}