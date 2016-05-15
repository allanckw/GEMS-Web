using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract]
    public class ParticipantWithName
    {
        [DataMember]
        public Participant participant;
        [DataMember]
        public string name;

        public ParticipantWithName(Participant participant, string name)
        {
            this.participant = participant;
            this.name = name;
        }
    }
}