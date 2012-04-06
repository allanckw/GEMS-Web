<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="GemsWeb._default" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <div style="float: left; width: 200;">
    <h3>
            <a href="#">List of Events</a></h3>
        <asp:ListBox ID="lstEvent" runat="server" Height="420px" Width="200px" 
            AutoPostBack="True" onselectedindexchanged="lstEvent_SelectedIndexChanged">
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
                <asp:Label ID="eventName" Width="100px" runat="server" Text="Name: "/><asp:Label ID="lbleventname" runat="server"/><br />
                <asp:Label ID="eventDate" Width="100px" runat="server" Text="Date: "/><asp:Label ID="lbleventdate" runat="server"/><br />
                <asp:Label ID="eventStartTime" Width="100px" runat="server" Text="Start Time: "/><asp:Label ID="lbleventstarttime" runat="server"/><br />
                <asp:Label ID="eventEndtime" Width="100px" runat="server" Text="End Time: "/><asp:Label ID="lbleventendtime" runat="server"/><br />
                <asp:Label ID="eventDescription" Width="100px" runat="server" Text="Description: "/><asp:Label ID="lbleventdescription" runat="server"/><br />
                <asp:Label ID="eventwebsite" Width="100px" runat="server" Text="Website: "/><asp:HyperLink ID="hypeventwebsite" runat="server" /><br />
                <asp:Label ID="eventPublishInfo" Width="100px" runat="server" Text="Publication Remarks : "/><asp:Label ID="lbleventpublishinfo" runat="server"/><br />
                
            </asp:View>
            <asp:View ID="tab1" runat="server" >
                <!-- Program information -->
                <asp:GridView ID="gvProgram" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="None" EmptyDataText="No Programmes Added">
               
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" >
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StartDateTime" HeaderText="Start Time" 
                            DataFormatString=" {0:HH:mm}" >
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EndDateTime" HeaderText="End Time" 
                            DataFormatString=" {0:HH:mm}" >
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description" >
                        <HeaderStyle Width="250px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
               
                </asp:GridView>
            </asp:View>
            <asp:View ID="tab2" runat="server">
                <!-- Guest Information information -->
                
                <asp:GridView ID="gvGuest" runat="server" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" AutoGenerateColumns="False" EmptyDataText="No Guest Added">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" >
                        <HeaderStyle Width="150px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description" >
                        <HeaderStyle Width="400px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </asp:View>
        </asp:MultiView>
        <br />
        <br />
        <!-- Adjust the navigate URL accordingly based on the event ID (selected index change event
            to go to register page set to (~/Register.aspx?eventID=x) -->
                   <%-- <asp:HyperLink ID="hypRegister"  NavigateUrl="~/Register.aspx?EventId=6&EventName=haha"  runat="server">Register Now</asp:HyperLink>
--%>

 <asp:HyperLink ID="hypRegister"  NavigateUrl="#" Text="Register Now"  runat="server" />

    </div>
</asp:Content>
