<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ManageRequest.aspx.cs" Inherits="GemsWeb.ManageRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
<div id="clear"></div>
    <div id="content2" style="float: left; width: 100%;">
        <table border="1">
            <tr>
                <td rowspan="5" align="left" valign="top" style="width: 200px">
                    List of Request
                    <br />
                    <asp:ListBox ID="lstRequest" AutoPostBack="true" Width="200px" Height="500px" runat="server">
                    </asp:ListBox>
                </td>
                <td style="width: 100px; height: 20px">
                    Title
                </td>
                <td style="width: 300px">
                    <asp:TextBox ID="txtRequestTitle" Width="300px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Description
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtRequestDesc" runat="server" TextMode="MultiLine" Width="500px"
                        Height="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
