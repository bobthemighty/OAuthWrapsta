<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<WrapClientRequest>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <h1><%= Resource.ClientName %> wants to access your address book.</h1>
    
    <p>The website <%= Resource.ClientName %> would like to be able to read and write to your address book.</p>
    <p>If you want to allow this request, enter your username and password below and press "Allow".</p>
    <p>To deny this request, press "Deny".</p>
    
    <form action="/auth/user-request" method="post">
        
        <input type="text" name="Username" value="username" />
        <input type="password" name="Password"  />
    
        <input type="submit" name="Authority" value="Allow" />
        <input type="submit" name="Authority" value="Deny" />
        
        <input type="hidden" name="scope" value="<%= Resource.Scope %>" />
        <input type="hidden" name="state" value="<%= Resource.State %>" />
        <input type="hidden" name="clientId" value="<%= Resource.ClientId %>" />
        <input type="hidden" name="callbackUri" value="<%= Resource.CallbackUri %>" />
    </form>
</body>
</html>
