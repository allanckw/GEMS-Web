<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paypal.aspx.cs" Inherits="GemsWeb.Paypal" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>
<body>
    <h1>
        <img src="images/Progress.gif" alt="loading..." />Please wait while we transfer
        you to PayPal...
    </h1>
    <form id="payForm" method="post" action="<% Response.Write(URL); %>">
    <input type="hidden" name="cmd" value="<% Response.Write(cmd);%>">
    <input type="hidden" name="business" value="<% Response.Write(business); %>">
    <input type="hidden" name="item_name" value="<% Response.Write(item_name); %>">
    <input type="hidden" name="amount" value="<% Response.Write(amount); %>">
    <input type="hidden" name="no_shipping" value="<% Response.Write(no_shipping); %>">
    <input type="hidden" name="return" value="<% Response.Write(return_url); %>">
    <input type="hidden" name="rm" value="<% Response.Write(rm); %>">
    <input type="hidden" name="notify_url" value="<% Response.Write(notify_url); %>">
    <input type="hidden" name="cancel_return" value="<% Response.Write(cancel_url); %>">
    <input type="hidden" name="currency_code" value="<% Response.Write(currency_code); %>">
    <input type="hidden" name="custom" value="<% Response.Write(request_id); %>">
    </form>
    <script type="text/javascript">
        document.forms["payForm"].submit();
    </script>
</body>
</html>
