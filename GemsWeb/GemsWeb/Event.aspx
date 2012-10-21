<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Event.aspx.cs" Inherits="GemsWeb.Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--Add a MultiView control to "contain" View controls which will serve as tab pages.-->
            <asp:Menu ID="menuEvent" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                OnMenuItemClick="menuEvent_MenuItemClick" Height="30px" Width="560px" ForeColor="#666666"
                BackColor="#E3EAEB" Font-Bold="True" Font-Underline="True" DynamicHorizontalOffset="2"
                Font-Names="Verdana" Font-Size="14" StaticSubMenuIndent="10px">
                <Items>
                    <asp:MenuItem Value="0" Text="Event Information"></asp:MenuItem>
                    <%--<asp:MenuItem Value="1" Text="Event's Day Information"></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
            <asp:MultiView ID="mvTab" runat="server" ActiveViewIndex="0">
                <!--Add View controls, one for each 'tab'-->
                <asp:View ID="tab0" runat="server">
                    <!-- Event information -->
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblName" runat="server" Text="Name: " Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbleventname" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="label2" runat="server" Text="Event Description: " Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbleventdescription" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblDate" runat="server" Text="Date: " Font-Bold="true" />
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbleventdate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblWebsite" runat="server" Text="Website: " Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:HyperLink ID="hypeventwebsite" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblPublish" Width="179px" runat="server" Text="Publication Remarks: "
                                    Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbleventpublishinfo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblpayment" runat="server" Text="Fees: " Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblpaymentinfo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="LblEventDayLabel" runat="server" Text="Select a day :" Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEventDay" Width="200px" runat="server" DataTextFormatString="{0:dd MMM yyyy}"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlEventDay_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <!-- Program information -->
                    Programme Information<br />
                    <asp:GridView ID="gvProgram" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" EmptyDataText="No Programmes Added">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StartDateTime" HeaderText="Start Time" DataFormatString=" {0:HH:mm}">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EndDateTime" HeaderText="End Time" DataFormatString=" {0:HH:mm}">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description">
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
    <h4>
        <asp:HyperLink ID="hypRegister" NavigateUrl="#" Text="Register For the Event Now!"
            runat="server" /></h4>
    <br />
    <br />
    <h5>
        <a href="#">Share This Event</a></h5>
    <div id="share">
        <span class='st_facebook_hcount' displaytext='Facebook'></span><span class='st_twitter_hcount'
            displaytext='Tweet'></span><span class='st_googleplus_hcount' displaytext='Google +'>
            </span><span class='st_sina_hcount' displaytext='Sina'></span><span class='st_email_hcount'
                displaytext='Email'></span>
    </div>
</asp:Content>
