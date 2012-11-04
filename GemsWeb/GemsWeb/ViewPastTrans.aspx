<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewPastTrans.aspx.cs" Inherits="GemsWeb.ViewPastTrans" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h3>
                <a href="#">View Past Payment Transactions</a></h3>
            <br />
            <table>
                <tr>
                    <td>
                        <b>From:</b>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdlstFromDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rdlstFromDateRange_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                            <asp:ListItem Value="30" Selected="True">Last 1 Month</asp:ListItem>
                            <asp:ListItem Value="90">Last 3 Month</asp:ListItem>
                            <asp:ListItem Value="365">Last Year</asp:ListItem>
                        </asp:RadioButtonList>
                        <GEMS:DatePicker ID="dpFrom" MonthsFromCurrent="-1" runat="server" Visible="True" />
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
                            <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                        </asp:RadioButtonList>
                        <GEMS:DatePicker ID="dpTo" runat="server" Visible="True" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        List of Transactions
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvPastTransaction" runat="server" AutoGenerateColumns="False" CellPadding="2"
                            ForeColor="Black"  BackColor="White" EmptyDataText="There are no transaction to display"
                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" GridLines="Horizontal" Width="125%"
                            Font-Size="Small">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Date & Time" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("TransactionDateTime").ToString().Trim() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="120px" HeaderText="TransactionID" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploader" runat="server" Text='<%# Eval("TransactionID").ToString().Trim() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="120px" HeaderText="Event Name" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransEvent" runat="server" Text='<%# getEventName(Eval("EventID").ToString().Trim()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransAmt" runat="server" Text='<%# currencyFormat(Eval("Amount").ToString().Trim()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
