<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="forgetPassword.aspx.cs" Inherits="GemsWeb.forgetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:PlaceHolder ID="ph1" runat="server">
        <center>
        <span style="color: Blue; font-weight: bold;">Enter the e-mail address and select the domain and we will send you your password via e-mail. </span>
    <table cellpadding="0">
        <tr>
            <th align="center" colspan="2" style="background-color: #005A97; color: White;">
                Retrieve Password
            </th>
        </tr>
        <tr>
            <th align="right">
                <asp:Label ID="Label1" runat="server" Text="User ID:"></asp:Label>
            </th>
            <td style="text-align: left;">
                <asp:TextBox ID="txtUserID" runat="server" Width="266px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqUserID" runat="server" ControlToValidate="txtUserID">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th align="right">
                <asp:Label ID="Label3" runat="server">E-mail:</asp:Label>
            </th>
            <td style="text-align: left;">
                <asp:TextBox ID="txtEmail" runat="server" Width="266px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                <br />
                <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                    ErrorMessage="Invalid E-mail Address." Display="Dynamic" Visible="true" runat="server">
                                                    
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                Domain: 
            </td>
            <td>
                <asp:DropDownList ID="ddlDomain" runat="server">
                    <asp:ListItem>Participant</asp:ListItem>
                    <asp:ListItem>Requestee</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="text-align: right;">
                <asp:Button ID="btnSubmit" runat="server" CommandName="Submit" OnClick="btnSubmit_Click"
                    CssClass="button" Text="Submit" ValidationGroup="PasswordRecovery1" />
            </td>
        </tr>

    </table>
    </center>
    </asp:PlaceHolder>
</asp:Content>
