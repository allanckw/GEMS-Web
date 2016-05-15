using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;


namespace evmsService.Controllers
{
    public class BudgetIncomeController
    {
        public static int AddBudgetIncome(User user, int EventID, BudgetIncome bIncome)
        {
            if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Income))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Add Income!"));
            try
            {
                DAL dalDataContext = new DAL();
                Table<BudgetIncome> income = dalDataContext.income;

                

                income.InsertOnSubmit(bIncome);
                income.Context.SubmitChanges();

                return bIncome.IncomeID;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Income, Please Try Again!"));
            }
        }

        public static BudgetIncome GetBudgetIncome(int BudgetIncomeID, int eventID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                BudgetIncome existingBudgetIncome = (from income in dalDataContext.income
                                                     where income.IncomeID == BudgetIncomeID &&
                                                    income.EventID == eventID
                                                     select income).FirstOrDefault();

                if (existingBudgetIncome == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Income Item"));
                }
                return existingBudgetIncome;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Income data, Please Try Again!"));
            }
        }

        public static void DeleteBudgetIncome(User user, int BudgetIncomeID, int eventID)
        {
            if (!user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Manage_Income))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Income!"));

            DAL dalDataContext = new DAL();

            try
            {
                BudgetIncome matchedincome = (from income in dalDataContext.income
                                              where income.IncomeID == BudgetIncomeID &&
                                               income.EventID == eventID
                                              select income).FirstOrDefault();


                dalDataContext.income.DeleteOnSubmit(matchedincome);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Income, Please Try Again!"));
            }
        }
        public static void EditBudgetIncome(User user, int EventID, int IncomeID, BudgetIncome bIncome)
        {

            if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Income))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit Income!"));

            if (IncomeID == -1)
                throw new FaultException<SException>(new SException(),
                    new FaultReason("Cannot edit partipants payment data!"));

            try
            {
                DAL dalDataContext = new DAL();

                BudgetIncome matchedincome = (from income in dalDataContext.income
                                              where income.IncomeID == IncomeID &&
                                              income.EventID == EventID
                                              select income).FirstOrDefault();

                if (matchedincome == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Income Item"));
                }
                else
                {
                    matchedincome.Name = bIncome.Name;
                    matchedincome.Description = bIncome.Description;
                    matchedincome.IsGstLiable = bIncome.IsGstLiable;
                    matchedincome.IncomeBeforeGST = bIncome.IncomeBeforeGST;
                    matchedincome.GstValue = bIncome.GstValue;
                    matchedincome.Source = bIncome.Source;
                    matchedincome.DateReceived = bIncome.DateReceived;
                    dalDataContext.SubmitChanges();

                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Income, Please Try Again!"));
            }
        }
        public static List<BudgetIncome> ViewBudgetIncome(User user, int eventID)
        {
            if (!user.isAuthorized(EventController.GetEvent(eventID), EnumFunctions.Manage_Income))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To View income!"));
            try
            {
                DAL dalDataContext = new DAL();

                List<BudgetIncome> BudgetIncomes = (from income in dalDataContext.income
                                                    where income.EventID == eventID
                                                    select income).ToList<BudgetIncome>();

                //Todo, add a new budgetIncome item to the list for participant payments
                decimal participantsIncome = ParticipantController.GetEventParticipantIncome(eventID);

                if (participantsIncome > 0)
                {
                    BudgetIncome partiIncome = new BudgetIncome(eventID, "Participants Registration Fees", 
                                                                "Income that came from participants Income",
                                                                false, participantsIncome, 0, "Participants Registration", 
                                                                DateTime.Now, -1);

                    BudgetIncomes.Add(partiIncome);
                }


                return BudgetIncomes;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured Retrieving Income data, Please Try Again!"));
            }
        }
    }
}