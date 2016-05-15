using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class ArtefactWSController
    {
        public static void CreateFolders(User user, int eventID, string folderName, string folderDesc, string remarks)
        {
            Events evnt = EventController.GetEvent(eventID);

            if (!RoleLogicController.HaveRights(evnt, user, EnumFunctions.Manage_Artefacts)) //user.isAuthorized(evnt, ManageArtefact) after enum f(x) up
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User does not have rights to Add Folder!"));

            if (GetWorkSpaceFolder(user, eventID, folderName) == null)
            {
                WorkspaceFolders newWorkspaceFolder = new WorkspaceFolders(user.UserID, eventID, folderName, folderDesc, remarks);

                DAL dalDataContext = new DAL();
                Table<WorkspaceFolders> wsfTable = dalDataContext.ArtefactWSFolders;
                wsfTable.InsertOnSubmit(newWorkspaceFolder);
                wsfTable.Context.SubmitChanges();
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Workspace Folder already exist!"));
            }
        }

        public static void DeleteFolder(User user, int eventID, string folderName)
        {
            Events evnt = EventController.GetEvent(eventID);

            //TODO: Add Enum CreateArtefactFolder in functions - Nick
            if (!RoleLogicController.HaveRights(evnt, user, EnumFunctions.Manage_Artefacts)) //user.isAuthorized(evnt, ManageArtefact) after enum f(x) up
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User does not have rights to Delete Folder!"));

            DAL dalDataContext = new DAL();

            try
            {
                WorkspaceFolders matchedWS = (from ws in dalDataContext.ArtefactWSFolders
                                              where ws.EventID == eventID &&
                                              ws.FolderName.ToLower() == folderName.ToLower()
                                              select ws).FirstOrDefault();

                if (matchedWS != null)
                {
                    dalDataContext.ArtefactWSFolders.DeleteOnSubmit(matchedWS);
                    dalDataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Deleting Workspace, Please Try Again!"));
            }
        }

        public static void UpdateFolder(User user, int eventID, string folderName, string folderDesc, string remarks)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (!RoleLogicController.HaveRights(evnt, user, EnumFunctions.Manage_Artefacts)) //user.isAuthorized(evnt, ManageArtefact) after enum f(x) up
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User does not have rights to Update Folder!"));

            DAL dalDataContext = new DAL();

            WorkspaceFolders matchedWSF = (from wsf in dalDataContext.ArtefactWSFolders
                                           where wsf.EventID == eventID &&
                                           wsf.FolderName.ToLower() == folderName.ToLower() &&
                                           wsf.CreatedBy.ToLower() == user.UserID.ToLower()
                                           select wsf).FirstOrDefault();

            if (matchedWSF == null)
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid Folder"));
            }
            else
            {
                matchedWSF.FolderDescription = folderDesc;
                matchedWSF.Remarks = remarks;
                dalDataContext.SubmitChanges();
            }
        }

        public static WorkspaceFolders GetWorkSpaceFolder(User user, int eventID, string folderName)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (RoleLogicController.isEventFacilitator(eventID, user.UserID))
            {
                DAL dalDataContext = new DAL();

                WorkspaceFolders existingWS = (from ws in dalDataContext.ArtefactWSFolders
                                               where ws.EventID == eventID &&
                                               ws.FolderName.ToLower() == folderName.ToLower()
                                               select ws).FirstOrDefault();

                return existingWS;
            }
            else
                return null;
        }

        public static void UploadNewFile(User user, int eventID, string folderName, string fileName, string fileDesc, string fileURL)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (RoleLogicController.isEventFacilitator(eventID, user.UserID))
            {

                WorkspaceFiles wsf = new WorkspaceFiles(user.UserID, eventID, folderName, fileName, fileDesc, fileURL);
                if (GetWorkSpaceFile(user, eventID, folderName, fileName) == null)
                {
                    WorkspaceFiles newWorkspace = new WorkspaceFiles(user.UserID, eventID, folderName, fileName, fileDesc, fileURL);

                    DAL dalDataContext = new DAL();
                    Table<WorkspaceFiles> wsTable = dalDataContext.ArtefactWSFiles;
                    wsTable.InsertOnSubmit(newWorkspace);
                    wsTable.Context.SubmitChanges();
                }
                else
                {
                    DeleteFile(user, eventID, folderName, fileName);
                    UploadNewFile(user, eventID, folderName, fileName, fileDesc, fileURL);
                }
            }
            else
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("You are not a facilitator of the event and thus you cannot upload any file!"));
            }
        }

        public static void DeleteFile(User user, int eventID, string folderName, string fileName)
        {
            Events evnt = EventController.GetEvent(eventID);
            DAL dalDataContext = new DAL();

            try
            {
                WorkspaceFiles matchedWS = (from wsf in dalDataContext.ArtefactWSFiles
                                            where wsf.EventID == eventID &&
                                            wsf.FolderName.ToLower() == folderName.ToLower() &&
                                            wsf.FileName.ToLower() == fileName.ToLower()
                                            select wsf).FirstOrDefault();

                if (matchedWS != null)
                {
                    //TODO: Add Enum CreateArtefactFolder in functions - Nick

                    if (user.UserID.Equals(matchedWS.UploadedBy, StringComparison.CurrentCultureIgnoreCase)
                        || RoleLogicController.HaveRights(evnt, user, EnumFunctions.Manage_Artefacts)) //user.isAuthorized(evnt, ManageArtefact) after enum f(x) up
                    {
                        dalDataContext.ArtefactWSFiles.DeleteOnSubmit(matchedWS);
                        dalDataContext.SubmitChanges();
                    }
                    else
                    {
                        throw new FaultException<SException>(new SException(),
                       new FaultReason("You cannot delete the file that is not uploaded by you!"));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<SException>(new SException(ex.Message),
                   new FaultReason("An Error occured While Deleting Workspace Files, Please Try Again!"));
            }
        }

        //File definitely cannot be renamed or it will be very messy..
        //File with the same name will be overwritten by calling update file method
        public static void UpdateFile(User user, int eventID, string folderName, string fileName, string fileDesc, string fileURL)
        {
            Events evnt = EventController.GetEvent(eventID);

            if (RoleLogicController.isEventFacilitator(eventID, user.UserID))
            {
                DAL dalDataContext = new DAL();

                WorkspaceFiles matchedWSF = (from wsf in dalDataContext.ArtefactWSFiles
                                             where wsf.EventID == eventID &&
                                             wsf.FolderName.ToLower() == folderName.ToLower() &&
                                             wsf.FileName.ToLower() == fileName.ToLower()
                                             select wsf).FirstOrDefault();

                if (matchedWSF == null)
                {
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid File"));
                }
                else
                {
                    if (user.UserID.Equals(matchedWSF.UploadedBy, StringComparison.CurrentCultureIgnoreCase))
                    {
                        matchedWSF.FileDescription = fileDesc;
                        matchedWSF.FileURL = fileURL;
                        dalDataContext.SubmitChanges();
                    }
                    else
                    {
                        throw new FaultException<SException>(new SException(),
                       new FaultReason("You cannot update the file that is not uploaded by you!"));
                    }
                }
            }
        }

        public static WorkspaceFiles GetWorkSpaceFile(User user, int eventID, string folderName, string fileName)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (RoleLogicController.isEventFacilitator(eventID, user.UserID))
            {
                DAL dalDataContext = new DAL();

                WorkspaceFiles existingWSF = (from wsf in dalDataContext.ArtefactWSFiles
                                              where wsf.EventID == eventID &&
                                              wsf.FolderName.ToLower() == folderName.ToLower() &&
                                              wsf.FileName.ToLower() == fileName.ToLower()
                                              select wsf).FirstOrDefault();

                return existingWSF;
            }
            else
                return null;
        }

        public static List<WorkspaceFolders> GetWorkSpaceFolders(User user, int eventID)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (RoleLogicController.isEventFacilitator(eventID, user.UserID))
            {
                DAL dalDataContext = new DAL();

                List<WorkspaceFolders> matchedWSF = (from wsf in dalDataContext.ArtefactWSFolders
                                                     where wsf.EventID == eventID
                                                     select wsf).ToList<WorkspaceFolders>();

                return matchedWSF;

            }
            else
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason("You are not authorized to view the workspace folders!"));
            }
        }

        public static List<WorkspaceFiles> GetWorkSpaceFiles(User user, int eventID, string folderName)
        {
            Events evnt = EventController.GetEvent(eventID);
            if (RoleLogicController.isEventFacilitator(eventID, user.UserID))
            {
                DAL dalDataContext = new DAL();

                List<WorkspaceFiles> matchedWSF = (from wsf in dalDataContext.ArtefactWSFiles
                                                   where wsf.EventID == eventID &&
                                                   wsf.FolderName.ToLower() == folderName.ToLower()
                                                   select wsf).ToList<WorkspaceFiles>();

                return matchedWSF;

            }
            else
            {
                throw new FaultException<SException>(new SException(),
                  new FaultReason("You are not authorized to view the workspace files!"));
            }
        }

    }


}