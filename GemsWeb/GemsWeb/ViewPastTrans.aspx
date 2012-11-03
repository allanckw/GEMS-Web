<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewPastTrans.aspx.cs" Inherits="GemsWeb.ViewPastTrans" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <b>Search Past Transaction</b>
                        <br />
                        <b>From:</b><br />
                        <asp:RadioButtonList ID="rdlstFromDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rdlstFromDateRange_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                            <asp:ListItem Value="30" Selected="True">Last 1 Month</asp:ListItem>
                            <asp:ListItem Value="90">Last 3 Month</asp:ListItem>
                            <asp:ListItem Value="365">Last Year</asp:ListItem>
                        </asp:RadioButtonList>
                        <GEMS:DatePicker ID="dpFrom" MonthsFromCurrent="-1" runat="server" Visible="True" />
                        <br />
                        <b>To:</b><br />
                        <asp:RadioButtonList ID="rdlstToDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rdlstToDateRange_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                        </asp:RadioButtonList>
                        <GEMS:DatePicker ID="dpTo" runat="server" Visible="True" />
                        <br />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvPastTransaction" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="None" EmptyDataText="No Past Transaction Available"
                            Font-Size="X-Small" AllowPaging="True" PageSize="20">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Date & Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("TransactionDateTime").ToString().Trim() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="120px" HeaderText="TransactionID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploader" runat="server" Text='<%# Eval("TransactionID").ToString().Trim() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="120px" HeaderText="Event Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransEvent" runat="server" Text='<%# getEventName(Eval("EventID").ToString().Trim()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Amount">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblTransAmt" runat="server" Text='<%# currencyFormat(Eval("Amount").ToString().Trim()) %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransRemark" runat="server" Text='<%# Eval("Remarks").ToString().Trim() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
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
