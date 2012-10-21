<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ManageWorkspace.aspx.cs" Inherits="GemsWeb.ManageWorkspace" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <act:Accordion ID="x" runat="server" FadeTransitions="true" HeaderCssClass="HeaderAccordian"
                ContentCssClass="ContentAccordian" Height="1312px" Width="1070px">
                <Panes>
                    <act:AccordionPane ID="AccordionPane1" runat="server">
                        <Header>
                            <h1>
                                Student Login Information</h1>
                        </Header>
                        <Content>
                            bleah
                        </Content>
                    </act:AccordionPane>
                    <act:AccordionPane ID="AccordionPane2" runat="server">
                        <Header>
                            <h1>
                                Classes Management</h1>
                            <center style="font-size: 1.2em; font-weight: bold;">Selected Class:
                 <asp:Label ID="lblSelectedClass" runat="server" Text="" ></asp:Label></center>
                        </Header>
                        <Content>
                            <div style="float: left; width: 30%; height: 480px; overflow: scroll;">
                                <b>Current Workspace</b>
                                <asp:TreeView ID="treeWS" OnSelectedNodeChanged="treeWS_SelectedNodeChanged" runat="server">
                                </asp:TreeView>
                            </div>
                            <div style="float: right; width: 69%;">
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
                                                <asp:ListItem Selected="True">Manage Classes</asp:ListItem>
                                                <asp:ListItem>Manage Files</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlClass" runat="server">
                                    <table>
                                        <tr>
                                            <th align="left" colspan="3">
                                                Classes
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%" align="right">
                                                Class Code:
                                            </td>
                                            <td style="width: 2%; color: Red">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtClassCode"
                                                    Display="Static" Visible="true" ValidationGroup="class"> * </asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 77%">
                                                <asp:HiddenField ID="hidClass" runat="server" />
                                                <asp:TextBox ID="txtClassCode" runat="server" Width="90%" ValidationGroup="class"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%" align="right">
                                                Class Name:
                                            </td>
                                            <td style="width: 2%; color: Red">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClassName"
                                                    Display="Static" Visible="true" ValidationGroup="class"> * </asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 77%">
                                                <asp:TextBox ID="txtClassName" runat="server" Width="90%" ValidationGroup="class"
                                                    MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button ID="btnAddClass" runat="server" Text="Add / Update" ValidationGroup="class"
                                                    class="button" />
                                                &nbsp; &nbsp;
                                                <asp:Button ID="btnDeleteClass" OnClick="btnDeleteClass_Click" runat="server" Text="Delete"
                                                    class="button" ValidationGroup="c" />
                                                &nbsp; &nbsp;
                                                <asp:Button ID="btnResetClass" OnClick="btnResetClass_Click" Text="Reset" runat="server"
                                                    class="button" ValidationGroup="r1" />
                                            </td>
                                        </tr>
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
                                                <asp:Button ID="btnAddFolder" runat="server" Text="Add / Update" ValidationGroup="Folder"
                                                    class="button" />
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
                                                    <asp:TextBox ID="txtFileURL" runat="server" ReadOnly="true" ValidationGroup="File"
                                                        Style="text-align: right;"></asp:TextBox>.agradesite.com/<asp:TextBox ID="txtFileURLExt"
                                                            runat="server" ValidationGroup="File"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="File">
                                                    </asp:ValidationSummary>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 39px" colspan="3" align="center">
                                                    <asp:Button ID="btnAddFile" runat="server" Text="Add / Update" ValidationGroup="File"
                                                        class="button" />
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
                                                        <asp:HyperLink ID="hypFile" runat="server" NavigateUrl='<%# Eval("FileURL").ToString().Trim() %>'
                                                            Target="_blank">
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName").ToString().Trim() %>'></asp:Label>
                                                        </asp:HyperLink>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblFileDesc" runat="server" Text='<%# Eval("FileDesc").ToString().Trim() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Date & Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# customStringtoDateS(Eval("DateUploaded").ToString().Trim()) %>'></asp:Label>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="lblTime" runat="server" Text='<%# Eval("TimeUploaded").ToString().Trim().Substring(0,4) %>'></asp:Label>
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
