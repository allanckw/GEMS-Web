<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="GemsWeb._default" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="float: left;">
                <h3>
                    <a href="#">Please Select the Date Range</a></h3>
                <table>
                    <tr>
                        <td>
                            From
                        </td>
                        <td>
                            <GEMS:DatePicker ID="dpFrom" runat="server" DisplayFutureDate="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            To
                        </td>
                        <td>
                            <GEMS:DatePicker ID="dpTo" runat="server" MonthsFromCurrent="1" DisplayFutureDate="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tag
                        </td>
                        <td>
                            <asp:TextBox ID="txtTag" runat="server" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search Events" OnClick="btnSearch_Click"
                                Height="30px" Width="160px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>
                                <a href="#">List of Events</a></h3>
                            <asp:ListBox ID="lstEvent" runat="server" Height="420px" Width="500px" >
                            </asp:ListBox>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnGO" runat="server" Text="View Event Details" Height="30px" OnClick="btnGO_Click"
                    Width="160px" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
