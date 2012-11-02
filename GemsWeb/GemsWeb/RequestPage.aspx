<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestPage.aspx.cs" Inherits="GemsWeb.RequestPage" %>

<!DOCTYPE html>
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
                            <div id="Div1" style="float: left; width: 100%;">
                                <table border="1">
                                    <tr>
                                        <td rowspan="5" align="left" valign="top" style="width: 200px">
                                            List of Request
                                            <br />
                                            <asp:ListBox ID="lstRequest" AutoPostBack="true" Width="200px" Height="500px" runat="server">
                                            </asp:ListBox>
                                        </td>
                                        <td style="width: 100px; height: 20px">
                                            Title
                                        </td>
                                        <td style="width: 300px">
                                            <asp:TextBox ID="txtRequestTitle" Width="300px" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px">
                                            Description
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtRequestDesc" runat="server" TextMode="MultiLine" Width="500px"
                                                Height="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
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
