<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequesteePage.aspx.cs"
    Inherits="GemsWeb.RequesteePage" %>

<!DOCTYPE html>
<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<html>
<head>
    <title></title>
    <link rel="stylesheet" href="css/style3.css" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
    <div id="wrapper1">
        <div id="wrapper2">
            <div id="header">
                <asp:HyperLink NavigateUrl="~/default.aspx" runat="server" ID="hyperlink30"><h1>
                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    Event Management <sup>Beta</sup></h1></asp:HyperLink>
                &nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <div id="container">
                <p class="description">
                    General Events Management System
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </p>
                <div id="content2" style="float: left; width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table border="0" width="100%">
                                <tr style="height: 40%">
                                    <td rowspan="3" align="left" valign="top" style="width: 25%">
                                        <b>Request Filter</b>
                                        <br />
                                        <b>From:</b><br />
                                        <asp:RadioButtonList ID="rdlstFromDateRange" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                            OnSelectedIndexChanged="rdlstFromDateRange_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                            <asp:ListItem Value="30" Selected="True">Last 1 Month</asp:ListItem>
                                            <asp:ListItem Value="90">Last 3 Month</asp:ListItem>
                                            <asp:ListItem Value="365">Last Year</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <GEMS:DatePicker ID="dpFrom" MonthsFromCurrent="-1" runat="server" Visible="True" />
                                        <br />
                                        <b>To:</b><br />
                                        <asp:RadioButtonList ID="rdlstToDateRange" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                            OnSelectedIndexChanged="rdlstToDateRange_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <GEMS:DatePicker ID="dpTo" runat="server" Visible="True" />
                                        <br />
                                        <asp:Label ID="Label1" runat="server" Text="Status" Font-Bold="True" ForeColor="Black" />
                                        &nbsp;
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Button ID="btnSearch" runat="server" Text="Search Request" OnClick="btnSearch_Click"
                                            Height="30px" Width="200px" />
                                        <br />
                                        <b>List of Request</b>
                                        <br />
                                        <asp:ListBox ID="lstRequest" AutoPostBack="true" Width="90%" Height="300px" runat="server"
                                            OnSelectedIndexChanged="lstRequest_SelectedIndexChanged"></asp:ListBox>
                                    </td>
                                    <td rowspan="2" align="left" valign="top" style="width: 35%;">
                                        <asp:Label ID="Label2" runat="server" Text="Requestor" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtFrmWho" Width="97%" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:HiddenField ID="hidRequestID" runat="server" />
                                        <asp:Label ID="Label7" runat="server" Text="Title" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtRequestTitle" Width="97%" runat="server" ReadOnly="true"></asp:TextBox><br />
                                        <asp:Label ID="Label8" runat="server" Text="Description" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtRequestDesc" runat="server" Style="resize: none;" Height="150px"
                                            MaxLength="1000" TextMode="MultiLine" Width="99%" Font-Bold="True" ReadOnly="true"></asp:TextBox><br />
                                        <asp:HyperLink ID="hypLnkFileUrl" runat="server" Target="_blank">Click to View URL</asp:HyperLink>
                                    </td>
                                    <td rowspan="2" style="width: 5%">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top" style="width: 35%">
                                        <asp:Label ID="Label3" runat="server" Text="Remarks" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtRemarks" runat="server" Style="resize: none;" Height="150px"
                                            MaxLength="1000" TextMode="MultiLine" Width="99%" Font-Bold="True"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:DropDownList ID="ddlRequesteeStatus" runat="server" Width="150px">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                                <tr style="height: 40%">
                                    <td colspan="3" valign="top" align="left">
                                        <asp:Label ID="lblRequestLogLabel" runat="server" Visible="false" Text="Request Past Log"
                                            Font-Bold="True"></asp:Label>
                                        <asp:GridView ID="gvRequestLog" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" EmptyDataText="No Log Available" Font-Size="X-Small"
                                            AllowPaging="True" OnPageIndexChanging="gvRequestLog_OnPageIndexChanging" EnableSortingAndPagingCallbacks="true"
                                            PageSize="10">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="LogDatetime" HeaderText="Date" DataFormatString=" {0:dd MMM yyyy}">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LogDatetime" HeaderText="Time" DataFormatString=" {0:HH:mm}">
                                                    <HeaderStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                    <HeaderStyle Width="250px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Status" HeaderText="Status">
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                    <HeaderStyle Width="250px" />
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
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
                            <asp:PostBackTrigger ControlID="gvRequestLog" />
                            <asp:PostBackTrigger ControlID="btnSearch" />
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="footer">
        <div style="float: left;">
            &copy; 2012 Copyright CS3213 Software Systems Design (Team 01)
            <br />
            National University of Singapore AY2012/2013 Semester 1
        </div>
        <div style="float: right;">
            <table>
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/pp.gif" alt="Paypal" />
                    </td>
                    <td>
                        <img style="border: 0; width: 136px; height: 45px" src="http://jigsaw.w3.org/css-validator/images/vcss-blue"
                            alt="Valid CSS!" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- End Footer -->
    </form>
</body>
</html>
