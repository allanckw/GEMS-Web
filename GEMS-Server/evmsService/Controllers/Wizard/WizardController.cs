using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using evmsService.entities;
using System.Transactions;
using evmsService.DataAccess;
using System.ServiceModel;


namespace evmsService.Controllers
{
    public class WizardController
    {
        public static void WizardAddEvent(User user, Events evnt, List<List<Program>> programs, List<List<Guest>> guests, List<ItemTypes> itemtypes, List<Items> items, Publish pub,List<Task> tasks)
        {
             
            try{

                if (!user.isEventOrganizer)
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Create Events!"));


                for (int x = 0; x < itemtypes.Count; x++)
                {
                    for(int y=0;y<itemtypes.Count;y++)
                    {
                        if (x == y)
                            continue;
                        if (itemtypes[x].typeString == itemtypes[y].typeString)
                        {
                            throw new Exception("Duplicate ItemType");
                        }
                    }
                }

                for (int x = 0; x < items.Count; x++)
                {
                    for (int y = 0; y < items.Count; y++)
                    {
                        if (x == y)
                            continue;
                        if (items[x].ItemName == items[y].ItemName && items[x].typeString == items[y].typeString)
                        {
                            throw new Exception("Duplicate ItemType");
                        }
                    }
                }



                DAL dalDataContext = new DAL();

                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    EventWithDays eventswithdays;

                    eventswithdays = EventDayController.AddEvent(user, evnt.Name, evnt.StartDateTime, evnt.EndDateTime, evnt.Description, 
                        evnt.Website, evnt.Tag, dalDataContext);

                    evnt = eventswithdays.e;

                    for (int x = 0; x < programs.Count; x++)
                    {
                        List<Program> daysprograms = programs[x];
                        for (int y = 0; y < daysprograms.Count; y++)
                        {
                            Program program = daysprograms[y];
                            ProgramController.AddProgram(program.Name, program.StartDateTime, program.EndDateTime, program.Description, eventswithdays.ds[x].DayID, program.Location, dalDataContext);
                        }
                    }

                    for (int x = 0; x < guests.Count; x++)
                    {
                        List<Guest> daysguests = guests[x];
                        for (int y = 0; y < daysguests.Count; y++)
                        {
                            Guest guest = daysguests[y];
                            GuestController.AddGuest(eventswithdays.ds[x].DayID, guest.Name, guest.Contact, guest.Description, dalDataContext);
                        }
                    }


                    for (int i = 0; i < itemtypes.Count; i++)
                    {
                        ItemTypesController.AddItemType(evnt.EventID, itemtypes[i].typeString, itemtypes[i].IsImportantType, dalDataContext);
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        ItemsController.AddItem(evnt.EventID, items[i].typeString, items[i].ItemName, items[i].Satisfaction, items[i].EstimatedPrice, dalDataContext);
                    }

                   // eventswithdays = EventDayController.AddEvent(u, );

                    //insert task

                    for (int i = 0; i < tasks.Count; i++)
                    {
                        TaskController.CreateTask(evnt.EventID, tasks[i].TaskName, tasks[i].TaskDesc, tasks[i].DueDate, dalDataContext);
                    }

                    //insert publish
                    if(pub.Remarks != null)
                    PublishController.AddPublish(evnt, pub.StartDateTime, pub.EndDateTime, pub.Remarks, pub.IsPayable, pub.PaymentAMount, dalDataContext);

                    dalDataContext.SubmitChanges();
                    
                    tScope.Complete();
                }

            }
            catch(Exception ex){

                throw new FaultException<SException>(new SException(),
                   new FaultReason("Error!, an error occured while adding in new event via wizard"));
                
            }
        }
    }
}