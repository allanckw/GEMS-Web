using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;

namespace GemsWeb
{
    public class Global : System.Web.HttpApplication
    {

        private void scheduler()
        {
            Timer mailTimer = new System.Timers.Timer();
            // Set the Interval to 5 seconds (5000 milliseconds).
            //mailTimer.Interval = 5000

            //Set the Interval to 24hours (86 400 000 milliseconds).
            mailTimer.Interval = 86400000;

            mailTimer.AutoReset = true;
            mailTimer.Elapsed += new ElapsedEventHandler(mailTimer_Elapsed);
            mailTimer.Enabled = true;
        }

        protected void mailTimer_Elapsed(object sender, EventArgs e)
        {
            string filepath = Server.MapPath("~") + "\\Logs\\lastsent.scd";
            System.IO.StreamReader strReader = new System.IO.StreamReader(filepath);
            string d = "" + strReader.ReadLine();
            strReader.Close();

            if (d.Length == 0)
            {
                DeleteUselessFolders();
            }
            else if (d.Length == 8)
            {
                DateTime dd = new DateTime(int.Parse(d.Substring(0,4)), int.Parse(d.Substring(4,2)), int.Parse(d.Substring(6,2)));
                if (dd.Date < DateTime.Now.Date)
                {
                    DeleteUselessFolders();
                }
            }
        }

        private void DeleteUselessFolders()
        {
            string wrkSpaceDir = Server.MapPath("~") + "WorkSpace\\";
            EventClient evClient = new EventClient();

            string[] subdirEntries = System.IO.Directory.GetDirectories(wrkSpaceDir);

            evClient.Close();
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
