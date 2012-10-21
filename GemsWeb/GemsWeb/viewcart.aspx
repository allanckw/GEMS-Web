<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="viewcart.aspx.cs" Inherits="GemsWeb.viewcart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <div>
        <!-- The GridView to display a list of goods in the current cart -->
        <asp:GridView ID="gvCarts" runat="server" DataSourceID="odsCarts" DataKeyNames="rec_id"
            CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" ShowFooter="True" OnDataBound="gvCarts_DataBound"
            Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
            GridLines="Horizontal">
            <Columns>
                <asp:BoundField DataField="Event_ID" HeaderText="Event ID" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="Event_Name" HeaderText="Event Name" HeaderStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="price" HeaderText="Price, $" HeaderStyle-HorizontalAlign="Left"/>
                <%--<asp:BoundField DataField="quantity" HeaderText="Quantity" />--%>
                <asp:ButtonField CommandName="Delete" ButtonType="Button" Text="Delete" HeaderText="Delete" HeaderStyle-HorizontalAlign="Left"
                    ControlStyle-CssClass="button">
                    <ControlStyle CssClass="button"></ControlStyle>
                </asp:ButtonField>
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
        <asp:Panel ID="pnlPayPal" runat="server" Width="100%">
            <br />
            <asp:ImageButton ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif"
                Style="margin-right: 7px; float: right;" runat="server" ID="btnPayPal" 
                onclick="btnPayPal_Click" />
        </asp:Panel>
        <!-- The ObjectDataSource to work with the current cart -->
        <asp:ObjectDataSource ID="odsCarts" runat="server" TypeName="GemsWeb.Controllers.Carts"
            SelectMethod="LoadCart" DeleteMethod="Delete">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="1" Name="cart_id" QueryStringField="cart_id"
                    Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="rec_id" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
