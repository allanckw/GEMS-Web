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
    public class RequestLogController
    {
        public static void InsertRequestLog(Request r)//if existing then return the existing otp
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<RequestLog> requestLogs = dalDataContext.requestLogs;

                RequestLog newRequestLog = new RequestLog(r.RequestID, r.Description, r.Remark, r.Status, r.URL);
                dalDataContext.requestLogs.InsertOnSubmit(newRequestLog);
                dalDataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                  new FaultReason("An Error occured While Creating New Requestee: " + ex.Message));
            }
        }

        public static List<RequestLog> ViewRequestLog(int RequestID)
        {
            //RequesteeController.ValidateRequestee(targetEmail, otp);

            DAL dalDataContext = new DAL();

            try
            {
                List<RequestLog> RequestLogs = (from requestLogs in dalDataContext.requestLogs
                                                join requests in dalDataContext.requests
                                                on requestLogs.RequestID equals requests.RequestID
                                                where requestLogs.RequestID.Equals(RequestID)
                                                select requestLogs).OrderByDescending(r => r.LogDateTime).ToList<RequestLog>();

                return RequestLogs;
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                      new FaultReason("An Error occured While Viewing RequestLogs"));
            }
        }

    }
}
