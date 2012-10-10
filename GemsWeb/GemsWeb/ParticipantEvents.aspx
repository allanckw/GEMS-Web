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
                                <td>
                                    Email:
                                </td>
                                <td style="height: 22px; width: 422px;">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="180px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator8" runat="server" ErrorMessage="E-mail required" ControlToValidate="txtEmail"
                                        Visible="true"> * </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                                        Display="Dynamic" Visible="true" runat="server" ForeColor="Red"> Invalid E-mail Format </asp:RegularExpressionValidator>
                                </td>
                            </tr>
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
                                    <asp:Button ID="btnSearch" runat="server" Text="Search Events" OnClick="btnSearch_Click"
                                        Height="30px" Width="160px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:Label ID="lblNoEvents" runat="server" ForeColor="Red" Text="No records were found for the corresponding e=mail address and timeframe"></asp:Label>
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
                        <asp:BoundField DataField="EventID" HeaderText="Event ID" />
                        <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                        <asp:BoundField DataField="EventCost" HeaderText="Price " />
                        <asp:BoundField DataField="EventStartDate" HeaderText="Start Date " DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="EventEndDate" HeaderText="End Date " DataFormatString="{0:dd MMM yyyy}" />
                        <asp:ButtonField ButtonType="Image" CommandName="AddToBasket" ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_cart_LG.gif"
                            Text="Add to Cart" />
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
                    Style="float: right;" onclick="btnViewCart_Click" />
                <br />
                <hr />
                <br />
                <h4>
                    Paid Events / Events that does not required Payment</h4>
                <asp:GridView ID="gvPaid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    AllowPaging="True" PageSize="5" Width="100%" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" OnPageIndexChanging="gv_PageIndexChanging" BorderWidth="1px"
                    GridLines="Horizontal" EmptyDataText="There are no events to display" 
                    DataKeyNames="EventID" ForeColor="Black">
                    <Columns>
                        <asp:BoundField DataField="EventID" HeaderText="Event ID" />
                        <asp:BoundField DataField="EventName" HeaderText="Event Name" />
                        <asp:BoundField DataField="EventCost" HeaderText="Price " />
                        <asp:BoundField DataField="EventStartDate" HeaderText="Start Date " DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="EventEndDate" HeaderText="End Date " DataFormatString="{0:dd MMM yyyy}" />
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
            <br />
            <br />
        </div>
    </div>
</asp:Content>
