<%@ Page Title="<%$ Resources:LocalizedText,Login %>" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EDC.Account.Login" %>

<!DOCTYPE html>
<html style="height: 100%; background-color: #fff">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title><%: Page.Title %></title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/Content/Site.css" rel="stylesheet" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
</head>
<body style="height:100%" class="loginBody">
    <form runat="server">

        <div class="LoginWindow">
            <hgroup>
            </hgroup>
            <asp:Image runat="server" ImageUrl="~/Images/mdplogo2.png" CssClass="logoImgLogin" />
            <asp:Login runat="server" ID="LoginControl" ViewStateMode="Disabled" DisplayRememberMe="false" RenderOuterTable="false" OnLoggingIn="Unnamed_LoggingIn">
                <LayoutTemplate>
                    <p class="validation-summary-errors loginError">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                    <fieldset class="loginForm">
                        <legend>Форма входа</legend>
                        <ol class="loginInfo">
                            <li>
                                <asp:Label runat="server" AssociatedControlID="UserName"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,UserName %>"/></asp:Label>
                                <asp:TextBox runat="server" ID="UserName" />
                                </br>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error align-middle" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" />
                            </li>
                            <li>
                                <asp:Label runat="server" AssociatedControlID="Password"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Password %>"/></asp:Label>
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                </br>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error align-middle" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" />
                            </li>
                            <li>
                                </br>
                                <asp:Button runat="server" CommandName="Login" Text="<%$ Resources:LocalizedText,Log_in %>" CssClass="loginButton" />
                            </li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlPRecovery" Text="<%$ Resources:LocalizedText,ForgotPassword %>" NavigateUrl="~/Pages/Account/PasswordRecovery.aspx" />
                            </li>
                        </ol>

                    </fieldset>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </form>
</body>
</html>
