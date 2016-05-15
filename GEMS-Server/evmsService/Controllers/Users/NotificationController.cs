using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class NotificationController
    {
        public static void sendNotification(string sender, string receiver, string title, string msg)
        {
            DAL dalDataContext = new DAL();
            Table<Notifications> noti = dalDataContext.notifications;

            Notifications newMsg = new Notifications(sender, receiver, title, msg);
            noti.InsertOnSubmit(newMsg);
            noti.Context.SubmitChanges();
        }

        public static Notifications retrieveNotification(User user, string sid, string rid, DateTime sent)
        {
            if (String.Compare(user.UserID, rid, true) != 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, You cannot read message not in your inbox"));

            DAL dalDataContext = new DAL();

            Notifications msg = (from notifs in dalDataContext.notifications
                               where notifs.Sender == sid &&
                               notifs.Receiver == user.UserID &&
                               notifs.SendDateTime == sent
                               orderby notifs.SendDateTime
                               select notifs).FirstOrDefault();

            return msg;

        }

        public static List<Notifications> ViewAllNotifications(User user, string rid, bool isAllMsg)
        {
            if (String.Compare(user.UserID, rid, true) != 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, You cannot read message not in your inbox"));

            DAL dalDataContext = new DAL();

            List<Notifications> nList;
            if (isAllMsg)
            {
                //Retrieve All msgs
                nList = (from notifs in dalDataContext.notifications
                         where notifs.Receiver == user.UserID
                         orderby notifs.SendDateTime descending
                         select notifs).ToList();
            }
            else
            {
                //Retrieve unread msg
                nList = (from notifs in dalDataContext.notifications
                         where notifs.Receiver == rid && notifs.isRead == false
                         orderby notifs.SendDateTime descending
                         select notifs).ToList();
            }


            return nList;

        }

        public static void setToRead(User user, Notifications msg)
        {
            if (String.Compare(user.UserID, msg.Receiver, true) != 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, You cannot read messages not in your inbox"));

            DAL dalDataContext = new DAL();
            try
            {
                Notifications n1 = (from notifs in dalDataContext.notifications
                                    where notifs.Sender == msg.Sender &&
                                    notifs.Receiver == user.UserID &&
                                    notifs.SendDateTime == msg.SendDateTime
                                    select notifs).SingleOrDefault();
                n1.isRead = true;
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

        public static string GetLastNotification(User user, string rid)
        {
            if (String.Compare(user.UserID, rid, true) != 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, You cannot read messages not in your inbox"));

            DAL dalDataContext = new DAL();
            Notifications msg = (from notifs in dalDataContext.notifications
                               where notifs.Receiver == user.UserID
                               orderby notifs.SendDateTime descending
                               select notifs).FirstOrDefault<Notifications>();

            if (msg != null)
            {
                TimeSpan t = DateTime.Now - msg.SendDateTime;
                if (t > TimeSpan.FromTicks(0) && t <= TimeSpan.FromSeconds(15))
                {
                    return msg.Sender;
                }
                else
                {
                    return "";
                }
            }
            else
                return "";
        }


        public static void deleteNotification(User user, Notifications msg)
        {
            if (String.Compare(user.UserID, msg.Receiver, true) != 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, You cannot delete message not in your inbox"));

            DAL dalDataContext = new DAL();
            try
            {
                Notifications matchingNotif = (from notif in dalDataContext.notifications
                                               where notif.Sender == msg.Sender &&
                                               notif.Receiver == user.UserID &&
                                               notif.SendDateTime == msg.SendDateTime
                                               select notif).Single<Notifications>();


                dalDataContext.notifications.DeleteOnSubmit(matchingNotif);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

        public static void deleteAllNotifications(User user, string uid)
        {
            if (String.Compare(user.UserID, uid, true) != 0)
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, You cannot delete message not in your inbox"));

            try
            {
                DAL dalDataContext = new DAL();
                List<Notifications> matchingNotif = (from notif in dalDataContext.notifications
                                                     where notif.Receiver == user.UserID
                                                     select notif).ToList<Notifications>();


                dalDataContext.notifications.DeleteAllOnSubmit(matchingNotif);
                dalDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason(ex.Message));
            }
        }

    }
}