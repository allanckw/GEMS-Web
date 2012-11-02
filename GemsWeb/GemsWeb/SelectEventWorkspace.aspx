<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SelectEventWorkspace.aspx.cs" Inherits="GemsWeb.SelectEventWorkspace" %>

<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="GEMS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="float: left; width: 600px">
                <h3>
                    Please Select the Date Range to View Workspace</h3>
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
                        <h4>From</h4>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdlstFromDateRange" runat="server" AutoPostBack="true" 
                                RepeatDirection="Horizontal" 
                                onselectedindexchanged="rdlstFromDateRange_SelectedIndexChanged">
                                <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                <asp:ListItem Value="30" Selected="True">Last 1 Month</asp:ListItem>
                                <asp:ListItem Value="90">Last 3 Month</asp:ListItem>
                                <asp:ListItem Value="365">Last Year</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                           <GEMS:DatePicker ID="dpFrom" runat="server" DisplayFutureDate="true" 
                                Visible="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <h4>To</h4>
                        </td>
                        <td>
                             <asp:RadioButtonList ID="rdlstToDateRange" runat="server" AutoPostBack="true" 
                                 RepeatDirection="Horizontal" 
                                 onselectedindexchanged="rdlstToDateRange_SelectedIndexChanged">
                                <asp:ListItem Value="-1">Custom Date Range </asp:ListItem>
                                <asp:ListItem Value="30" Selected="True">Next 1 Month</asp:ListItem>
                                <asp:ListItem Value="90">Next 3 Month</asp:ListItem>
                                <asp:ListItem Value="365">Next Year</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <GEMS:DatePicker ID="dpTo" runat="server" MonthsFromCurrent="1" 
                                DisplayFutureDate="true" Visible="True"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>Tag</h4>
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
                                List of Event WorkSpace</h3>
                            <%--                            <asp:ListBox ID="lstEvent" runat="server" Height="420px" Width="500px" OnSelectedIndexChanged="lstEvent_SelectedIndexChanged"
                                AutoPostBack="True"></asp:ListBox>--%>
                            <asp:Repeater ID="rtpEvent" runat="server">
                                <ItemTemplate>
                                    <div class="newsItem">
                                        <b>
                                            <asp:Label ID="Label1" runat="server" Text="Event Name: " ForeColor="#0000CC">                                                                 
                                            <%# DataBinder.Eval(Container, "DataItem.Name")%>
                                            </asp:Label>
                                        </b>
                                        <br />
                                        From
                                        <%# DateToCustomString((DateTime)Eval("StartDateTime")) %>
                                        &nbsp; To
                                        <%# DateToCustomString((DateTime)Eval("EndDateTime")) %>
                                        <br />
                                        <%--'<%# Eval("studentID", "StudentDetails.aspx?id={0}") %>'--%>
                                        <asp:HyperLink ID="lnkArtefact" runat="server" Target="_blank" NavigateUrl='<%# Eval("EventID", "ArtefactBin.aspx?EventID={0}") %>'>View Event Workspace</asp:HyperLink>
                                        <%--<asp:LinkButton ID="ArtefactBinLink" runat="server" OnClick="window.open('ArtefactBin.aspx?EventID=" + <%# DataBinder.Eval("DataItem.EventID")%> + "', 'ArtefactBin','left=250px, top=245px, width=1100px, height=650px, directories=no, scrollbars=yes, status=no, resizable=no');return false;">View Event Workspace</asp:LinkButton>--%>
                                        <br />
                                        <br />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
                <%--<asp:LinkButton ID="ArtefactBinLink" runat="server" OnClientClick="window.document.forms[0].target='_blank';" >View Event Workspace</asp:LinkButton>--%>
                <%--<asp:Button ID="btnGO" runat="server" Text="View Event Workspace" Height="30px" OnClick="btnGO_Click"
                    Width="160px" />--%>
            </div>
        </ContentTemplate>
        <%--        <Triggers>
            <asp:PostBackTrigger ControlID="ArtefactBinLink" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
