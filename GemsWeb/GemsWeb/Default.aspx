<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="GemsWeb._default" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="float: left; width: 600px">
                <h3>
                    <a href="#">Please Select the Date Range</a></h3>
                <table>
                    <tr>
                        <td style="width: 20%">
                        </td>
                        <td style="width: 80%">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>
                                From</h4>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlstFromDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rdlstFromDateRange_SelectedIndexChanged" Font-Size="Small">
                                <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <GEMS:DatePicker ID="dpFrom" runat="server" DisplayFutureDate="true" Visible="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>
                                To</h4>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlstToDateRange" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rdlstToDateRange_SelectedIndexChanged" Font-Size="Small">
                                <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                <asp:ListItem Value="30" Selected="True">Next Month</asp:ListItem>
                                <asp:ListItem Value="90">Next 3 Month</asp:ListItem>
                                <asp:ListItem Value="365">Next Year</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <GEMS:DatePicker ID="dpTo" runat="server" MonthsFromCurrent="1" DisplayFutureDate="true"
                                Visible="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>
                                Tag</h4>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTag" runat="server" Width="200px" />
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
                    <tr>
                        <td colspan="2">
                            <h3>
                                List of Events</h3>
                            <asp:Repeater ID="rtpEvent" runat="server">
                                <ItemTemplate>
                                    <div class="newsItem">
                                        <b><a href="#">
                                            <asp:Label ID="Label1" runat="server" Text="" ForeColor="#0000CC">                                                                 
                                            <%# DataBinder.Eval(Container, "DataItem.Name")%>
                                            </asp:Label></a> </b>
                                        <br />
                                        From
                                        <%# DateToCustomString((DateTime)Eval("StartDateTime")) %>
                                        &nbsp; To
                                        <%# DateToCustomString((DateTime)Eval("EndDateTime")) %>
                                        <br />
                                        <asp:HyperLink ID="lnkArtefact" runat="server" Target="_blank" NavigateUrl='<%# Eval("EventID", "Event.aspx?EventID={0}") %>'>Click here to find out more!</asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
