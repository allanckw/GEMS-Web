<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="contactus.aspx.cs" Inherits="GemsWeb.contactus" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <table width="95%">
        <tr>
            <th style="width: 30%; height: 22px" align="right">
                <asp:Label ID="Label2" runat="server" Text="Title: "></asp:Label>
            </th>
            <td align="left" style="width: 69%; height: 22px">
                <asp:RadioButtonList ID="radTitle" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True">Mr.</asp:ListItem>
                    <asp:ListItem>Mrs.</asp:ListItem>
                    <asp:ListItem>Miss</asp:ListItem>
                    <asp:ListItem>Dr.</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th style="width: 30%; height: 22px" align="right">
                <asp:Label ID="Label3" runat="server" Text="Name: "></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="dynamic"
                    ValidationGroup="zzz" ErrorMessage="*" ControlToValidate="txtContactName"></asp:RequiredFieldValidator>
            </th>
            <td align="left" style="width: 69%; height: 22px">
                <asp:TextBox ID="txtContactName" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th style="width: 30%; height: 22px" align="right">
                <asp:Label ID="Label11" runat="server" Text="E-mail: "></asp:Label>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="* You must enter a value in the text box"
                    ValidationGroup="Email" ControlToValidate="txtEmail" Display="dynamic">
                    *
                </asp:RequiredFieldValidator>
            </th>
            <td align="left" style="width: 69%; height: 22px">
                <asp:TextBox ID="txtEmail" runat="server" Width="250px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                    Display="Dynamic" runat="server">
                    Invalid E-mail Address.
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th style="width: 30%; height: 22px" align="right">
                <asp:Label ID="Label5" runat="server" Text="Nature of enquiry: "></asp:Label>
            </th>
            <td align="left" style="width: 69%; height: 22px">
                <asp:RadioButtonList ID="RadNature" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True">&nbsp;General Feedback&nbsp;&nbsp;&nbsp;   </asp:ListItem>
                    <asp:ListItem>&nbsp;Question&nbsp;&nbsp;&nbsp;   </asp:ListItem>
                    <asp:ListItem>&nbsp;Error&nbsp;&nbsp;&nbsp;   </asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th style="width: 30%; height: 22px" align="right">
                <asp:Label ID="Label1" runat="server" Text="Enquiry:"></asp:Label>
            </th>
            <td align="left" style="width: 69%; height: 22px">
                <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" Width="500px" Height="166px"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* You must enter a value in the text box"
                    ValidationGroup="Email" ControlToValidate="txtMsg" Display="dynamic">
                    Message is required
                </asp:RequiredFieldValidator><br />
            </td>
        </tr>
        <tr id="Captcha">
            <td style="width: 30%" align="right">
                <asp:Label ID="Label17" runat="server" Text="Word verification:"></asp:Label>
            </td>
            <td style="width: 67%">
                <cc1:CaptchaControl ID="ccJoin" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                    CaptchaHeight="60" CaptchaWidth="200" CaptchaLineNoise="None" CaptchaMinTimeout="15"
                    CaptchaMaxTimeout="240" />
                <asp:TextBox ID="txtCaptcha" runat="server" Width="180px" MaxLength="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* You must enter a value in the text box"
                    ControlToValidate="txtCaptcha" Display="Static" Visible="true"> * </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit"
                    CssClass="button" ValidationGroup="Email" />
            </td>
        </tr>
    </table>
</asp:Content>

