<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="GemsWeb._default" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <div style="float: left; width: 200;">
    <h3>
            <a href="#">List of Events</a></h3>
        <asp:ListBox ID="lstEvent" runat="server" Height="420px" Width="200px" AutoPostBack="True">
        </asp:ListBox>
    </div>
    <div style="float: left;">
        <h3>
            <a href="#">Please Select the Date Range</a></h3>
        <table>
            <tr>
                <td>
                    From
                </td>
                <td>
                    <GEMS:DatePicker ID="dpFrom" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    To
                </td>
                <td>
                    <GEMS:DatePicker ID="dpTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search Events" 
                        onclick="btnSearch_Click" />
                </td>
            </tr>
        </table>
        <!--Add a MultiView control to "contain" View controls which will serve as tab pages.-->
        <asp:Menu ID="menuEvent" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
            OnMenuItemClick="menuEvent_MenuItemClick" Height="30px" Width="560px" ForeColor="#666666"
            BackColor="#E3EAEB" Font-Bold="True" Font-Underline="True" DynamicHorizontalOffset="2"
            Font-Names="Verdana" Font-Size="0.8em" StaticSubMenuIndent="10px">
            <Items>
                <asp:MenuItem Value="0" Text="Event Information"></asp:MenuItem>
                <asp:MenuItem Value="1" Text="Event's Programme"></asp:MenuItem>
                <asp:MenuItem Value="2" Text="Guests for the Event"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="mvTab" runat="server" ActiveViewIndex="0">
            <!--Add View controls, one for each 'tab'-->
            <asp:View ID="tab0" runat="server" >
                <!-- Event information -->
                (Event.Information)
            </asp:View>
            <asp:View ID="tab1" runat="server" >
                <!-- Program information -->
                (Event.Programs)
            </asp:View>
            <asp:View ID="tab2" runat="server">
                <!-- Guest Information information -->
                (Event.Guests)
            </asp:View>
        </asp:MultiView>
        <br />
        <br />
        <!-- Adjust the navigate URL accordingly based on the event ID (selected index change event
            to go to register page set to (~/Register.aspx?eventID=x) -->
        <asp:HyperLink ID="hypRegister" NavigateUrl="~/Register.aspx" runat="server">Register Now</asp:HyperLink>
    </div>
</asp:Content>
