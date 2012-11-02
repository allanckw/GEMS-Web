﻿using System;
using System.IO;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using evmsService.entities;
using GemsWeb.Controllers;

namespace GemsWeb
{
    public partial class ArtefactBin : System.Web.UI.Page
    {
        string wrkSpaceDir = "WorkSpace\\";
        protected void Page_Init(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            /*http://couldbedone.blogspot.com/2007/07/what-wrong-with-accordion-control.html*/
            x.FindControl("null");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //int eventID = int.Parse(Request.QueryString["EventID"]);
                //EventClient evClient = new EventClient();
                //Events event_ = evClient.GetEvent(eventID);
                //evClient.Close();
                lblSelectedFolder.Text = "-";
            }

            if (NUSNetUser() == null)
            {
                Alert.Show("Please Login First", true);
            }

            loadTreeView();
        }

        protected void treeWS_SelectedNodeChanged(object sender, EventArgs e)
        {
            btnResetFolder_Click(sender, e);
            btnResetFile_Click(sender, e);

            TreeNode tn = treeWS.SelectedNode;
            int level = getNodeLevel(tn.ValuePath);

            if (level == 0) // Event
            {

                lblSelectedFolder.Text = "-";
                loadFiles();
                hidFolder.Value = "";
                hidFile.Value = "";
                return;
            }
            else
            {
                //Folder
                if (level == 1)
                {
                    lblSelectedFolder.Text = tn.Value.Trim();
                    lvlFolderSelected(tn);
                    loadFiles(tn.Value.Trim());
                    if (ddlAction.SelectedIndex == 0)
                    {
                        //pnlAnnouce.Visible = true;
                        //Folder
                    }
                }

            }
        }

        #region "Load"

        private double GetFolderSize(string DirPath, bool IncludeSubFolders = true)
        {
            //long lngDirSize = 0;
            ////FileInfo objFileInfo = default(FileInfo);
            //DirectoryInfo objDir = new DirectoryInfo(DirPath);
            //try {
            //    //add length of each file

            //    foreach (FileInfo objFileInfo in objDir.GetFiles()) {
            //        lngDirSize += objFileInfo.Length;
            //    }

            //    //call recursively to get sub folders
            //    //if you don't want this set optional
            //    //parameter to false 
            //    if (IncludeSubFolders) {
            //        foreach (DirectoryInfo objSubFolder in objDir.GetDirectories())
            //        {
            //            lngDirSize += GetFolderSize(objSubFolder.FullName);
            //        }
            //    }

            //} catch (Exception Ex) {
            //}

            //return lngDirSize;
            return 0;
        }

        private void loadTreeView()
        {
            EventClient evClient = new EventClient();
            Events event_ = evClient.GetEvent(EventID());
            evClient.Close();

            treeWS.Nodes.Clear();
            TreeNode treeRoot = new TreeNode(event_.Name);
            treeRoot.ImageUrl = "~/images/folders.gif";
            loadFolders(treeRoot);
           
            treeWS.Nodes.Add(treeRoot);
            treeWS.ExpandAll();
        }

        private void lvlFolderSelected(TreeNode tn)
        {
            ArtefactClient client = new ArtefactClient();
            WorkspaceFolders wrkSpaceFolder = client.GetWorkSpaceFolder(EventID(), tn.Value);
            client.Close();
            hidFolder.Value = tn.Value;

            txtfolderName.Text = tn.Value.Trim();
            txtfolderName.ReadOnly = true;
            txtfolderDesc.Text = wrkSpaceFolder.FolderDescription.Trim();
        }

        private void loadFolders(TreeNode tn)
        {
            User u = NUSNetUser();
            if (u == null)
            {
                return;
            }
            ArtefactClient arclient = new ArtefactClient();

            WorkspaceFolders[] WrkSpace = arclient.GetWorkSpaceFolders(NUSNetUser(), EventID());
            if (WrkSpace == null || WrkSpace.Count() == 0)
            {
                return;
            }

            foreach (WorkspaceFolders folder in WrkSpace)
            {
                TreeNode childNode = new TreeNode(folder.FolderName.ToString());
                childNode.ImageUrl = "~/images/folder.gif";
                childNode.ToolTip = folder.FolderDescription.ToString().Trim();
                tn.ChildNodes.Add(childNode);
            }
        }

