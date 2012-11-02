<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ManageWorkspace.aspx.cs" Inherits="GemsWeb.ManageWorkspace" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <act:Accordion ID="x" runat="server" FadeTransitions="true" HeaderCssClass="HeaderAccordian"
                ContentCssClass="ContentAccordian">
                <Panes>
                    <act:AccordionPane ID="AccordionPane2" runat="server">
                        <Header>
                            <h1>
                                Event Folder Management</h1>
                            <center style="font-size: 1.2em; font-weight: bold;">
                                Selected Folder:
                                <asp:Label ID="lblSelectedFolder" runat="server" Text=""></asp:Label></center>
                        </Header>
                        <Content>
                            <div style="float: left; width: 35%; height: 480px; overflow: scroll;">
                                <b>Current Workspace</b>
                                <asp:TreeView ID="treeWS" OnSelectedNodeChanged="treeWS_SelectedNodeChanged" runat="server">
                                </asp:TreeView>
                            </div>
                            <div style="float: right; width: 65%;">
                                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Blue" Font-Size="Larger"
                                    Font-Bold="True">
                                </asp:Label><br />
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 20%" align="right">
                                            Select Action:
                                        </td>
                                        <td style="width: 2%; color: Red">
                                            *
                                        </td>
                                        <td style="width: 77%">
                                            <asp:RadioButtonList ID="ddlAction" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged"
                                                runat="server" AutoPostBack="true">
                                                <asp:ListItem Selected="True">Manage Folders</asp:ListItem>
                                                <asp:ListItem>Manage Files</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlClass" runat="server">
                                    <table>
                                        <tr>
                                            <th align="left" colspan="3">
                                                Folders / Categories
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%" align="right">
                                                Folder Name:
                                            </td>
                                            <td style="width: 2%; color: Red">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtfolderName"
                                                    Display="Static" Visible="true" ValidationGroup="folder"> * </asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 77%">
                                                <%--                                                <asp:HiddenField ID="hidEvent" runat="server" />--%>
                                                <asp:HiddenField ID="hidFolder" runat="server" />
                                                <asp:TextBox ID="txtfolderName" runat="server" Width="90%" ValidationGroup="folder"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%" align="right">
                                                Folder Description:
                                            </td>
                                            <td style="width: 2%; color: Red">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtfolderDesc"
                                                    Display="Static" Visible="true" ValidationGroup="folder"> * </asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 77%">
                                                <asp:TextBox ID="txtfolderDesc" runat="server" Width="90%" ValidationGroup="folder"
                                                    MaxLength="250"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button ID="btnAddFolder" OnClick="btnAddFolder_Click" runat="server" Text="Add / Update"
                                                    ValidationGroup="Folder" class="button" />
                                                &nbsp; &nbsp;
                                                <asp:Button ID="btnDeleteFolder" OnClick="btnDeleteFolder_Click" runat="server" Text="Delete"
                                                    class="button" ValidationGroup="c" />
                                                &nbsp; &nbsp;
                                                <asp:Button ID="btnResetFolder" OnClick="btnResetFolder_Click" Text="Reset" runat="server"
                                                    class="button" ValidationGroup="r1" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:UpdatePanel ID="pnlFiles" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <th align="left" colspan="3">
                                                    Files<asp:HiddenField ID="hidFile" runat="server"></asp:HiddenField>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="width: 20%" align="right">
                                                    File Description:
                                                </td>
                                                <td style="width: 2%; color: Red">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFileDesc"
                                                        Display="Static" Visible="true" ValidationGroup="File" ErrorMessage="Please enter file description"> * </asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 77%">
                                                    <asp:TextBox ID="txtFileDesc" runat="server" Width="90%" ValidationGroup="File" MaxLength="250"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 20%" align="right">
                                                    Attach:
                                                </td>
                                                <td style="width: 2%; color: Red">
                                                </td>
                                                <td style="width: 77%">
                                                    <asp:FileUpload ID="fuFileUpload" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 20%" align="right">
                                                    File URL:
                                                </td>
                                                <td style="width: 2%; color: Red">
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp; &nbsp; &nbsp; &nbsp;
                                                    <asp:TextBox ID="txtFileURLExt" Width="80%" runat="server" ValidationGroup="File"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="regexFileURL" ControlToValidate="txtFileURLExt"
                                                        runat="server" ValidationExpression="^[\s\S]{0,2000}$" Text="2000 characters max" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp; &nbsp; &nbsp; &nbsp; <font size="2" color="blue">Maximum Length. 2000 character
                                                    </font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="File">
                                                    </asp:ValidationSummary>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 39px" colspan="3" align="center">
                                                    <asp:Button ID="btnAddFile" OnClick="btnAddFile_Click" runat="server" Text="Add / Update"
                                                        ValidationGroup="File" class="button" />
                                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                    <asp:Button ID="btnResetFile" OnClick="btnResetFile_Click" Text="Reset" runat="server"
                                                        class="button" ValidationGroup="r1" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvFiles" OnRowCommand="gvFiles_RowCommand" OnSelectedIndexChanged="gvFiles_SelectedIndexChanged"
                                            runat="server" CellPadding="2" Width="100%" AutoGenerateColumns="false" EmptyDataText="There are no items to display"
                                            ForeColor="#333333" GridLines="None">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/images/Select.jpg" CommandName="Select"
                                                    HeaderStyle-Width="33px" HeaderText="Select" />
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/images/trash.png" CommandName="Del"
                                                    HeaderStyle-Width="33px" HeaderText="Delete" />
                                                <asp:TemplateField HeaderStyle-Width="200px" HeaderText="File">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hypFile" runat="server" NavigateUrl='<%# hyperLinkage(Eval("FileURL").ToString().Trim(), Eval("FileName").ToString().Trim()) %>'
                                                            Target="_blank">
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName").ToString().Trim().Replace("%20"," ") %>'></asp:Label>
                                                        </asp:HyperLink>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblFileDesc" runat="server" Text='<%# Eval("FileDescription").ToString().Trim() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Date & Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("UploadedOn").ToString().Trim() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Uploaded By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUploader" runat="server" Text='<%# Uploader(Eval("UploadedBy").ToString().Trim()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle BackColor="#E3EAEB" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" Width="95%" />
                                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                                        </asp:GridView>
                                        <br />
                                        <br />
                                        <%--<asp:Label ID="lblFilePath" runat="server" Text=""></asp:Label>--%>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnAddFile" />
                                        <asp:PostBackTrigger ControlID="btnAddFolder" />
                                        <asp:PostBackTrigger ControlID="btnDeleteFolder" />
                                        <asp:PostBackTrigger ControlID="btnDeleteFolder" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </Content>
                    </act:AccordionPane>
                </Panes>
            </act:Accordion>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
