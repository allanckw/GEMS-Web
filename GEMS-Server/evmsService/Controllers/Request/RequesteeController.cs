using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;
using System.Transactions;

namespace evmsService.Controllers
{
    public class RequesteeController
    {
        public static Requestee ValidateRequestee(string targetEmail, string OTP)
        {
            Requestee existingRequestee;
            DAL dalDataContext = new DAL();
            try
            {

                Table<Requestee> requestees = dalDataContext.requestees;


                existingRequestee = (from requestee in dalDataContext.requestees
                                               where requestee.TargetEmail.ToLower() == targetEmail.ToLower()
                                               select requestee).SingleOrDefault<Requestee>();

            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                  new FaultReason("An Error occured While Validating login: " + ex.Message));
            }

            if (existingRequestee == null)
            {
                throw new FaultException<SException>(new SException(),
              new FaultReason("Error, No such email"));
            }
            else
            {
                if (!existingRequestee.Otp.Equals(OTP))
                {
                    throw new FaultException<SException>(new SException(),
                          new FaultReason("Error, Wrong Password"));
                }
                return existingRequestee;
            }
        }

        public static string GetOtp(string targetEmail)
        {
            DAL dalDataContext = new DAL();
                try
                {

                    Table<Requestee> requestees = dalDataContext.requestees;
                    

                    Requestee existingRequestee = (from requestee in dalDataContext.requestees
                                                   where requestee.TargetEmail.ToLower() == targetEmail.ToLower()
                                                    select requestee).SingleOrDefault<Requestee>();

                    if (existingRequestee == null)
                    {
                        throw new FaultException<SException>(new SException(),
                      new FaultReason("The email is not registered in the system"));
                    }
                    else
                    {
                        return existingRequestee.Otp;
                    }

                }
                catch (Exception ex)
                {
                    throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While Getting OTP New Requestee: " + ex.Message));
                }
        }

        public static string CreateNewRequestee(string targetEmail, DAL dalDataContext)//if existing then return the existing otp
        {
            
                try
                {

                    Table<Requestee> requestees = dalDataContext.requestees;

                    Requestee existingRequestee = (from requestee in dalDataContext.requestees
                                                   where requestee.TargetEmail.ToLower() == targetEmail.ToLower()
                                                    select requestee).SingleOrDefault<Requestee>();

                    if (existingRequestee == null)
                    {
                        string otp = OTPGenerator.Generate();

                        Requestee newRequestee = new Requestee(targetEmail,otp);
                        dalDataContext.requestees.InsertOnSubmit(newRequestee);
                        dalDataContext.SubmitChanges();
                        return otp;
                    }
                    else
                    {
                        return existingRequestee.Otp;
                    }

                }
                catch (Exception ex)
                {
                    throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While Creating New Requestee: " + ex.Message));
                }
            }

        public static string CreateNewRequestee(string targetEmail)//if existing then return the existing otp
        {
            
            DAL dalDataContext = new DAL();
            try
            {

                Table<Requestee> requestees = dalDataContext.requestees;

                Requestee existingRequestee = (from requestee in dalDataContext.requestees
                                               where requestee.TargetEmail.ToLower() == targetEmail.ToLower()
                                                select requestee).SingleOrDefault<Requestee>();

                if (existingRequestee == null)
                {
                    string otp = OTPGenerator.Generate();

                    Requestee newRequestee = new Requestee(targetEmail,otp);
                    dalDataContext.requestees.DeleteOnSubmit(newRequestee);
                    dalDataContext.SubmitChanges();
                    return otp;
                }
                else
                {
                    return existingRequestee.Otp;
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                    new FaultReason("An Error occured While Creating New Requestee: " + ex.Message));
            }
        }
    }
}
