using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract(Name = "BookingStatus")]
    public enum BookingStatus
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