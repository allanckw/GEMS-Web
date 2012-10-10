<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="GemsWeb.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""><h2>Please Log in to access Content</h2></asp:Label>
    <%--<div id="login-box">
        <asp:Login ID="Login2" runat="server">
            <LayoutTemplate>
                <h2>
                    Sign in</h2>
                <br />
                <div class="login-box-name" style="margin-top: 20px;">
                    <b>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User ID:</asp:Label></b>&nbsp;&nbsp;
                </div>
                <div class="login-box-field" style="margin-top: 15px;">
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ForeColor="White"
                        ValidationGroup="Login1" Font-Bold="True" Font-Size="Large">*</asp:RequiredFieldValidator>
                </div>
                <br />
                <br />
                <div class="login-box-name">
                    <b>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label></b>&nbsp;&nbsp;
                </div>
                <br />
                <div class="login-box-field">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ForeColor="White"
                        ValidationGroup="Login1" Font-Bold="True" Font-Size="Large">*</asp:RequiredFieldValidator>
                </div>
                <br />
                <br />
                <div class="login-box-name">
                    <b>
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlDomain">Domain:</asp:Label></b>&nbsp;&nbsp;
                </div>
                <br />
                <div class="login-box-field">
                    <asp:DropDownList ID="ddlDomain" runat="server" Width="140px">
                        <asp:ListItem Selected="True">NUSNET</asp:ListItem>
                        <asp:ListItem>Requestees</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <br />
                <span class="login-box-options"><span class="login-box-options">
                    <br />
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                    <br />
                    <asp:ImageButton ID="LoginButton" runat="server" CommandName="Login" Text="Sign In"
                        ValidationGroup="Login1" ImageUrl="~/images/login-btn.png" Style="float: right;" />
                </div>
            </LayoutTemplate>
        </asp:Login>
    </div>--%>
    <div id="login-box">
        <asp:Login ID="Login2" runat="server">
            <LayoutTemplate>
                <h2>
                    Sign in</h2>
                <br />
                <div class="login-box-name" style="margin-top: 20px;">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User ID:&nbsp;</asp:Label>
                </div>
                <div class="login-box-field" style="margin-top: 15px;">
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ForeColor="White"
                        ValidationGroup="Login1" Font-Bold="True" Font-Size="Large">*</asp:RequiredFieldValidator>
                </div>
                <br />
                <br />
                <div class="login-box-name">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:&nbsp;</asp:Label>
                </div>
                <br />
                <div class="login-box-field">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ForeColor="White"
                        ValidationGroup="Login1" Font-Bold="True" Font-Size="Large">*</asp:RequiredFieldValidator>
                </div>
                <br />
                <br />
                <div class="login-box-name">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlDomain">Domain:&nbsp; </asp:Label>
                </div>
                <br />
                <div class="login-box-field">
                    <asp:DropDownList ID="ddlDomain" runat="server" Width="140px">
                        <asp:ListItem Selected="True">NUSNET</asp:ListItem>
                        <asp:ListItem>Requestees</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <br />
                <span class="login-box-options"><span class="login-box-options">
                    <br />
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                    <br />
                    <asp:ImageButton ID="LoginButton" runat="server" CommandName="Login" Text="Sign In"
                        ValidationGroup="Login1" ImageUrl="~/images/login-btn.png" Style="float: right;" />
                </div>
            </LayoutTemplate>
        </asp:Login>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/forgetPassword.aspx"
            ForeColor="White" Style="float: right;">Forget Password?</asp:HyperLink>
    </div>
</asp:Content>
