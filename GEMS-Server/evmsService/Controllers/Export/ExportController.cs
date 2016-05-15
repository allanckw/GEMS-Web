using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using evmsService.Controllers;
using evmsService.entities;

using System.ServiceModel;


namespace evmsService.Controllers
{
    public class ExportController
    {
        public static ExportData GetExportData(User u, int eventID, Boolean NeedFacilities, Boolean NeedPrograms, Boolean NeedIncome, Boolean NeedOptItems, Boolean NeedTasks
        , Boolean NeedGuest, Boolean NeedParticipant)
        {


            Events evnt = EventController.GetEvent(eventID);
            if (!evnt.Organizerid.Equals(u.UserID))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Export!"));

            EventDay[] days = DayController.GetDays(eventID).ToArray();
            List<Program[]> tempprograms = new List<Program[]>();
            List<Guest[]> tempguests = new List<Guest[]>();
            List<FacilityBookingConfirmed[]> tempfacilities = new List<FacilityBookingConfirmed[]>();

            for (int i = 0; i < days.Count(); i++)
            {
                tempprograms.Add(ProgramController.ViewProgram(days[i].DayID).ToArray());
                tempguests.Add(GuestController.ViewGuest(days[i].DayID).ToArray());

                tempfacilities.Add(FaciConfirmedBookingController.GetEventConfirmedDetail(eventID, days[i].DayDate).ToArray());

            }



            FacilityBookingConfirmed[][] facilities = null;

            if (NeedFacilities)
                facilities = tempfacilities.ToArray();

            Program[][] programs = null;

            if (NeedPrograms)
                programs = tempprograms.ToArray();

            Guest[][] guests = null;

            if (NeedGuest)
                guests = tempguests.ToArray();

            Task[] tasks = null;
            if (NeedTasks)
                tasks = TaskController.GetTasksByEvent(eventID).ToArray();


            Participant[] participant = null;
            Field[] field = null;
            if (NeedParticipant)
            {

                participant = ParticipantController.ViewParticipant(eventID).ToArray();
                field = FieldController.ViewField(eventID).ToArray();
            }

            //ItemTypes[] itemtypes = ItemTypesController.GetExistingItemTypes(eventID).ToArray();
            //Items[] items = ItemsController.GetItemsList(eventID).ToArray();

            OptimizedBudgetItems optitems = null;

            if (NeedOptItems)
                optitems = BudgetItemController.GetBudget(eventID);

            BudgetIncome[] budgetincomes = null;
            if (NeedIncome)
                budgetincomes = BudgetIncomeController.ViewBudgetIncome(u, eventID).ToArray();



            //FacilityBookingConfirmed 



            //ExportData epr = new ExportData();
            //public static
            return new ExportData(evnt, days, tasks, facilities, programs, guests, participant, optitems, budgetincomes, field);

        }
    }


}