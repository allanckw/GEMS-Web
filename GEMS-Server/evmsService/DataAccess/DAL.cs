using System.Data.Linq;
using evmsService.entities;
using System.Web.Hosting;
using System.Configuration;

namespace evmsService.DataAccess
{
    public class DAL : DataContext
    {
        #region "Version 0.1"

        public Table<User> users;
        public Table<SysRole> sRole;
        
        public Table<Notifications> notifications;

        public Table<FacilityAdmin> facAdmins;
        public Table<Facility> Facilities;
        public Table<FacilityBookingRequest> facBookReqs;
        public Table<FacilityBookingRequestDetails> facBookReqsDetails;
        public Table<FacilityBookingConfirmed> facConfirmedBookings;
        
        public Table<Events> events;
        public Table<Program> programs;
       
        public Table<Role> roles;
        public Table<Right> rights;
        public Table<Function> functions;
        public Table<RoleTemplate> roleTemplate;
        public Table<RightTemplate> rightTemplate;

        public Table<Guest> guests;

        public Table<Items> items;
        public Table<ItemTypes> itemTypes;
        #endregion

        #region "Version 0.2"

        public Table<OptimizedBudgetItems> optimizedBudgetItems;
        public Table<OptimizedBudgetItemsDetails> optimizedBudgetItemDetails;
        public Table<BudgetIncome> income;

        public Table<Task> tasks;
        public Table<TaskAssignment> taskAssignments;

        public Table<Participant> participants;
        public Table<Field> fields;
        public Table<StaticField> staticFields;
        public Table<FieldAnswer> fieldAnswer;
        public Table<Publish> publishs;

        public Table<Service> services;
        public Table<PointOfContact> pointOfContacts;
        public Table<Review> reviews;
        
        #endregion

        #region "Version 0.3"
        public Table<EventDay> days;
        public Table<ParticipantTransaction> partiTrans;
        
        public Table<Request> requests;
        public Table<Requestee> requestees;
        public Table<RequestLog> requestLogs;

        public Table<WorkspaceFolders> ArtefactWSFolders;
        public Table<WorkspaceFiles> ArtefactWSFiles;

        #endregion

        //private static string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        private static string dataDirectory = HostingEnvironment.MapPath("~/App_Data"); //Does the same thing as above but safer

        public DAL()
            : base(ConfigurationManager.ConnectionStrings["evms_connString"].ConnectionString.ToString().Replace("|DataDirectory|", dataDirectory))
        {
        }
    }
}