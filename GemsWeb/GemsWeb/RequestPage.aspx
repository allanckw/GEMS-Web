<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestPage.aspx.cs" Inherits="GemsWeb.RequestPage" %>

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
                            <table border="0">
                                <tr style="height: 40%">
                                    <td rowspan="2" align="left" valign="top" style="width: 30%">
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
                                    </td>
                                    <td align="left" valign="top" style="width: 45%;">
                                        <asp:Label ID="Label2" runat="server" Text="Email To" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtToWho" Width="95%" runat="server" MaxLength="150"></asp:TextBox><br />
                                        <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtToWho" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                                            Display="Dynamic" Visible="true" runat="server" ForeColor="Red"> Invalid E-mail Format </asp:RegularExpressionValidator>
                                        <asp:HiddenField ID="hidRequestID" runat="server" />
                                        <asp:Label ID="Label7" runat="server" Text="Title" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtRequestTitle" Width="95%" runat="server"></asp:TextBox><br />
                                        <asp:Label ID="Label8" runat="server" Text="Description" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtRequestDesc" runat="server" Style="resize: none;" Height="150px"
                                            MaxLength="999" TextMode="MultiLine" Width="97%" Font-Bold="True"></asp:TextBox><br />
                                        <asp:Label ID="Label9" runat="server" Text="URL" Width="30%" Font-Bold="true" /><br />
                                        <asp:TextBox ID="txtFileUrl" Width="95%" runat="server"></asp:TextBox>

                                        <asp:RegularExpressionValidator ID="regexFileURL" ControlToValidate="txtFileUrl"
                                            runat="server" ValidationExpression="^[\s\S]{0,2000}$" Text="2000 characters max" />
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="Label3" runat="server" Text="Event Name" Font-Bold="True"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblEventName" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Label4" runat="server" Text="Event Date" Font-Bold="True"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblFrmDate" runat="server" Text=""></asp:Label>
                                        &nbsp;<asp:Label ID="Label5" runat="server" Text="To" Font-Bold="True" Font-Size="X-Small"></asp:Label>
                                        &nbsp;<asp:Label ID="lblToDate" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Label6" runat="server" Text="Description" Font-Bold="True"></asp:Label>
                                        &nbsp;<br />
                                        <asp:TextBox ID="txtEventDesc" runat="server" Style="resize: none;" ReadOnly="true"
                                            TextMode="MultiLine" Width="200px" Font-Bold="True" BorderColor="Transparent"
                                            Height="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                <td align="right">
                                <asp:Button ID="btnRequestNew" runat="server" Text="New Request" OnClick="btnRequestNew_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnRequestSave" runat="server" Text="Add/Update" OnClick="btnRequestSave_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btnRequestCancel" runat="server" Text="Cancel" OnClick="btnRequestCancel_Click" />&nbsp;&nbsp;
                                        <br />
                                        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Font-Size="X-Small"
                                            Font-Bold="True">
                                        </asp:Label>
                                </td>
                                <td></td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="left" valign="top" style="width: 30%">
                                        <b>List of Request</b>
                                        <br />
                                        <asp:ListBox ID="lstRequest" AutoPostBack="true" Width="280px" Height="300px" runat="server"
                                            OnSelectedIndexChanged="lstRequest_SelectedIndexChanged"></asp:ListBox>
                                    </td>
                                    <td align="left" valign="top">
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
                            <asp:PostBackTrigger ControlID="btnRequestSave" />
                            <asp:PostBackTrigger ControlID="btnRequestCancel" />
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
