<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationField.ascx.cs"
    Inherits="GemsWeb.CustomControls.RegistrationField" %>
<div>
    <asp:Label ID="lblFieldName" runat="server" Text="Label"></asp:Label>
    &nbsp; &nbsp; &nbsp;
    <asp:TextBox ID="txtFieldResult" runat="server"></asp:TextBox>&nbsp;
    <asp:RequiredFieldValidator ID="reqValidator" runat="server" ControlToValidate="txtFieldResult"
        Display="Static" Visible="true"> * </asp:RequiredFieldValidator>&nbsp;
    <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtFieldResult"
        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Display="Dynamic" Visible="true" runat="server" ForeColor="Red"> Invalid E-mail Format </asp:RegularExpressionValidator>
</div>
