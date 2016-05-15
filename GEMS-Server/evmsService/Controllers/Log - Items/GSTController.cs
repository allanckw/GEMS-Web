using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Hosting;
using System.Xml.Linq;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class GSTController
    {
        private static string dataDirectory = HostingEnvironment.MapPath("~/App_Data");
        private static string filePath = dataDirectory + "\\GST.xml";

        private static XElement LoadXML()
        {

            if (System.IO.File.Exists(filePath))
            {
                XElement xml = XElement.Load(filePath);
                return xml;
            }
            return null;
        }

        public static void UpdateGST(User user, int newValue)
        {
            if (user.isSystemAdmin)
            {
                XElement element = LoadXML();
                var gst = (from member in element.Elements("value")
                           select member).FirstOrDefault();

                gst.SetValue(newValue);

                element.Save(filePath);
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To set GST!"));
            }
        }

        public static int GetGST()
        {

            XElement element = LoadXML();
            var gst = (from member in element.Elements("value")
                       select member).FirstOrDefault();

            return int.Parse(gst.Value);
        }
    }
}