<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Event.aspx.cs" Inherits="GemsWeb.Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlEventDay" />
        </Triggers>
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
            <!--Add a MultiView control to "contain" View controls which will serve as tab pages.-->
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
                    <!-- Program information -->
                    <asp:Menu ID="menu1" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                        OnMenuItemClick="menuEvent_MenuItemClick" Height="30px" Width="560px" ForeColor="#666666"
                        BackColor="#E3EAEB" Font-Bold="True" Font-Underline="True" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" StaticSubMenuIndent="10px">
                        <Items>
                            <asp:MenuItem Value="0" Text="Event's Programme"></asp:MenuItem>
                            <asp:MenuItem Value="1" Text="Guests for the Event"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                    <asp:MultiView ID="mvProgramGuest" runat="server" ActiveViewIndex="0">
                        <asp:View ID="mvPGtab0" runat="server">
                            <h2>
                                Programme Information</h2>
                            <asp:Repeater ID="rptProgramme" runat="server">
                                <ItemTemplate>
                                    <div class="newsItem">
                                        <b>
                                            <asp:Label ID="Label1" runat="server" Text="Progam Name: " ForeColor="#0000CC">                                                                 
                                            <%# DataBinder.Eval(Container, "DataItem.Name")%>
                                            </asp:Label>
                                        </b>
                                        <br />
                                        <b>From</b>
                                        <%# DateTimeToCustomString((DateTime)Eval("StartDateTime"))%>
                                        <b>To</b>
                                        <%# DateTimeToCustomString((DateTime)Eval("EndDateTime"))%>
                                        <br />
                                        Description:<br />
                                        <%# Eval("Description").ToString().Trim()%>
                                        <br />
                                        Location:
                                        <%# Eval("Location").ToString().Trim()%>
                                        <br />
                                        <br />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:View>
                        <asp:View ID="mvPGtab1" runat="server">
                            <h2>
                                Guest Information</h2>
                            <asp:Repeater ID="rtpGuest" runat="server">
                                <ItemTemplate>
                                    <div class="newsItem">
                                        <b>
                                            <asp:Label ID="Label1" runat="server" Text="Guest Name: " ForeColor="#0000CC">                                                                 
                                            <%# DataBinder.Eval(Container, "DataItem.Name")%>
                                            </asp:Label>
                                        </b>
                                        <br />
                                        Description:<br />
                                        <%# Eval("Description")%>
                                        <br />
                                        <br />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:View>
                    </asp:MultiView>
                    <br />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