        private void loadFiles(string folderName = "-")
        {
            ArtefactClient arclient = new ArtefactClient();
            WorkspaceFiles[] WrkSpaceFile = arclient.GetWorkSpaceFiles(NUSNetUser(), EventID(), folderName);
            gvFiles.DataSource = WrkSpaceFile;
            gvFiles.DataBind();
        }
        #endregion

        #region " Reset  "
        protected void btnResetFolder_Click(object sender, EventArgs e)
        {
            txtfolderName.Text = "";
            txtfolderDesc.Text = "";
            txtfolderName.ReadOnly = false;
            lblSelectedFolder.Text = "-";
        }

        protected void btnResetFile_Click(object sender, EventArgs e)
        {
            txtFileDesc.Text = "";
            txtFileURLExt.Text = "";
        }
        #endregion

        #region " Delete "
        protected void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            //string classid = txtClassCode.Text.Trim();
            string folderid = hidFolder.Value.Trim();

            if (folderid.Trim().Length == 0)
            {
                lblMsg.Text = "Please select a folder";
                return;
            }


            try
            {
                ArtefactClient arClient = new ArtefactClient();
                arClient.DeleteFolder(NUSNetUser(), EventID(), lblSelectedFolder.Text.Trim());
                string dir = workSpaceDir(lblSelectedFolder.Text.Trim());
                DeleteDirectory(dir);
                loadTreeView();
                btnResetFolder_Click(sender, e);
                Alert.Show("Folder Deleted Successfully!");
            }
            catch (Exception)
            {
                Alert.Show("Folder Failed to Delete!");
                throw;
            }
        }
        #endregion

        #region " Add "
        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            ArtefactClient arClient = new ArtefactClient();
            try
            {
                string foldercode = txtfolderName.Text.ToString().Trim().Replace(" ", "_");

                if (lblSelectedFolder.Text.Trim() != "-")
                {
                    arClient.UpdateFolder(NUSNetUser(), EventID(), foldercode, txtfolderDesc.Text.Trim(), "");
                    Alert.Show("Folder updated Successfully!");
                }
                else
                {
                    //juz add new folder
                    arClient.CreateFolder(NUSNetUser(), EventID(), foldercode, txtfolderDesc.Text.Trim(), "");
                    CreateFolder(foldercode);
                    loadTreeView();
                    Alert.Show("Folder created!");
                }

                btnResetFolder_Click(sender, e);
            }
            catch (Exception)
            {
                Alert.Show("Folder failed to create!");
                throw;
            }
            finally
            {
                arClient.Close();
            }
        }

        protected void btnAddFile_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (lblSelectedFolder.Text.Trim() != "-")
            {
                if (gvFiles.SelectedIndex < 0)
                {
                    if (!fuFileUpload.HasFile && txtFileURLExt.Text.Trim().Length == 0)
                    {
                        lblMsg.Text = "Please attach your file or specify the file name in the external url text field.";
                        return;
                    }
                    else if (fuFileUpload.HasFile && txtFileURLExt.Text.Trim().Length > 0)
                    {
                        lblMsg.Text = "Please only attach your file OR specify the file name in your external url, not both.";
                        return;
                    }
                }
            }
            else
            {
                lblMsg.Text = "You Cannot Upload file at root Folder!";
                return;
            }

            string filename = null;
            string fileurl = "";

            ArtefactClient arClient = new ArtefactClient();

            if (fuFileUpload.HasFile)
            {
                filename = fuFileUpload.FileName.Replace(" ", "_");
            }
            else
            {
                fileurl = txtFileURLExt.Text.Trim();
                filename = fileurl.Substring(fileurl.LastIndexOf("/") + 1);
            }

            try
            {
                if (hidFile.Value == "")
                {
                    //Upload New file                    
                    arClient.UploadFile(NUSNetUser(), EventID(), lblSelectedFolder.Text.Trim(), filename, txtFileDesc.Text.Trim(), fileurl);
                    
                    if (fuFileUpload.HasFile)
                    {
                        UploadFile(filename);
                    }

                    loadFiles(lblSelectedFolder.Text.Trim());
                }
                else
                {
                    arClient.UpdateFile(NUSNetUser(), EventID(), lblSelectedFolder.Text.Trim(), hidFile.Value, txtFileDesc.Text.Trim(), fileurl);
                }
                lblMsg.Text = "Upload Success";
                txtFileDesc.Text = "";
                txtFileURLExt.Text = "";
                loadFiles(lblSelectedFolder.Text.Trim());
            }
            catch (Exception)
            {
                lblMsg.Text = "Upload Failed";
                throw;
            }     


            //    oldcode = hidFile.Value;
            //    //classid = txtClassCode.Text.Trim();
            //    folderid = txtfolderName.Text.Trim();

            //    if (filename.Trim().Length == 0 && hidFile.Value.Trim().Length > 0)
            //    {
            //        filename = oldcode;
            //    }

            //    if (string.Compare(filename.ToLower().Trim(), oldcode.ToLower().Trim()) == 0)
            //    {
            //        filename = oldcode;
            //    }

            //    clsWorkSpaceFiles ws = new clsWorkSpaceFiles(userid, filename, classid, folderid, WSFilePath);

            //    var _with1 = ws;

            //    if (filename == oldcode)
            //    {
            //    }
            //    else if (fuFileUpload.HasFile)
            //    {
            //        _with1.SetContent(fldWorkSpaceFiles.FileName, filename);
            //        string fileURL = URIController.getTutorFileUrl(Request.Url.ToString().Replace(Request.RawUrl.Replace("%2f", "/"), ""), lblSelectedClass.Text.Trim()) + filename;
            //        _with1.SetContent(fldWorkSpaceFiles.FileURL, fileURL);
            //    }
            //    else
            //    {
            //        filename = txtFileURLExt.Text.Trim.Substring(txtFileURLExt.Text.Trim().LastIndexOf("/") + 1);
            //        _with1.SetContent(fldWorkSpaceFiles.FileName, filename);
            //        string fileURL = "http://" + txtFileURL.Text.Trim() + ".agradesite.com/" + txtFileURLExt.Text.Trim();
            //        _with1.SetContent(fldWorkSpaceFiles.FileURL, fileURL);
            //    }

            //    _with1.SetContent(fldWorkSpaceFiles.FileDesc, txtFileDesc.Text.Trim());
            //    _with1.SetContent(fldWorkSpaceFiles.DateUploaded, customDatetoString(DateTime.Now));
            //    _with1.SetContent(fldWorkSpaceFiles.TimeUploaded, getTimeString(DateTime.Now));
            //    _with1.SetContent(fldWorkSpaceFiles.userID, userid);
            //    _with1.SetContent(fldWorkSpaceFiles.folderID, folderid);
            //    _with1.SetContent(fldWorkSpaceFiles.ClassID, classid);

            //    if (_with1.SaveToDataBase())
            //    {
            //        if (fuFileUpload.HasFile)
            //        {
            //            string dir = "";
            //            if (getNodeLevel(lblSelectedClass.Text) == 1)
            //            {
            //                dir = Server.MapPath("~") + "\\usr\\" + lblSelectedClass.Text.Replace("/", "\\") + "\\" + filename;
            //            }
            //            else if (getNodeLevel(lblSelectedClass.Text) == 2)
            //            {
            //                dir = Server.MapPath("~") + "\\usr\\" + lblSelectedClass.Text.Replace("/", "\\");
            //            }
            //            if (string.Compare(filename.ToLower().Trim(), oldcode.ToLower().Trim()) == 0)
            //            {
            //                fuFileUpload.SaveAs(dir);
            //            }
            //            else
            //            {
            //                fuFileUpload.SaveAs(dir + "\\" + filename);
            //            }
            //        }
            //        if (txtfolderName.Text.Trim.Length > 0)
            //        {
            //            loadFiles(txtClassCode.Text.Trim(), txtfolderName.Text.Trim());
            //        }
            //        else
            //        {
            //            loadFiles(txtClassCode.Text.Trim());
            //        }

            //        lblMsg.Text = "Upload Success";
            //        txtFileDesc.Text = "";
            //        txtFileURLExt.Text = "";
            //    }
            //    hidFile.Value = "";
            //    gvFiles.SelectedIndex = -1;
            //}

        }
        #endregion

        public Boolean DeleteDirectory(String target_dir)
        {
            Boolean result = false;
            if (Directory.Exists(target_dir))
            {
                result = true;
                Directory.Delete(target_dir);
            }
            return result;
        }

        private int getNodeLevel(string path)
        {
            int x = path.Length - path.Replace("/", "").Length;
            return x;
        }

        protected void gvFiles_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                
                string fileID = ((Label)gvFiles.Rows[index].Cells[2].FindControl("lblFileName")).Text.Replace(" ","%20");

                string filepath = workSpaceDir(lblSelectedFolder.Text.Trim()) + "\\" + ((Label)gvFiles.Rows[index].Cells[2].FindControl("lblFileName")).Text;
                ArtefactClient arClient = new ArtefactClient();
                try
                {
                    arClient.DeleteFile(NUSNetUser(), EventID(), lblSelectedFolder.Text.Trim(), fileID);

                    if (filepath.Contains("\\WorkSpace\\"))
                    {
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }

                    lblMsg.Text = "File Successfully removed!";
                    loadFiles(lblSelectedFolder.Text.Trim());
                }
                catch (Exception)
                {
                    lblMsg.Text = "File Failed to remove!";
                }
                finally
                {
                    arClient.Close();
                }
               

                //loadFiles(classId, folderID);
            }
        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAction.SelectedIndex == 0)
            {
                pnlClass.Visible = true;
            }
            else
            {
                pnlClass.Visible = false;
            }
            pnlFiles.Visible = !pnlClass.Visible;
        }

        protected void gvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidFile.Value = ((Label)gvFiles.SelectedRow.Cells[2].FindControl("lblFileName")).Text;
            txtFileDesc.Text = ((Label)gvFiles.SelectedRow.Cells[2].FindControl("lblFileDesc")).Text;
        }

        #region "Page Global Function"
        private User NUSNetUser()
        {
            try
            {
                User u = (User)Session["nusNETuser"];
                return u;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private int EventID()
        {
            int eventID = int.Parse(Request.QueryString["EventID"]);
            return eventID;
        }

        private string workSpaceDir(string folderName = "-")
        {
            if (folderName == "-")
                folderName = "";

            return Server.MapPath("~") + wrkSpaceDir + EventID().ToString() + "\\" + folderName;
        }
        #endregion

        private void CreateFolder(string folderName)
        {
            string userDir = workSpaceDir(folderName);

            if (!System.IO.Directory.Exists(userDir))
            {
                System.IO.Directory.CreateDirectory(userDir);
            }
        }

        private string UploadFile(string fileName)
        {
            string uploadPath = workSpaceDir(lblSelectedFolder.Text.Trim()) + "\\" + fileName;
            fuFileUpload.SaveAs(uploadPath);
            return uploadPath;
        }

        protected string Uploader(string s)
        {
            AdministrationClient adClient = new AdministrationClient();
            string uploader = adClient.GetUserName(s);
            adClient.Close();
            return uploader;
        }

        protected string hyperLinkage(string url, string fileName)
        {
            if (url != "")
                return url;

            string path = Request.Url.ToString().Replace(Request.RawUrl.Replace("%2f", "/"), "") + "/" + wrkSpaceDir.Replace("\\", "/") + EventID() + "/" + lblSelectedFolder.Text.Trim() + "/" + fileName;
            return path;
        }
    }
}