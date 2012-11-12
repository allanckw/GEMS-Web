<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ParticipantEvents.aspx.cs" Inherits="GemsWeb.ParticipantEvents" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="float: left; width: 100%;">
                        <h3>
                            <a href="#">Search Registered Events and Make Payments (if any)</a></h3>
                        <table>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="180px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator8" runat="server" ErrorMessage="E-mail required" ControlToValidate="txtEmail"
                                        Visible="true"> * </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                                        Display="Dynamic" Visible="true" runat="server" ForeColor="Red"> Invalid E-mail Format </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>From:</b>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdlstFromDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rdlstFromDateRange_SelectedIndexChanged">
                                        <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <GEMS:DatePicker ID="dpFrom" runat="server" Visible="True" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>To:</b>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdlstToDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rdlstToDateRange_SelectedIndexChanged">
                                        <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                        <asp:ListItem Value="30" Selected="True">Next 1 Month</asp:ListItem>
                                        <asp:ListItem Value="90">Next 3 Month</asp:ListItem>
                                        <asp:ListItem Value="365">Next Year</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <GEMS:DatePicker ID="dpTo" runat="server" Visible="True" />
                                    <br />
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
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <br />
        <div>
            <asp:Label ID="lblNoEvents" runat="server" ForeColor="Red" Text="No records were found for the corresponding e-mail address and timeframe"></asp:Label>
            <asp:Panel ID="pnlEvents" runat="server" Visible="false">
                <h3>
                    All Registered Events For Email Address</h3>
                <h4>
                    Events that Requires Payment</h4>
                <asp:GridView ID="gvUnpaid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    AllowPaging="True" PageSize="5" Width="100%" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" OnPageIndexChanging="gv_PageIndexChanging" BorderWidth="1px"
                    OnRowCommand="gvGoods_RowCommand" GridLines="Horizontal" EmptyDataText="There are no events to display"
                    DataKeyNames="EventID" ForeColor="Black">
                    <Columns>
                        <asp:BoundField DataField="EventID" HeaderText="Event ID" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventName" HeaderText="Event Name" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventCost" HeaderText="Price " HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventStartDate" HeaderText="Start Date " DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventEndDate" HeaderText="End Date " DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:ButtonField ButtonType="Image" CommandName="AddToBasket" ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_cart_LG.gif"
                            HeaderStyle-HorizontalAlign="Left" Text="Add to Cart" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle Width="95%" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#242121" />
                </asp:GridView>
                <br />
                <asp:ImageButton ID="btnViewCart" runat="server" ImageUrl="~/images/viewcart.jpeg"
                    Style="float: right;" OnClick="btnViewCart_Click" />
                <br />
                <h4>
                    Paid Events / Events that does not required Payment</h4>
                <asp:GridView ID="gvPaid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    AllowPaging="True" PageSize="5" Width="100%" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" OnPageIndexChanging="gv_PageIndexChanging" BorderWidth="1px"
                    GridLines="Horizontal" EmptyDataText="There are no events to display" DataKeyNames="EventID"
                    ForeColor="Black">
                    <Columns>
                        <asp:BoundField DataField="EventID" HeaderText="Event ID" HeaderStyle-HorizontalAlign="Left"
                            Visible="false" />
                        <asp:BoundField DataField="EventName" HeaderText="Event Name" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventCost" HeaderText="Price " HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventStartDate" HeaderText="Start Date " DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="EventEndDate" HeaderText="End Date " DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-HorizontalAlign="Left" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle Width="95%" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#242121" />
                </asp:GridView>
            </asp:Panel>
        </div>
        <br /><br /><br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ViewPastTrans.aspx">View Past Transactions</asp:HyperLink>
        <br />
    </div>
</asp:Content>
