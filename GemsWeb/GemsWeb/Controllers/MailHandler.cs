using System;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace GemsWeb.Controllers
{


    //Sends an email..
    //Remember to unlock vendor data after implementation, require discussion with koh
    public class MailHandler
    {

        public MailHandler()
        {
        }

        private const string smtpServer = "smtp.gmail.com";
        private const string smtpUserName = "nus.gems@gmail.com";
        private const string smtpPwd = "2l2in2b3";
        private const int port = 587;

        public static string url;

        public static void sendVerifyMail(string pwd, string email)
        {
            StringBuilder sb = new StringBuilder();
            string FromEmail = "no-reply@gems.nus.edu.sg";

            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(email);
            mailMsg.From = new System.Net.Mail.MailAddress(FromEmail, "No-Reply (NUS GEMS)");

            mailMsg.Subject = "NUS GEMS Participant Password";
            mailMsg.IsBodyHtml = true;

            sb.AppendLine();
            sb.AppendLine("Email From NUS GEMS");
            sb.AppendLine();
            sb.AppendLine("Thank you for registering with NUS GEMS");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Please click on the following link: ");

            sb.AppendLine("<a href='" + url + "/Login.aspx?mode=0'> <b> Login Here </b> </a>");
            sb.AppendLine();
            sb.AppendLine("Login using your e-mail and the password below to view and make payments to your registered events");
            sb.AppendLine();
            sb.AppendLine("Your Password is " + pwd);
            sb.AppendLine();
            sb.AppendLine("Please note that your password cannot be changed.");
            sb.AppendLine();

            sb.AppendLine("Regards");
            sb.AppendLine();
            sb.AppendLine("NUS GEMS Server Administrator");
            sb.AppendLine();
            sb.AppendLine("This e-mail is an auto-responder, Please do not reply to this e-mail");
            sb.AppendLine();
            sb.AppendLine("We thank you for your co-operation.");

            mailMsg.Body = sb.ToString().Replace(Environment.NewLine, "<br> ");

            try
            {
                NetworkCredential basicAuthenticationInfo = new NetworkCredential(smtpUserName, smtpPwd);
                SmtpClient MailObj = new SmtpClient(smtpServer, port);
                MailObj.Credentials = basicAuthenticationInfo;
                MailObj.Send(mailMsg);

            }
            catch (Exception ex)
            {

            }
        }

        public static void sendPaymentReceivedMail(string targetMail, string tx, decimal payment)
        {
            StringBuilder sb = new StringBuilder();
            string FromEmail = "no-reply@gems.nus.edu.sg";

            MailMessage mailMsg = new MailMessage();

            mailMsg.To.Add(targetMail);
            mailMsg.From = new MailAddress(FromEmail, "No-Reply (NUS GEMS)");

            mailMsg.Subject = "Payment Received Notification";
            mailMsg.IsBodyHtml = true;

            sb.AppendLine();
            sb.AppendLine("Email From NUS GEMS");
            sb.AppendLine();
            sb.AppendLine("Thank you for shopping with NUS GEMS");
            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("We have received your payment of SGD $" + payment.ToString("0.00"));
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Your Transaction number is " + tx);
            sb.AppendLine();
            sb.AppendLine();


            sb.AppendLine("Regards");
            sb.AppendLine();
            sb.AppendLine("NUS GEMS Server Administrator");
            sb.AppendLine();
            sb.AppendLine("This e-mail is an auto-responder, Please do not reply to this e-mail");
            sb.AppendLine();
            sb.AppendLine("We thank you for your co-operation.");

            mailMsg.Body = sb.ToString().Replace(Environment.NewLine, "<br> ");

            try
            {
                NetworkCredential basicAuthenticationInfo = new NetworkCredential(smtpUserName, smtpPwd);
                SmtpClient MailObj = new SmtpClient(smtpServer, port);
                MailObj.Credentials = basicAuthenticationInfo;
                MailObj.Send(mailMsg);

            }
            catch (Exception ex)
            {

            }
        }

        public static void sendReportMail(string senderName, int eventID, string title, string targetEmail, string url, string pwd)
        {
            EventClient evClient = new EventClient();
            string eventName = evClient.GetEventName(eventID);
            evClient.Close();

            StringBuilder sb = new StringBuilder();
            string FromEmail = "no-reply@gems.nus.edu.sg";

            MailMessage mailMsg = new MailMessage();

            mailMsg.To.Add(targetEmail);

            mailMsg.From = new System.Net.Mail.MailAddress(FromEmail, "No-Reply (NUS GEMS)");
            mailMsg.Subject = "New Request From NUS GEMS (General Events Management System)";
            mailMsg.IsBodyHtml = true;

            sb.AppendLine();
            sb.AppendLine("To whom it may concern,");

            sb.AppendLine(senderName + " from National University of Singapore has sent you a request regarding " + title);
            sb.AppendLine();
            sb.AppendLine("For more details about the request, please login at our website <a href='" + url + "'> here </a>");

            sb.AppendLine();
            sb.AppendLine("Your userid is " + targetEmail + " and your password is " + pwd);

            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("Regards");
            sb.AppendLine();
            sb.AppendLine("NUS GEMS Server Administrator");
            sb.AppendLine();
            sb.AppendLine("This e-mail is an auto-responder, Please do not reply to this e-mail");
            sb.AppendLine();
            sb.AppendLine("We thank you for your co-operation.");

            mailMsg.Body = sb.ToString().Replace(Environment.NewLine, "<br>");

            try
            {
                NetworkCredential basicAuthenticationInfo = new NetworkCredential(smtpUserName, smtpPwd);
                SmtpClient MailObj = new SmtpClient(smtpServer, port);
                MailObj.Credentials = basicAuthenticationInfo;
                MailObj.Send(mailMsg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void sendContactEmail(string email, string name, string message, string nature)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(smtpUserName);
            mailMsg.From = new MailAddress(email, name);

            mailMsg.Subject = "NUS GEMS Enquiry - " + nature;
            mailMsg.IsBodyHtml = false;

            mailMsg.Body += Environment.NewLine + "Message from " + name;
            mailMsg.Body += Environment.NewLine + "E-mail: " + email + Environment.NewLine + Environment.NewLine;
            mailMsg.Body += "Message: " + Environment.NewLine;
            mailMsg.Body += message;


            NetworkCredential basicAuthenticationInfo = new NetworkCredential(smtpUserName, smtpPwd);
            SmtpClient MailObj = new SmtpClient(smtpServer, port);
            MailObj.Credentials = basicAuthenticationInfo;
            MailObj.Send(mailMsg);

        }

    }

}