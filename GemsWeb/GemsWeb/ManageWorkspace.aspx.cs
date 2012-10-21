using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;

namespace GemsWeb
{
    public partial class ManageWorkspace : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            /*http://couldbedone.blogspot.com/2007/07/what-wrong-with-accordion-control.html*/
            x.FindControl("null");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //userid = (string)Session["username"];
            txtFileURL.Text = "abc";//userid;
            //String dir = Server.MapPath("~") + "\\usr\\" + userid;
            //Double size = 0.00;
            //if (size > 120)
            //{
            //    fuFileUpload.Enabled = false;
            //    lblMsg.Text = "You have exceeded the 100MB Limit for your files, please purchase more space!";
            //}
            loadTreeView();
            //if (!Page.IsPostBack)
            //{
            //    lblSelectedClass.Text = "NIL";
            //    //loadStudentID();
            //    //pnlAnnouce.Visible = False;
            //    try
            //    {
            //        int login = (int)Session["Login"];
            //        if (login > 0)
            //        {
            //            hypSpace.NavigateUrl += "?uid" + userid;
            //        }
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}
        }

        protected void treeWS_SelectedNodeChanged(object sender, EventArgs e)
        {
            btnResetClass_Click(sender, e);
            btnResetFolder_Click(sender, e);
            btnResetFile_Click(sender, e);

            TreeNode tn = treeWS.SelectedNode;
            int level = getNodeLevel(tn.ValuePath);
            lblSelectedClass.Text = tn.ValuePath.Trim();

            if (level == 0)
            {
                hidClass.Value = "";
                hidFolder.Value = "";
                hidFile.Value = "";
                //pnlAnnouce.Visible = false;
                return;
            }
            else
            {
                //Class
                if (level == 1)
                {
                    lvl1Selected(tn);
                    loadFiles(tn.Value.Trim());
                    hidFolder.Value = "";
                    //annEditor.SaveDirectory = Server.MapPath("~") + "\\usr\\" + userid + "\\" + txtClassCode.Text.Trim();
                    //annEditor.SaveFileName = "notice.csb";
                    if (ddlAction.SelectedIndex == 0)
                    {
                        //pnlAnnouce.Visible = true;
                    }
                    //Folder
                }
                else if (level == 2)
                {
                    lvl2Selected(tn);
                    loadFiles(tn.Parent.Value, tn.Value.Trim());
                    hidFile.Value = "";
                    //annEditor.SaveDirectory = Server.MapPath("~") + "\\usr\\" + userid + "\\" + txtClassCode.Text.Trim();
                    //annEditor.SaveFileName = "notice.csb";
                    if (ddlAction.SelectedIndex == 0)
                    {
                        //pnlAnnouce.Visible = true;
                    }
                    //Files
                }
                else if (level == 3)
                {
                    lvl3Selected(tn);

                }
            }

        }

        #region "Load"

        private void loadStudentID()
        {
            //clsWorkspacePw_Data pw_data = new clsWorkspacePw_Data(WSPwPath);
            //if ((pw_data.createdStudent(userid)))
            //{
            //    x.SelectedIndex = 1;
            //    txtLoginID.Text = pw_data.getStudentID(userid).Trim();
            //}
            //else
            //{
            //    x.SelectedIndex = 0;
            //}
        }

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
            treeWS.Nodes.Clear();
            TreeNode treeRoot = new TreeNode("AAA");

            //treeWS.Nodes.Clear();
            //TreeNode treeRoot = new TreeNode(userid);
            //treeRoot.ImageUrl = "~/Images/folders.gif";

            //DataTable dt = new clsWorkspace_Data(WSPath).Select_UserClass(userid);

            //if (dt == null)
            //{
            //    return;
            //}

            //foreach (DataRow r in dt.Rows)
            //{
            //    string code = r[fldWorkspace.ClassCode].ToString().Trim();
            //    TreeNode tn = new TreeNode(code);
            //    tn.ImageUrl = "~/images/class.gif";
            //    tn.ToolTip = r[fldWorkspace.ClassName.ToString()].ToString().Trim().Trim();
            //    loadFolders(tn, code);
            //    treeRoot.ChildNodes.Add(tn);
            //}
            treeWS.Nodes.Add(treeRoot);

            treeWS.ExpandAll();
        }

        private void lvl1Selected(TreeNode tn)
        {
            //hidClass.Value = tn.Value;
            //DataTable dt = new clsWorkspace_Data(WSPath).Select_ID(tn.Value, userid);
            //txtClassCode.Text = tn.Value.Trim();
            //txtClassName.Text = dt.Rows[0][fldWorkspace.ClassName].ToString().Trim();

        }

        private void lvl2Selected(TreeNode tn)
        {
            //hidFolder.Value = tn.Value;
            //DataTable dt = new clsWorkSpaceFolders_Data(WSFolderPath).Select_ID(tn.Value, userid);
            //txtfolderName.Text = tn.Value.Trim();
            //txtfolderDesc.Text = dt.Rows[0][fldWorkSpaceFolders.FolderDesc].ToString().Trim();
            //lvl1Selected(tn.Parent);

        }

        private void lvl3Selected(TreeNode tn)
        {
            //hidFile.Value = tn.Value;
            //DataTable dt = new clsWorkSpaceFiles_Data(WSFilePath).Select_ID(tn.Value, userid);

            //txtFileDesc.Text = dt.Rows[0][fldWorkSpaceFiles.FileDesc].ToString().Trim();
            //lvl2Selected(tn.Parent);

        }

        private void loadFolders(TreeNode tn, string code)
        {
            //DataTable dt = new clsWorkSpaceFolders_Data(WSFolderPath).Select_userclass(userid, code);

            //if (dt == null)
            //{
            //    return;
            //}

            //foreach (DataRow r in dt.Rows)
            //{
            //    TreeNode childNode = new TreeNode(r[fldWorkSpaceFolders.FolderName].ToString());
            //    childNode.ImageUrl = "~/images/folder.gif";
            //    childNode.ToolTip = r[fldWorkSpaceFolders.FolderDesc.ToString()].ToString().Trim();
            //    tn.ChildNodes.Add(childNode);
            //}
        }

        private void loadFiles(string classid, string folderid = "-")
        {
            //clsWorkSpaceFiles_Data fdata = new clsWorkSpaceFiles_Data(WSFilePath);
            //DataTable dt = fdata.selectUserFiles(userid, classid, folderid);
            //gvFiles.DataSource = dt;
            //gvFiles.DataBind();

        }
        #endregion

        #region " Reset  "
        protected void btnResetClass_Click(object sender, EventArgs e)
        {
            txtClassCode.Text = "";
            txtClassName.Text = "";
        }

        protected void btnResetFolder_Click(object sender, EventArgs e)
        {
            txtfolderName.Text = "";
            txtfolderDesc.Text = "";
        }

        protected void btnResetFile_Click(object sender, EventArgs e)
        {
            txtFileDesc.Text = "";
        }
        #endregion

        #region " Delete "
        protected void btnDeleteClass_Click(object sender, EventArgs e)
        {
            string classid = hidClass.Value.Trim();
            if (classid.Trim().Length == 0)
            {
                lblMsg.Text = "Please select a class";
                return;
            }

            //clsWorkspace_Data class_Data = new clsWorkspace_Data(WSPath);
            string dir = Server.MapPath("~") + "\\usr\\" + lblSelectedClass.Text.Trim().Replace("/", "\\");
            Debug.WriteLine(dir);
            //class_Data.deleteClassFiles(userid, classid);
            DeleteDirectory(dir);
            loadTreeView();
            loadFiles(classid);
            btnResetClass_Click(sender, e);
        }

        protected void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            string classid = txtClassCode.Text.Trim();
            string folderid = hidFolder.Value.Trim();

            if (folderid.Trim().Length == 0)
            {
                lblMsg.Text = "Please select a folder";
                return;
            }

            //clsWorkSpaceFolders_Data folder_data = new clsWorkSpaceFolders_Data(WSFolderPath);
            string dir = Server.MapPath("~") + "\\usr\\" + lblSelectedClass.Text.Trim().Replace("/", "\\");
            //folder_data.deleteFolder(userid, classid, folderid);
            DeleteDirectory(dir);
            loadTreeView();
            loadFiles(classid, folderid);
            btnResetFolder_Click(sender, e);
        }
        #endregion

        public Boolean DeleteDirectory(String target_dir)
        {
            Boolean result = false;
            if (Directory.Exists(target_dir))
            {
                String[] files = Directory.GetFiles(target_dir);
                String[] dirs = Directory.GetDirectories(target_dir);
                foreach (string file__1 in files)
                {
                    File.SetAttributes(file__1, FileAttributes.Normal);
                    File.Delete(file__1);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }
            }
            return result;
        }

        protected string customStringtoDateS(string s)
        {
            System.DateTime fD = new System.DateTime(int.Parse(s.ToString().Trim().Substring(0, 4)), 
                int.Parse(s.ToString().Trim().Substring(4, 2)),
                int.Parse(s.ToString().Trim().Substring(6, 2)));

            return fD.ToShortDateString();
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

                int index = (int)e.CommandArgument;
                string fileID = ((Label)gvFiles.Rows[index].Cells[2].FindControl("lblFileName")).Text;
                //clsWorkSpaceFiles_Data fdata = new clsWorkSpaceFiles_Data(WSFilePath);
                //string folderID = fdata.getFolderID(fileID, userid);
                //string classId = fdata.getClassID(fileID, userid);
                //fdata.deleteFiles(userid, classId, folderID, fileID);

                string filepath = Server.MapPath("~") + "\\usr\\" + lblSelectedClass.Text.Replace("/", "\\") + "\\" + ((Label)gvFiles.Rows[index].Cells[2].FindControl("lblFileName")).Text;

                if (filepath.Contains("\\usr\\"))
                {
                    //lblFilePath.Text = filepath
                    //lblFilePath.Text &= "<br> Exist: " & IO.File.Exists(filepath).ToString()
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }

                //loadFiles(classId, folderID);
            }
        }

        protected void btnStuLogin_Click(object sender, EventArgs e)
        {
            //clsWorkspacePw_Data pw_data = new clsWorkspacePw_Data(WSPwPath);
            //if ((pw_data.exist(txtLoginID.Text.Trim())))
            //{
            //    lblMsgLogin.Text = "The student id have been used by another tutor, please use another.";


            //}
            //else
            //{
            //    clsWorkspacePw pw = new clsWorkspacePw(userid, WSPwPath);
            //    var _with1 = pw;
            //    _with1.SetContent(fldWorkspacePw.StudentLogin, txtLoginID.Text.Trim());
            //    _with1.SetContent(fldWorkspacePw.StudentPwd, txtVerifyPw.Text.Trim());
            //    if (_with1.SaveToDataBase())
            //    {
            //        lblMsgLogin.Text = "Your student id have been created!";
            //    }
            //}

        }

        protected void btnPwReset_Click(object sender, EventArgs e)
        {

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
            //pnlAnnouce.Visible = pnlClass.Visible;
        }

        protected void gvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidFile.Value = ((Label)gvFiles.SelectedRow.Cells[2].FindControl("lblFileName")).Text;
            txtFileDesc.Text = ((Label)gvFiles.SelectedRow.Cells[2].FindControl("lblFileDesc")).Text;
        }
    }
}
