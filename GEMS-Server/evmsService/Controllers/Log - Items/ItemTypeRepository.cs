using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Hosting;
using System.Xml.Linq;

namespace evmsService.Controllers
{

    public class ItemTypeRepository
    {
        private static string dataDirectory = HostingEnvironment.MapPath("~/App_Data");
        private static string filePath = dataDirectory + "\\ItemType.xml";

        public static void AddItemType(string itemType)
        {
            if (isDuplicate(itemType))
                return;
            else
            {
                XDocument doc = XDocument.Load(filePath);  //load the xml file.
                IEnumerable<XElement> oMemberList = doc.Elements("ItemTypes").Descendants("itemtype");
                var newItemType = new XElement("itemtype", itemType.ToUpper());
                oMemberList.Last().AddAfterSelf(newItemType);  //Add node to the last element.
                doc.Save(filePath);
            }
        }

        public static void UpdateItemType(string type, string newValue)
        {
            newValue = newValue.ToUpper();
            if (isDuplicate(newValue))
                throw new FaultException<SException>(new SException(),
                  new FaultReason("Item type is a duplicate!"));

            XDocument doc = XDocument.Load(filePath); //replace with xml file path
            IEnumerable<XElement> oMemberList = doc.Elements("ItemTypes"); //get the member node.
            var oMember = (from member in oMemberList.Elements("itemtype")
                           where member.Value == type
                           select member).FirstOrDefault(); //replace memberId by querystring value.
            oMember.SetValue(newValue);
            doc.Save(filePath);
        }

        public static void deleteItemType(string itemType)
        {
            itemType = itemType.ToUpper();
            XElement element = LoadXML();
            if (element != null)
            {
                var xml = (from member in element.Descendants("itemtype")
                           where member.Value == itemType
                           select member).FirstOrDefault();
                if (xml != null)
                {
                    xml.Remove();
                    element.Save(filePath);
                }
            }
        }

        public static List<string> GetItemTypeList()
        {
            XElement element = LoadXML();
            if (element != null)
            {
                var query = from member in element.Descendants("itemtype")
                            orderby member.Value
                            select member.Value;

                if (query != null && query.Count() > 0)
                {
                    List<string> list = new List<string>();
                    foreach (var itemType in query)
                    {
                        list.Add(itemType.ToString());
                    }
                    return list;
                }

            }
            return null;
        }

        private static XElement LoadXML()
        {

            if (System.IO.File.Exists(filePath))
            {
                XElement xml = XElement.Load(filePath);
                return xml;
            }
            return null;
        }

        private static bool isDuplicate(string itemType)
        {
            XElement element = LoadXML();
            var xml = (from member in element.Descendants("itemtype")
                       where member.Value.ToUpper() == itemType.ToUpper()
                       select member).FirstOrDefault();

            if (xml != null)
                return true;
            else
                return false;
        }


    }
}