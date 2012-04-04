<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.ascx.cs" Inherits="GemsWeb.CustomControls.DatePicker" %>
<asp:TextBox ID="txtDate" runat="server" Width="170px" ReadOnly="true"></asp:TextBox>
<asp:Button ID="btnChangeDate" runat="server" Text="..."  ValidationGroup="date" 
    Width="30px" onclick="btnChangeDate_Click" />
<table cellspacing="0" cellpadding="0" width="20%" border="0" runat="server" id="dTable">
    <tbody>
        <tr>
            <td align="left" bgcolor="#cccccc">
                <asp:DropDownList ID="drpCalMonth" runat="Server" AutoPostBack="True" CssClass="calTitle"
                    Width="100px">
                </asp:DropDownList>
            </td>
            <td align="right" bgcolor="#cccccc">
                <asp:DropDownList ID="drpCalYear" runat="Server" AutoPostBack="True" CssClass="calTitle"
                    Width="100px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Calendar OtherMonthDayStyle-BackColor="White" DayStyle-BackColor="LightYellow"
                    ID="myCalendar" runat="Server" CssClass="calBody" DayHeaderStyle-BackColor="#eeeeee"
                    Width="100%" FirstDayOfWeek="Monday" 
                    onselectionchanged="myCalendar_SelectionChanged"></asp:Calendar>
            </td>
        </tr>
    </tbody>
</table>
