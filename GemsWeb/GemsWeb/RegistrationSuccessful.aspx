<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="RegistrationSuccessful.aspx.cs" Inherits="GemsWeb.RegistrationSuccessful" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <div style="margin-left: 10%;">
        <h3>
            <a href="#">You have successfully registered for the event </a>
        </h3>
        <asp:HyperLink ID="hypGoBack" NavigateUrl="~/Default.aspx" Text="Home" runat="server" />
    </div>
</asp:Content>
