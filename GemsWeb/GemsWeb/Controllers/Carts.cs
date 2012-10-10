using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Text;
using System.Web;

namespace GemsWeb.Controllers
{
    public class Carts
    {
        static DataSet cart = new DataSet();

        /// <summary>
        /// Loads the list of goods in a cart with a definite cart_id from an XML file into DataSet
        /// </summary>
        /// <param name="cart_id">the cart idetntifier</param>
        /// <returns>The DataSet with the list of goods in the cart with the cart_id.</returns>
        public static DataSet LoadCart(string cart_id)
        {

            DataSet currentCart = new DataSet();
            string cartsFile = HttpContext.Current.Server.MapPath("~/App_Data/Carts.xml");
            cart.Clear();

            if (File.Exists(cartsFile))
            {
                cart.ReadXml(cartsFile);
            }
            else
            {
                Carts.CreateXml(cartsFile, "Carts");
                cart.ReadXml(cartsFile);
            }


            // creates a copy of all carts
            currentCart = cart.Copy();
            string expression = null;
            DataRow[] foundRows = null;

            //an expression for searching for a cart with a cart_id
            expression = "cart_id NOT LIKE '" + cart_id + "'";
            try
            {
                if (currentCart.Tables.Count == 0)
                {
                   // WriteFile("Error in Carts.LoadCart(): " + "currentCart.Tables.Count = 0");
                    Alert.Show("You got no item in your cart", true, "ParticipantEvents.aspx");
                }

                if (currentCart.Tables[0].Rows.Count == 0)
                {
                   // WriteFile("Error in Carts.LoadCart(): " + "currentCart.Rows.Count = 0");
                    Alert.Show("You got no item in your cart", true, "ParticipantEvents.aspx");
                }

                // remove all goods from the current cart with the cart_id that does not ocincide with the specified one
                foundRows = currentCart.Tables[0].Select(expression);
                int i = 0;
                for (i = 0; i <= foundRows.GetUpperBound(0); i++)
                {
                    foundRows[i].Delete();
                }

                // getting a name of item for its Event_ID in order to display it further in GridView
                DataColumn column = currentCart.Tables[0].Columns["Event_ID"];
                currentCart.Tables[0].Columns.Add("Name", System.Type.GetType("System.String"));
                foreach (DataRow row in currentCart.Tables[0].Rows)
                {
                    //TODO: Get the event name via its ID 
                    EventClient client = new EventClient();

                    string name = client.GetEventName(int.Parse(row["Event_ID"].ToString()));

                    if (!string.IsNullOrEmpty(name))
                    {
                        row["Name"] = name;
                    }
                }

                return currentCart;
            }
            catch (Exception ex)
            {
                currentCart.Clear();
                //Alert.Show("Error in Carts.LoadCart(): " + ex.Message);
                //WriteFile("Error in Carts.LoadCart(): " + ex.Message);
                //HttpContext.Current.Response.Redirect("~/Default.aspx")
                return null;
            }
        }


        /// <summary>
        /// removing an item with rec_id from cart
        /// </summary>
        /// <param name="rec_id">the record identifier</param>
        public static void Delete(int rec_id)
        {
            try
            {
                string xmlFile = HttpContext.Current.Server.MapPath("~/App_Data/Carts.xml");

                //An expression for searching for an item for its record identifier in the Carts.xml file
                string expression = null;
                expression = "rec_id = '" + rec_id.ToString() + "'";

                DataRow[] tempRow = cart.Tables[0].Select(expression);
                if (tempRow.Length == 1)
                {
                    tempRow[0].Delete();
                    cart.WriteXml(xmlFile);
                }
            }
            catch (Exception ex)
            {
                Alert.Show("Error in Carts.Delete(): " + ex.Message);
                //WriteFile("Error in Carts.Delete(): " + ex.Message)
                //HttpContext.Current.Response.Redirect("~/Default.aspx")
            }
        }

        /// <summary>
        /// removing an item with rec_id from cart
        /// </summary>
        /// <param name="userid">the record identifier</param>
        public static void DeleteUserCart(string userid)
        {
            try
            {
                string xmlFile = HttpContext.Current.Server.MapPath("~/App_Data/Carts.xml");

                //An expression for searching for an item for its record identifier in the Carts.xml file
                string expression = null;
                expression = "cart_id = '" + userid.ToString() + "'";

                DataRow[] tempRow = cart.Tables[0].Select(expression);
                if (tempRow.Length == 1)
                {
                    tempRow[0].Delete();
                    cart.WriteXml(xmlFile);
                }
            }
            catch (Exception ex)
            {
                Alert.Show("Error in Carts.Delete(): " + ex.Message);
                //WriteFile("Error in Carts.Delete(): " + ex.Message);
                //HttpContext.Current.Response.Redirect("~/Default.aspx")
            }
        }

        /// <summary>        
        /// crating different XML files using a file name and a root element
        /// </summary>
        /// <param name="xmlFile">the file name</param>
        /// <param name="element">a name of the root element</param>

        public static void CreateXml(string xmlFile, string element)
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            string xmlData = "<" + element + "></" + element + ">";

            doc.Load(new StringReader(xmlData));

            try
            {
                doc.Save(xmlFile);
            }
            catch (Exception ex)
            {
                //WriteFile("Error in Carts.CreateXml(): " + ex.Message);
            }

        }

        /// <summary>        
        /// getting a unique identification number
        /// </summary>
        /// <param name="nodes">a list of existing nodes</param>
        /// <param name="columnName">a name of the column where the unique identifier is searched</param>
        /// <returns>the unique record identifier for the specified column</returns>
        /// <remarks></remarks>
        public static int GetIdentity(XmlNodeList nodes, string columnName)
        {
            try
            {
                int max_rec = 0;
                foreach (XmlNode node in nodes)
                {
                    int currentRec = Convert.ToInt32(node.Attributes[columnName].InnerText);
                    if ((currentRec > max_rec))
                    {
                        max_rec = currentRec;
                    }
                }
                return max_rec + 1;
            }
            catch (Exception ex)
            {
                //WriteFile("Error in Carts.GetIdentity(): " + ex.Message);
                return 0;
            }

        }

        public static bool isDuplicateEvent(XmlNodeList nodes, string evntID, string cartID)
        {
            int eventID = int.Parse(evntID);
            bool found = false;
            foreach (XmlNode node in nodes)
            {
                int currentRec = int.Parse(node.Attributes["Event_ID"].InnerText);
                string cID = node.Attributes["cart_id"].InnerText;

                if (currentRec == eventID && string.Compare(cID, cartID, true) == 0)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        /// <summary>
        /// creating a Log file
        /// </summary>
        /// <param name="sErrMsg">a text to be written in the Log file</param>
        //public static void WriteFile(string sErrMsg)
        //{
        //    CultureInfo ci = new CultureInfo("en-us");
        //    StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Logs\\" + customDatetoString(DateTime.Today) + ".log", true, Encoding.ASCII);
        //    sw.Write(DateTime.Now.ToString(ci));
        //    sw.Write(": ");
        //    sw.Write(sErrMsg);
        //    sw.Write(Environment.NewLine);
        //    sw.Close();
        //}

        public static string customDatetoString(System.DateTime d)
        {
            string dStr = d.Year.ToString();

            if (d.Month < 10)
            {
                dStr += "0";
            }

            dStr += d.Month;

            if (d.Day < 10)
            {
                dStr += "0";
            }

            dStr += d.Day;

            return dStr;
        }
    }

}