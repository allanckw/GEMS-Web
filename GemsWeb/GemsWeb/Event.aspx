<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Event.aspx.cs" Inherits="GemsWeb.Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--Add a MultiView control to "contain" View controls which will serve as tab pages.-->
            <asp:Menu ID="menuEvent" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                OnMenuItemClick="menuEvent_MenuItemClick" Height="30px" Width="560px" ForeColor="#666666"
                BackColor="#E3EAEB" Font-Bold="True" Font-Underline="True" DynamicHorizontalOffset="2"
                Font-Names="Verdana" Font-Size="0.8em" StaticSubMenuIndent="10px">
                <Items>
                    <asp:MenuItem Value="0" Text="Event Information"></asp:MenuItem>
                    <%--<asp:MenuItem Value="1" Text="Event's Day Information"></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
            <asp:MultiView ID="mvTab" runat="server" ActiveViewIndex="0">
                <!--Add View controls, one for each 'tab'-->
                <asp:View ID="tab0" runat="server">
                    <!-- Event information -->
                    <asp:Label ID="lblName" Width="100px" runat="server" Text="Name: " /><asp:Label
                        ID="lbleventname" runat="server" /><br />
                    <asp:Label ID="lblDate" Width="100px" runat="server" Text="Date: " /><asp:Label
                        ID="lbleventdate" runat="server" /><br />
                    <%--                    <asp:Label ID="eventStartTime" Width="100px" runat="server" Text="Start Time: " /><asp:Label
                        ID="lbleventstarttime" runat="server" /><br />
                    <asp:Label ID="eventEndtime" Width="100px" runat="server" Text="End Time: " /><asp:Label
                        ID="lbleventendtime" runat="server" /><br />--%>
                    <asp:Label ID="lblDesc" Width="100px" runat="server" Text="Description: " /><asp:Label
                        ID="lbleventdescription" runat="server" /><br />
                    <asp:Label ID="lblWebsite" Width="100px" runat="server" Text="Website: " /><asp:HyperLink
                        ID="hypeventwebsite" runat="server" /><br />
                    <asp:Label ID="lblPublish" Width="100px" runat="server" 
                        Text="Publication Remarks : " /><asp:Label
                        ID="lbleventpublishinfo" runat="server" /><br />

                        <br />
                    <!-- Program information -->
                    <asp:Label ID="LblEventDayLabel" Width="100px" runat="server" Text="Select a day :" />
                    <asp:DropDownList ID="ddlEventDay" Width="200px" runat="server" DataTextFormatString="{0:dd MMM yyyy}"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlEventDay_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    <br />
                    Programme Information<br />
                    <asp:GridView ID="gvProgram" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" EmptyDataText="No Programmes Added">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle Width="100px" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StartDateTime" HeaderText="Start Time" DataFormatString=" {0:HH:mm}">
                                <HeaderStyle Width="100px" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EndDateTime" HeaderText="End Time" DataFormatString=" {0:HH:mm}">
                                <HeaderStyle Width="100px" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description">
                                <HeaderStyle Width="250px" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Location" HeaderText="Location" />
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
                    <br />
                    Guest Information<asp:GridView ID="gvGuest" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" EmptyDataText="No Guest Added" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle Width="150px" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description">
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
<%--                <asp:View ID="tab1" runat="server">

                </asp:View>
                <asp:View ID="tab2" runat="server">
                    <!-- Guest Information information -->
                </asp:View>--%>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:HyperLink ID="hypRegister" NavigateUrl="#" Text="Register Now" runat="server" />
    <br />
    <h4>
        Share This Event</h4>
    Click on a platform you like to share
    <div id="share">
        <span class='st_facebook_large'></span><span class='st_twitter_large'></span><span
            class='st_blogger_large'></span><span class='st_sina_large'></span><span class='st_stumbleupon_large'>
            </span><span class='st_delicious_large'></span><span class='st_email_large'>
        </span><span class='st_sharethis_large'></span>
    </div>
</asp:Content>
