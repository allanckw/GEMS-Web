using System;
using System.Timers;
using System.IO;

namespace GemsWeb
{
    public class Global : System.Web.HttpApplication
    {

        private void scheduler()
        {
            Timer deleteTimer = new System.Timers.Timer();
            // Set the Interval to 5 seconds (5000 milliseconds).
            //mailTimer.Interval = 5000

            //Set the Interval to 24hours (86 400 000 milliseconds).
            deleteTimer.Interval = 86400000;

            deleteTimer.AutoReset = true;
            deleteTimer.Elapsed += new ElapsedEventHandler(DeleteTimer_Elapsed);
            deleteTimer.Enabled = true;
        }

        protected void DeleteTimer_Elapsed(object sender, EventArgs e)
        {
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\Logs\\lastsent.scd";
            System.IO.StreamReader strReader = new System.IO.StreamReader(filepath);
            string d = "" + strReader.ReadLine();
            strReader.Close();

            if (d.Length == 0)
            {
                removeUnwantedFolders(filepath);
            }
            else if (d.Length == 8)
            {
                DateTime dd = new DateTime(int.Parse(d.Substring(0, 4)), int.Parse(d.Substring(4, 2)), int.Parse(d.Substring(6, 2)));
                if (dd.Date < DateTime.Now.Date)
                {
                    removeUnwantedFolders(filepath);
                }
            }
        }

        private void removeUnwantedFolders(string logFile)
        {
     
            string wrkSpaceDir = System.Web.Hosting.HostingEnvironment.MapPath("~") + "WorkSpace\\";
            EventClient evClient = new EventClient();

            string[] subdirs = System.IO.Directory.GetDirectories(wrkSpaceDir);

            foreach (string dir in subdirs)
            {
                int evFolder;
                string dirToCheck = dir.Substring(dir.LastIndexOf('\\')).Remove(0, 1);
                bool parse = int.TryParse(dirToCheck, out evFolder);


                if (!evClient.isEventExist(evFolder) || !parse)
                {
                    emptyFolder(dir);
                    System.IO.Directory.Delete(dir);
                }
            }

            evClient.Close();

            System.IO.StreamWriter strWriter = new System.IO.StreamWriter(logFile, false);
            strWriter.WriteLine(DateTime.Now.ToString("yyyyMMdd"));
            strWriter.Close();
        }

        private void emptyFolder(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            foreach (FileInfo fi in dirInfo.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }

            foreach (DirectoryInfo di in dirInfo.GetDirectories())
            {
                emptyFolder(di.FullName);
                di.Delete();
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            scheduler();
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
