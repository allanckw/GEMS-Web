using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;
//To Add A new Service for IOS
using System.ServiceModel.Activation;
using System.Web.Configuration;

namespace evmsService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [DataContractFormat(Style = OperationFormatStyle.Document)]
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class EvmsService :
        IEvent, IProgramme, IFacility, IFacilityBookings,
        IEventItems, IRegistration, IRole, ITasks, IGuest,
        IAdministration, INotifications, IServiceContact, IBudget,
        IParticipantsTransactions, IWizard, IMobile, IRequest, IArtefact, IExport
    {

        //Administration 
        #region "ADSS"
        public User SecureAuthenticate(Credentials credentials)
        {
            User user = null;
            try
            {
                user = UserController.authenticate(credentials);
                if (user == null)
                {
                    throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid login details, please try again"));
                }
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new FaultException<SException>(new SException(),
                   new FaultReason(e.Message));
            }
        }

        public void AssignLocationAdmin(User assigner, string userid, Faculties fac)
        {

            SysRoleController.AddFacilityAdmin(assigner, userid, fac);
        }

        public void AssignEventOrganizer(User assigner, string userid, string description)
        {
            SysRole s = new SysRole(userid, EnumRoles.Event_Organizer, description);
            SysRoleController.AddRole(assigner, s);
        }

        public void AssignSystemAdmin(User assigner, string userid, string description)
        {
            SysRole s = new SysRole(userid, EnumRoles.System_Admin, description);
            SysRoleController.AddRole(assigner, s);
        }

        public void UnAssignRole(User assigner, string userid)
        {
            SysRoleController.RemoveRole(assigner, userid);
        }

        public List<User> SearchUser(string name, string userid)
        {
            return UserController.searchUsers(name, userid);
        }

        public List<User> SearchUserByRole(string name, string userid, EnumRoles r)
        {
            return UserController.searchUser(name, userid, r);
        }


        public EnumRoles ViewUserRole(string userid)
        {
            return SysRoleController.GetRole(userid);
        }

        public string GetUserName(string userid)
        {
            return UserController.GetUserName(userid);
        }

        public string GetUserEmail(string userid)
        {
            return UserController.GetUserMail(userid);
        }

        public int GetClientTimeOut()
        {
            return int.Parse(WebConfigurationManager.AppSettings["clientTimeout"]);
        }
        #endregion

        #region "Notifications Sub System"
        public string GetNewMessage(User user, string receiverID)
        {
            return NotificationController.GetLastNotification(user, receiverID);
        }

        public int GetUnreadMessageCount(User user, string receiverID)
        {
            return NotificationController.ViewAllNotifications(user, receiverID, false).Count;
        }

        public List<Notifications> GetUnreadMessages(User user, string receiverID)
        {
            return NotificationController.ViewAllNotifications(user, receiverID, false);
        }

        public List<Notifications> GetAllMessages(User user, string receiverID)
        {
            return NotificationController.ViewAllNotifications(user, receiverID, true);
        }

        public void DeleteNotifications(User user, Notifications msg)
        {
            NotificationController.deleteNotification(user, msg);
        }

        public void DeleteAllNotificationsOfUser(User user, string uid)
        {
            NotificationController.deleteAllNotifications(user, uid);
        }

        public void SetNotificationRead(User user, Notifications msg)
        {
            NotificationController.setToRead(user, msg);
        }

        public void SendNotification(string sender, string receiver, string title, string msg)
        {
            NotificationController.sendNotification(sender, receiver, title, msg);
        }
        #endregion

        //Event MS 
        #region "EvMS"
        public void CreateEvent(User user, string EventName, DateTime EventStartDateTime, DateTime EventEndDatetime,
        string EventDescription, string EventWebsite, string EventTag)
        {
            //EventController.AddEvent(user, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag);
            EventDayController.AddEvent(user, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag);
        }

        public List<Events> ViewUserAssociatedEvent(User user)
        {
            return EventController.ViewUserAssociatedEvent(user);
        }

        public List<Events> ViewOrganizerEvent(User user)
        {
            return EventController.ViewOrganizerEvent(user);
        }

        public List<Events> ViewEventsByDateAndTag(User user, DateTime start, DateTime end, string tag)
        {
            return EventController.ViewEventsByDateTag(user, start, end, tag);
        }

        public List<Events> ViewEventsByTag(User user, string tag)
        {
            return EventController.ViewEventsByTag(user, tag);
        }

        public void EditEvent(User user, Events evnt, string EventName, DateTime EventStartDateTime,
            DateTime EventEndDatetime, string EventDescription, string EventWebsite, string EventTag)
        {
            //EventController.EditEvent(user, evnt, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag);
            //
            EventDayController.EditEvent(user, evnt, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite, EventTag);

        }

        public void DeleteEvent(User user, Events evnt)
        {
            EventController.DeleteEvent(user, evnt);
        }

        public string GetEventName(int eventid)
        {
            return EventController.GetEvent(eventid).Name;
        }

        public Events GetEvent(int eventid)
        {
            return EventController.GetEvent(eventid);
        }

        public List<Events> ViewAllEvents(User user)
        {
            return EventController.ViewAllEvents(user.UserID);
        }

        public EventDay GetDay(int DayID)
        {
            return DayController.GetDay(DayID);
        }

        public List<EventDay> GetDays(int EventID)
        {
            return DayController.GetDays(EventID);
        }

        public Boolean isEventExist(int eventID)
        {
            return EventController.isEventExist(eventID);
        }

        public List<Events> ViewAuthorizedEventsForFacBookings(User sender)
        {
            return FaciRequestController.ViewAuthorizedEventsForFacBookings(sender);
        }
        #endregion

        //Manage Programs
        #region "Manage Programs"
        public int AddProgram(User user, string ProgramName, DateTime ProgramStartDateTime, DateTime ProgramEndDatetime,
        string ProgramDescription, int ProgramDayID, string ProgramLocaton)
        {
            return ProgramController.AddProgram(user, ProgramName, ProgramStartDateTime, ProgramEndDatetime, ProgramDescription, ProgramDayID, ProgramLocaton);
        }

        public List<Program> ViewProgram(int dayID)
        {
            return ProgramController.ViewProgram(dayID);
        }

        public void EditProgram(User user, int ProgramID, string ProgramName, DateTime ProgramStartDateTime
        , DateTime ProgramEndDatetime, string ProgramDescription, string ProgramLocaton)
        {
            ProgramController.EditProgram(user, ProgramID, ProgramName, ProgramStartDateTime
        , ProgramEndDatetime, ProgramDescription, ProgramLocaton);
            //
        }

        public void DeleteProgram(User user, int ProgramID)
        {
            ProgramController.DeleteProgram(user, ProgramID);
        }

        public bool ValidateProgramTime(int dayID, DateTime segmentStart, DateTime segmentEnd)
        {
            return ProgramController.ValidateProgramTime(dayID, segmentStart, segmentEnd);
        }

        public void SwapProgram(User user, int ProgramID1, int ProgramID2)
        {
            ProgramController.SwapProgram(user, ProgramID1, ProgramID2);
        }

        public List<int> GetEventProgCount(int eventID)
        {
            return ProgramController.GetEventProgCount(eventID);
        }
        #endregion

        #region "Logistics

        //Manage Guest
        #region "Logistics - Manage Guest"
        public int AddGuest(User user, int dayID, string GuestName, string GuestContact, string GuestDescription)
        {
            return GuestController.AddGuest(user, dayID, GuestName, GuestContact, GuestDescription);
        }

        public List<Guest> ViewGuest(int dayID)
        {
            return GuestController.ViewGuest(dayID);
        }

        //public int GetEventGuestCount(int dayID)
        //{
        //    return GuestController.ViewGuest(dayID).Count;
        //}

        public void EditGuest(User user, int GuestID, string GuestName, string GuestDescription, string GuestContact)
        {
            GuestController.EditGuest(user, GuestID, GuestName, GuestDescription, GuestContact);
        }

        public void DeleteGuest(User user, int GuestID)
        {
            GuestController.DeleteGuest(user, GuestID);
        }

        public List<int> GetEventGuestCount(int eventID)
        {
            return GuestController.GetEventGuestCount(eventID);
        }
        #endregion

        //Manage Role
        #region "Logistics - Manage Role"
        public int AddRole(User user, string RoleUserID, int eventID, string RolePost, string RoleDescription, List<EnumFunctions> functionID)
        {
            return RoleLogicController.AddRoleAndRights(user, RoleUserID, eventID, RolePost, RoleDescription, functionID);
        }

        public void DeleteRole(User user, int RoleID)
        {
            RoleLogicController.DeleteRole(user, RoleID);
        }

        public void EditRole(User user, string RoleUserID, int RoleID, string RolePost, string RoleDescription, List<EnumFunctions> FunctionList)
        {
            RoleLogicController.EditRole(user, RoleUserID, RoleID, RolePost, RoleDescription, FunctionList);
        }

        public List<EnumFunctions> GetRights(int eventID, string userID)
        {
            return RoleLogicController.GetRights(eventID, userID);
        }

        public bool haveRightsTo(int eventID, string userID, EnumFunctions fx)
        {
            return RoleLogicController.HaveRights(EventController.GetEvent(eventID), UserController.GetUser(userID), fx);
        }

        public List<Role> ViewRole(User user, Events evnt)
        {
            return RoleController.ViewUserRolesForEvent(user, evnt);
        }

        public List<RoleWithUser> ViewEventRoles(User user, Events evnt)
        {
            return RoleController.ViewEventRoles(user, evnt);
        }

        public List<Function> ViewFunction()
        {
            return FunctionController.ViewFunction();
        }

        public List<Role> ViewUserEventRoles(string userID, int eventID)
        {
            return RoleController.ViewUserEventRoles(userID, eventID);
        }

        public bool isEventFacilitator(string userid, int eventID)
        {
            return RoleLogicController.isEventFacilitator(eventID, userid);
        }
        #region "RoleTemplate Management"
        public int AddRoleTemplate(User user, Events evnt, string RolePost, string RoleDescription, List<EnumFunctions> functionID)
        {
            return RoleTemplateLogicController.
                AddRightsTemplate(user, evnt, RolePost, RoleDescription, functionID);
        }

        public void DeleteRoleTemplate(User user, int RoleID)
        {
            RoleTemplateLogicController.DeleteRoleTemplate(user, RoleID);
        }

        public void EditRightsTemplate(User user, int RoleID, string RolePost, string RoleDescription, List<EnumFunctions> functionID)
        {
            RoleTemplateLogicController.EditRightsTemplate(user, RoleID, RolePost, RoleDescription, functionID);
        }

        public List<RoleTemplate> ViewTemplateRole(User user, Events evnt)
        {
            return RoleTemplateController.ViewRoleTemplates(user, evnt);
        }

        public List<RightTemplate> GetTemplateRight(int RoleTemplateID)
        {
            return RightTemplateController.GetTemplateRights(RoleTemplateID);
        }
        #endregion
        #endregion

        //Manage Facilities
        #region "Logistics - Facilites Sub System"
        public void RemoveFacility(User user, string venue, Faculties fac)
        {
            FacilityController.RemoveFacility(user, venue, fac);
        }

        public void UpdateFacility(User user, string venue, Faculties faculty, string loc,
            string bookingCon, string techCon, int cap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            FacilityController.UpdateFacility(user, venue, faculty, loc, bookingCon, techCon, cap, rtype,
                hasWebcast, hasFlexiSeat, hasVidConf, hasMIC, hasProjector, hasVisualizer);
        }

        public Faculties GetFacilityAdmin(string userid)
        {
            return SysRoleController.GetFaculty(userid);
        }

        public string GetFacilityAdminFaculty(Faculties fac)
        {
            return SysRoleController.GetFacultyAdmin(fac);
        }

        public List<User> GetFacilityAdmins()
        {
            return UserController.searchUser("", "", EnumRoles.Facility_Admin);
        }

        public List<Facility> SearchFacilities(Faculties fac, int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
              bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            return FacilityController.SearchFacilities(fac, minCap, maxCap, rtype, hasWebcast, hasFlexiSeat, hasVidConf, hasMIC, hasProjector, hasVisualizer);
        }

        public List<Faculties> SearchFacilitiesFac(int minCap, int maxCap, RoomType rtype, bool hasWebcast, bool hasFlexiSeat,
            bool hasVidConf, bool hasMIC, bool hasProjector, bool hasVisualizer)
        {
            return FacilityController.SearchFacilitiesFacs(minCap, maxCap, rtype, hasWebcast, hasFlexiSeat, hasVidConf, hasMIC, hasProjector, hasVisualizer);
        }
        //public List<Facility> GetVenues(int minCap = 0, int maxCap = int.MaxValue)
        //{
        //    return FacilityController.GetVenues(minCap, maxCap);
        //}

        public List<Facility> GetVenuesByFaculty(Faculties fac)
        {
            return FacilityController.GetVenues(fac);
        }

        #endregion

        //Facilities Booking
        #region "Logistics - Facilites Booking Sub System"
        public bool AddFacilityBookingRequest(User user, EventDay evntDay, Faculties faculty, DateTime reqstart,
        DateTime reqEnd, List<FacilityBookingRequestDetails> reqDetails)
        {
            return FacilityBookingController.AddBookingRequest(user, evntDay, reqstart, reqEnd, faculty, reqDetails);
        }

        public bool CheckRequestExist(int eventid, DateTime startTime, DateTime endTime)
        {
            return FacilityBookingController.checkRequestExists(eventid, startTime, endTime);
        }

        public List<FacilityBookingRequest> GetFacBookingRequestList(User FacAdmin)
        {
            return FacilityBookingController.ViewBookingRequestByFaculty(FacAdmin);
        }

        public void ApproveFacilityBooking(User approver, int reqID, int evID, int reqDetailID,
            string remarks, string purpose)
        {
            FacilityBookingController.approveBookingRequest(approver, reqID, evID,
                reqDetailID, remarks, purpose);
        }

        public void RejectFacilityBooking(User rejecter, int reqID, int evID, string remarks)
        {
            FacilityBookingController.rejectBookingRequest(rejecter, reqID, evID, remarks);
        }

        public void DropConfirmedRequest(User user, int reqID, int evID, string remarks)
        {
            FacilityBookingController.dropConfirmedBookingRequest(user.UserID, reqID, evID, remarks);
        }

        public void CancelFacilityBooking(User user, int reqID, int evID, string remarks)
        {
            FacilityBookingController.cancelBookingRequest(user.UserID, reqID, evID, remarks);
        }


        public List<FacilityBookingRequest> ViewFacilityBookingRequestsByEventDay(User user, int evID,
           BookingStatus status, bool viewAllStatus, bool ViewAllDays, DateTime day)
        {
            return FacilityBookingController.ViewBookingRequestByStatusNEventDay(user.UserID, status, evID,
                viewAllStatus, ViewAllDays, day);
        }


        public List<FacilityBookingConfirmed> GetActivitiesForDay(User user, DateTime day,
            Faculties faculty, string venue)
        {
            return FacilityBookingController.GetActivitesForLocation(user, day, faculty, venue);
        }

        public FacilityBookingConfirmed GetConfirmedBooking(int reqID)
        {
            return FacilityBookingController.GetConfirmedDetail(reqID);
        }

        public List<FacilityBookingConfirmed> GetConfirmedFacBookings(int eventID, DateTime dayDate)
        {
            return Controllers.FaciConfirmedBookingController.GetEventConfirmedDetail(eventID, dayDate);
        }

        public int GetConfirmedFacBookingsCount(int eventID, DateTime dayDate)
        {
            return Controllers.FaciConfirmedBookingController.GetEventConfirmedDetail(eventID, dayDate).Count;
        }

        #endregion


        //Item Sub System - Kok Wei & Ji Kai
        #region "Logistics - Event Item Management Sub System"
        #region "Item Type Repository"
        public List<string> GetItemsTypes()
        {
            return ItemTypeRepository.GetItemTypeList();
        }

        public void AddItemsTypes(string itemType)
        {
            ItemTypeRepository.AddItemType(itemType);
        }

        public void UpdateItemTypes(string itemType, string newItemType)
        {
            ItemTypeRepository.UpdateItemType(itemType, newItemType);
        }

        public void DeleteItemType(string itemType)
        {
            ItemTypeRepository.deleteItemType(itemType);
        }
        #endregion

        //Event Specific Item Type
        public ItemTypes AddEventItemType(User user, int evid, string type, bool isImpt)
        {
            return ItemTypesController.AddItemType(user, evid, type, isImpt);
        }

        public void DeleteEventItemType(User user, ItemTypes type)
        {
            ItemTypesController.deleteItemType(user, type);
        }

        public List<ItemTypes> GetEventSpecificItemType(int eventid)
        {
            return ItemTypesController.GetExistingItemTypes(eventid);
        }

        public void SetItemTypeImportance(User user, ItemTypes type, Boolean isImpt)
        {
            ItemTypesController.setItemTypeImportance(user, type, isImpt);
        }

        public ItemTypes GetEventItemType(int eventID, string typeName)
        {
            return ItemTypesController.GetItemType(eventID, typeName);
        }
        //Event Specific Item
        public Items AddEventItem(User user, ItemTypes type, string name, int sat, decimal est)
        {
            return ItemsController.AddItem(user, type, name, sat, est);
        }

        public void DeleteEventItem(User user, Items iten)
        {
            ItemsController.deleteItem(user, iten);
        }

        public void UpdateSatifactionAndEstPrice(User user, Items iten, int s, decimal est)
        {
            ItemsController.UpdateSatifactionAndEstPrice(user, iten, s, est);
        }

        public List<Items> GetItemsByEvent(int eventID)
        {
            return ItemsController.GetItemsList(eventID);
        }

        #endregion

        #endregion

        #region "Tasks"
        public void CreateTask(User user, int eventID, string taskName,
            string taskDesc, DateTime DueDate)
        {
            TaskController.CreateTask(user, eventID, taskName, taskDesc, DueDate);
        }

        public void DeleteTask(User user, int eventID, int taskID)
        {
            TaskController.DeleteTask(user, taskID, eventID);
        }

        public void UpdateTask(User user, int eventID, int taskID, string taskName, string taskDesc,
            DateTime dueDate)
        {
            TaskController.UpdateTask(user, taskID, eventID, taskName, taskDesc, dueDate);
        }

        public void SetTaskRead(Task task, int roleID)
        {
            TaskAssignmentController.SetToRead(task, roleID);
        }

        public void SetTaskCompleted(Task task, int roleID, string remarks)
        {
            TaskAssignmentController.SetCompleted(task, roleID, remarks);
        }

        public void SetTaskIncomplete(User user, Task task, int roleID, string remarks)
        {
            TaskAssignmentController.SetInComplete(user, task, roleID, remarks);
        }

        public List<Task> GetTasksByEvent(string userID, int eventID)
        {
            return TaskController.GetTasksByEvent(userID, eventID);
        }

        public List<Task> GetTaskByRole(int eventID, int roleID)
        {
            return TaskController.ViewTasksByRole(eventID, roleID);
        }

        public TaskAssignment GetTaskAssignment(int taskID, int eventID, int roleID)
        {
            return TaskAssignmentController.GetTaskAssignment(taskID, eventID, roleID);
        }

        public void AssignTask(User user, int eventID, int roleID, List<Task> taskList)
        {
            TaskAssignmentController.AssignTasks(user, eventID, roleID, taskList);
        }

        public Task GetTask(int taskID)
        {
            return TaskController.GetTask(taskID);
        }

        public List<Task> GetEventTasks(int eventID)
        {
            return TaskController.GetEventTasks(eventID);
        }
        #endregion

        #region "Budget"

        public void SaveBudgetList(User sender, int eventID, int totalSat, decimal totalPrice,
            List<Items> itemList)
        {
            BudgetItemController.SaveBudgetItemList(sender, eventID, totalSat, totalPrice, itemList);
        }

        public OptimizedBudgetItems GetBudgetItem(int eventID)
        {
            return BudgetItemController.GetBudget(eventID);
        }

        public Items GetItemDetail(OptimizedBudgetItemsDetails bItem)
        {
            return ItemsController.GetItem(bItem);
        }

        public void UpdateActualPrice(User user, Items iten, decimal price)
        {
            ItemsController.UpdateActualPrice(user, iten, price);
        }

        public int GetGST()
        {
            return GSTController.GetGST();
        }

        public void UpdateGST(User user, int newPercentage)
        {
            GSTController.UpdateGST(user, newPercentage);
        }
        public int AddIncome(User user, int eventID, BudgetIncome bIncome)
        {
            return BudgetIncomeController.AddBudgetIncome(user, eventID, bIncome);
        }

        public void DeleteBudgetIncome(User user, int BudgetIncomeID, int eventID)
        {
            BudgetIncomeController.DeleteBudgetIncome(user, BudgetIncomeID, eventID);
        }

        public void EditBudgetIncome(User user, int eventID, int incomeID, BudgetIncome bIncome)
        {
            BudgetIncomeController.EditBudgetIncome(user, eventID, incomeID, bIncome);
        }

        public List<BudgetIncome> ViewBudgetIncome(User user, int eventID)
        {
            return BudgetIncomeController.ViewBudgetIncome(user, eventID);
        }


        #endregion

        #region "Service Contact List"

        #region "Manage Service"

        public void AddService(User user, int eventID, string Address, string name, string url, string notes)
        {
            ServiceController.AddService(user, eventID, Address, name, url, notes);
        }

        public void EditService(int ServiceID, User user, int eventID, string Address, string name,
            string url, string notes)
        {
            ServiceController.EditService(ServiceID, user, eventID, Address, name, url, notes);
        }

        public void DeleteService(User user, int serviceID)
        {
            ServiceController.DeleteService(user, serviceID);
        }

        public List<Service> ViewService(string SearchString)
        {
            return ServiceController.ViewService(SearchString);
        }

        #endregion

        #region "Manage Point Of Contact"
        public void AddPointOfContact(User user, int eventID, int serviceID, string name, string position, string phone, string email)
        {
            PointOfContactController.AddPointOfContact(user, eventID, serviceID, name, position, phone, email);
        }

        public void EditPointOfContact(User user, int eventID, int PointOfContactID, string name, string position, string phone, string email)
        {
            PointOfContactController.EditPointOfContact(user, eventID, PointOfContactID, name, position, phone, email);
        }

        public void DeletePointOfContact(User user, int PointOfContactID)
        {
            PointOfContactController.DeletePointOfContact(user, PointOfContactID);
        }

        public List<PointOfContact> ViewPointOfContact(int serviceID)
        {
            return PointOfContactController.ViewPointOfContact(serviceID);
        }
        #endregion

        #region "Manage Review"
        public void Review(User user, int eventID, int serviceID, int rating, DateTime reviewDate, string reviewDescription)
        {
            ReviewController.Review(user, eventID, serviceID, rating, reviewDate, reviewDescription);
        }

        public void DeleteReview(User user, string UserID, int ServiceID)
        {
            ReviewController.DeleteReview(user, UserID, ServiceID);
        }

        public List<Review> ViewReview(int ServiceID)
        {
            return ReviewController.ViewReview(ServiceID);
        }
        #endregion

        #endregion

        #region "Registration & Publish"

        #region "Registration"
        public void RegisterParticipant(int eventID, List<QuestionIDWithAnswer> answers)
        {
            Registration.RegisterParticipant(eventID, answers);
        }

        public void DeleteParticipant(User user, int eventID, int participantID)
        {

            Registration.DeleteParticipant(user, eventID, participantID);
        }

        public List<ParticipantWithName> ViewEventParticipantWithName(User user, int eventID)
        {
            return Registration.ViewEventParticipantWithName(user, eventID);
        }

        public List<Participant> ViewEventParticipant(User user, int eventID)
        {
            return Registration.ViewEventParticipant(user, eventID);
        }

        public List<FieldAnswer> GetParticipantFieldAnswer(User user, int eventID, int ParticipantID)
        {
            return Registration.GetParticipantFieldAnswer(user, eventID, ParticipantID);
        }

        public Participant EditParticipant(int ParticipantID, bool paid)
        {
            return ParticipantController.EditParticipant(ParticipantID, paid);
        }



        public void AddField(User user, int eventID, List<Field> ListField)
        {
            FieldController.AddField(user, eventID, ListField);
        }

        public List<StaticField> ViewStaticField()
        {
            return StaticFieldController.ViewStaticField();
        }

        public List<Field> ViewField(int eventID)
        {
            return FieldController.ViewField(eventID);
        }




        #endregion

        #region "Publish"

        public void AddPublish(User user, int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, Boolean isPayable, decimal paymentAmount)
        {
            PublishController.AddPublish(user, eventID, startDateTime, endDateTime, remarks, isPayable, paymentAmount);
        }



        public void DeletePublish(User user, int eventID)
        {
            PublishController.DeletePublish(user, eventID);
        }

        public void EditPublish(User user, int eventID, DateTime startDateTime, DateTime endDateTime, string remarks, bool isPayable, decimal amount)
        {
            PublishController.EditPublish(user, eventID, startDateTime, endDateTime, remarks, isPayable, amount);
        }

        public Publish ViewPublish(int eventID)
        {
            return PublishController.ViewPublish(eventID);
        }

        public List<Events> ViewEventForPublish(DateTime start, DateTime end)
        {
            return PublishController.ViewEvent(start, end);
        }

        public List<Events> ViewEventForPublishByDateAndTag(DateTime start, DateTime end, String tag)
        {
            return PublishController.ViewEventDateAndTag(start, end, tag);
        }
        #endregion

        #endregion

        #region "Participants For PayPal Integrations"

        public List<ParticipantEvent> ParticipantViewEvents(string participantEmail, DateTime start, DateTime end, bool paid)
        {
            return PublishController.ParticipantViewEvents(participantEmail, start, end, paid);
        }

        public void SetPaid(string Email, int eventID)
        {
            ParticipantController.SetPaid(Email, eventID);
        }

        public bool isRegistered(string Email)
        {
            return ParticipantController.isRegistered(Email);
        }

        public bool isEventRegistered(string Email, int EventID)
        {
            return ParticipantController.EventRegistered(Email, EventID);
        }

        public void SaveTransaction(string transID, List<ParticipantTransaction> trans)
        {
            ParticipantController.SaveTransaction(transID, trans);
        }

        public List<ParticipantTransaction> ViewParticipantsTransactions(string email, DateTime fromDate, DateTime toDate)
        {
            return ParticipantController.ViewParticipantTransactions(email, fromDate, toDate);
        }

        #endregion

        #region "Event Artefacts"

        //Folders
        public void CreateFolder(User user, int eventID, string folderName, string folderDesc, string remarks)
        {
            ArtefactWSController.CreateFolders(user, eventID, folderName, folderDesc, remarks);
        }

        public void DeleteFolder(User user, int eventID, string folderName)
        {
            ArtefactWSController.DeleteFolder(user, eventID, folderName);
        }

        public void UpdateFolder(User user, int eventID, string folderName, string folderDesc, string remarks)
        {
            ArtefactWSController.UpdateFolder(user, eventID, folderName, folderDesc, remarks);
        }

        public WorkspaceFolders GetWorkSpaceFolder(User user, int eventID, string folderName)
        {
            return ArtefactWSController.GetWorkSpaceFolder(user, eventID, folderName);
        }

        public List<WorkspaceFolders> GetWorkSpaceFolders(User user, int eventID)
        {
            return ArtefactWSController.GetWorkSpaceFolders(user, eventID);
        }

        //File
        public void UploadFile(User user, int eventID, string folderName, string fileName, string fileDesc, string fileURL)
        {
            ArtefactWSController.UploadNewFile(user, eventID, folderName, fileName, fileDesc, fileURL);
        }

        public void DeleteFile(User user, int eventID, string FolderName, string fileName)
        {
            ArtefactWSController.DeleteFile(user, eventID, FolderName, fileName);
        }

        public void UpdateFile(User user, int eventID, string folderName, string fileName, string fileDesc, string fileURL)
        {
            ArtefactWSController.UpdateFile(user, eventID, folderName, fileName, fileDesc, fileURL);
        }

        public WorkspaceFiles GetWorkSpaceFile(User user, int eventID, string folderName, string fileName)
        {
            return ArtefactWSController.GetWorkSpaceFile(user, eventID, folderName, fileName);
        }

        public List<WorkspaceFiles> GetWorkSpaceFiles(User user, int eventID, string folderName)
        {
            return ArtefactWSController.GetWorkSpaceFiles(user, eventID, folderName);
        }

        #endregion

        #region "Wizard"

        public void WizardAddEvent(User user, Events evnt, List<List<Program>> programs, List<List<Guest>> guests, List<ItemTypes> itemtypes, List<Items> items, Publish pub, List<Task> tasks)
        {
            WizardController.WizardAddEvent(user, evnt, programs, guests, itemtypes, items, pub, tasks);
        }
        #endregion

        #region "Mobile"
        public List<M_ShortEvent> m_GetEventsWithTag(DateTime fromDate, DateTime toDate, string tag)
        {
            return MobileController.GetEventsWithTag(fromDate, toDate, tag);
        }

        public List<M_ShortEvent> m_GetEvents(DateTime fromDate, DateTime toDate)
        {
            return MobileController.GetEvents(fromDate, toDate);
        }

        public M_Event m_GetEventObj(int eventID)
        {
            return MobileController.GetEvent(eventID);
        }

        public List<M_ShortProgram> m_GetEventProgramme(int dayID)
        {
            return MobileController.GetEventProgramme(dayID);
        }

        public M_Program m_GetProgramme(int progID)
        {
            return MobileController.GetProgram(progID);
        }

        public List<M_ShortGuest> m_GetEventGuests(int dayID)
        {
            return MobileController.GetEventGuests(dayID);
        }

        public M_Guest m_GetGuest(int guestID)
        {
            return MobileController.GetGuest(guestID);
        }

        public string m_Authenticate(string uid, string pwd)
        {
            return MobileController.Authenticate(uid, pwd);
        }

        public bool m_CanViewTask(string key, int eventID)
        {
            return MobileController.CanViewTask(MobileController.isValidKey(key).UserID, eventID);
        }

        public List<M_ShortTask> m_ViewMyTask(string key, int eventID)
        {
            try
            {
                return MobileController.ViewUserTask(MobileController.isValidKey(key).UserID, eventID);
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                     new FaultReason(ex.Message));
            }
        }

        public List<M_ShortTask> m_ViewEventTask(string key, int eventID)
        {
            try
            {
                return MobileController.ViewEventTask(MobileController.isValidKey(key).UserID, eventID);
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                     new FaultReason(ex.Message));
            }
        }

        public M_Task m_GetTask(string key, int taskID)
        {
            try
            {
                return MobileController.GetTask(MobileController.isValidKey(key).UserID, taskID);
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                     new FaultReason(ex.Message));
            }
        }



        #endregion

        #region "Request Management"

        public string CreateNewRequest(User user, int eventid, string targetEmail, string description, string url, string title)
        {
            return RequestController.CreateNewRequest(user, eventid, targetEmail, description, url, title);
        }

        public Request GetRequest(int requestID)
        {
            return RequestController.GetRequest(requestID);
        }

        public void EditRequest(User user, int requestID, string description, string url)
        {
            RequestController.UpdateRequest(user, requestID, description, url);
        }

        public List<Request> ViewRequests(Requestee requestee, DateTime fromDate, DateTime toDate, RequestStatus status, bool viewAllStatus)
        {
            return RequestController.ViewRequests(requestee, fromDate, toDate, status, viewAllStatus);
        }

        public void ChangeStatus(Requestee requestee, int requestID, RequestStatus status, string remark)
        {
            RequestController.ChangeStatus(requestee, requestID, status, remark);
        }

        public Requestee ValidateRequestee(string targetEmail, string OTP)
        {
            return RequesteeController.ValidateRequestee(targetEmail, OTP);
        }

        public string GetOtp(string targetEmail)
        {
            return RequesteeController.GetOtp(targetEmail);
        }



        public List<Request> ViewRequestViaRequester(int eventID, User user, DateTime fromDate, DateTime toDate,
                            RequestStatus status, bool viewAllStatus)
        {
            return RequestController.ViewRequestViaRequester(eventID, user, fromDate, toDate, status, viewAllStatus);
        }

        public void CancelRequest(User user, int requestID)
        {
            RequestController.CancelRequest(user, requestID);
        }
        #endregion

        #region 'Export'
        public ExportData GetExportData(User u, int eventID, Boolean NeedFacilities, Boolean NeedPrograms, Boolean NeedIncome, Boolean NeedOptItems, Boolean NeedTasks
        , Boolean NeedGuest, Boolean NeedParticipant)
        {
            return ExportController.GetExportData(u, eventID, NeedFacilities, NeedPrograms, NeedIncome, NeedOptItems, NeedTasks, NeedGuest, NeedParticipant);
        }
        #endregion
    }
}
