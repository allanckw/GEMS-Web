using System.Runtime.Serialization;


namespace evmsService.entities
{
    [DataContract]
    public class ExportData
    {

        [DataMember]
        public Events evnts;
        [DataMember]
        public EventDay[] days;
        [DataMember]
        public Task[] tasks;
        [DataMember]
        public FacilityBookingConfirmed[][] facilities;
        [DataMember]
        public Program[][] programs;
        [DataMember]
        public Guest[][] guests;
        [DataMember]
        public Field[] field;
        [DataMember]
        public Participant[] participants;
        [DataMember]
        public OptimizedBudgetItems optitems;
        [DataMember]
        public BudgetIncome[] budgetincomes;





        public ExportData(Events evnt, EventDay[] days, Task[] tasks, FacilityBookingConfirmed[][] facilities, Program[][] programs, Guest[][] guests, Participant[] participants
        , OptimizedBudgetItems optitems, BudgetIncome[] budgetincomes, Field[] field)
        {
            this.evnts = evnt;
            this.days = days;
            this.tasks = tasks;
            this.facilities = facilities;
            this.programs = programs;
            this.guests = guests;
            this.participants = participants;
            this.optitems = optitems;
            this.budgetincomes = budgetincomes;
            this.field = field;
        }

    }
}