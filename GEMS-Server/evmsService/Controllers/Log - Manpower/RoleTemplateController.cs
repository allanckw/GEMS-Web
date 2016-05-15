using System;
using System.Collections.Generic;
using System.Linq;
using evmsService.entities;
using evmsService.DataAccess;
using System.Data.Linq;
using System.ServiceModel;

namespace evmsService.Controllers
{
    public class RoleTemplateController
    {
        public static RoleTemplate AddRoleTemplate(User user, int EventID, string RoleTemplatePost, string RoleTemplateDescription)
        {
            try
            {
                DAL dalDataContext = new DAL();
                Table<RoleTemplate> roles = dalDataContext.roleTemplate;

                RoleTemplate creatingRole = new RoleTemplate(RoleTemplatePost, RoleTemplateDescription, EventController.GetEvent(EventID));

                roles.InsertOnSubmit(creatingRole);

                roles.Context.SubmitChanges();

                return creatingRole;

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role Template, Please Try Again!"));
            }
        }

        public static RoleTemplate GetRoleTemplate(int RoleTemplateID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                RoleTemplate existingRole = (from roles in dalDataContext.roleTemplate
                                             where roles.RoleTemplateID == RoleTemplateID
                                             select roles).FirstOrDefault();

                if (existingRole == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Role Template"));
                }
                return existingRole;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Role Template Data, Please Try Again!"));
            }
        }

        public static RoleTemplate AddRoleTemplate(Events evnt, string RoleTemplatePost, string RoleTemplateDescription, DAL dalDataContext)
        {

            try
            {

                Table<RoleTemplate> roles = dalDataContext.roleTemplate;
                RoleTemplate creatingRole;
                //if(e == null)
                creatingRole = new RoleTemplate(RoleTemplatePost, RoleTemplateDescription, evnt);
                //else
                //    creatingRole = new RoleTemplate(RoleTemplatePost, RoleTemplateDescription, evnt.EventID);

                roles.InsertOnSubmit(creatingRole);

                roles.Context.SubmitChanges();

                return creatingRole;

            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role Template, Please Try Again!"));
            }
        }

        public static RoleTemplate AddRoleTemplate(User user, RoleTemplate r)
        {

            //chk if user can do this anot

            try
            {
                DAL dalDataContext = new DAL();
                Table<RoleTemplate> roles = dalDataContext.roleTemplate;
                roles.InsertOnSubmit(r);
                roles.Context.SubmitChanges();
                return r;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Role Template, Please Try Again!"));
            }
        }

        public static void DeleteRoleTemplate(User user, int RoleTemplateID)
        {

            DAL dalDataContext = new DAL();

            try
            {
                RoleTemplate matchedrole = (from roles in dalDataContext.roleTemplate
                                            where roles.RoleTemplateID == RoleTemplateID
                                            select roles).FirstOrDefault();

                dalDataContext.roleTemplate.DeleteOnSubmit(matchedrole);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Role Template, Please Try Again!"));
            }
        }

        public static void EditRoleTemplate(int RoleTemplateID, string RoleTemplatePost, string RoleTemplateDescription, DAL dalDataContext)
        {
            try
            {
                RoleTemplate matchedroleTemplate = (from roles in dalDataContext.roleTemplate
                                            where roles.RoleTemplateID == RoleTemplateID
                                            select roles).FirstOrDefault();
                if (matchedroleTemplate == null)
                {

                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Role Template"));
                }
                else
                {
                    matchedroleTemplate.Description = RoleTemplateDescription;
                    matchedroleTemplate.Post = RoleTemplatePost;


                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Role Template, Please Try Again!"));
            }
        }

        public static void EditRoleTemplate(int RoleTemplateID, User user, Events evnt, string RoleTemplatePost, string RoleTemplateDescription)
        {
            if (!user.isAuthorized( evnt, EnumFunctions.Add_Role))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Edit This Role Template!"));

            try
            {
                DAL dalDataContext = new DAL();

                RoleTemplate matchedrole;

                if (evnt != null)
                {
                    matchedrole = (from roles in dalDataContext.roleTemplate
                                   where roles.RoleTemplateID == RoleTemplateID
                                   select roles).FirstOrDefault();
                }
                else
                {
                    matchedrole = (from roles in dalDataContext.roleTemplate
                                   where roles.RoleTemplateID == null
                                   select roles).FirstOrDefault();
                }


                if (matchedrole == null)
                {

                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid Role Template"));
                }
                else
                {
                    matchedrole.Description = RoleTemplateDescription;
                    matchedrole.Post = RoleTemplatePost;


                    dalDataContext.SubmitChanges();
                }
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Editing Role Template, Please Try Again!"));
            }
        }
       
        public static List<RoleTemplate> ViewRoleTemplates(User user, Events evnt)
        {
            DAL dalDataContext = new DAL();

            List<RoleTemplate> Roles;
            if (evnt  != null)
            {
                Roles = (from roles in dalDataContext.roleTemplate
                         where roles.EventID == evnt.EventID
                         select roles).ToList<RoleTemplate>();
            }
            else
            {
                Roles = (from roles in dalDataContext.roleTemplate
                         where roles.EventID == null
                         select roles).ToList<RoleTemplate>();
            }

            return Roles;
        }

    }
}