<%@ Page Title="<%$ Resources:LocalizedText,ChangePassword %>" Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="EDC.Pages.Account.ChangePassword" %>

<!DOCTYPE html>
<html lang="ru" style="height: 100%; background-color: #fff">
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
<body style="height: 100%" class="loginBody">
    <form runat="server">
        <div class="LoginWindow">
            <asp:Image runat="server" ImageUrl="~/Images/mdplogo2.png" CssClass="logoImgLogin" />
            <p class="validation-summary-errors loginError">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
            <asp:ScriptManager runat="server"/>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Timer Interval="2000" runat="server" Enabled="false" ID="timerRedirect" OnTick="timerRedirect_Tick"/>
                </ContentTemplate>
            </asp:UpdatePanel>
            <p runat="server" id="pLOK" style="color:#7ac0da;font-weight: bold;font-size: 1em; text-align:center; display:none;">
                <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PasswordWasChangedSuccessfully %>" />
            </p>
            <fieldset runat="server" id="fsLoginForm" class="loginForm">
                <ol class="loginInfo">
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbOldPassword"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,CurrentPassword %>"/></asp:Label>
                        <asp:TextBox runat="server" ID="tbOldPassword" TextMode="Password" />
                        </br>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbOldPassword" CssClass="field-validation-error align-middle" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbNewPassword"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,NewPassword %>"/></asp:Label>
                        <asp:TextBox runat="server" ID="tbNewPassword" TextMode="Password" />
                        </br>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbNewPassword" CssClass="field-validation-error align-middle" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbConfirmPassword"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PasswordConfirmation %>"/></asp:Label>
                        <asp:TextBox runat="server" ID="tbConfirmPassword" TextMode="Password" />
                        </br>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbConfirmPassword" CssClass="field-validation-error align-middle" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" />
                    </li>
                    <li>
                        <asp:Button ID="btnChangePassword" runat="server" Text="<%$ Resources:LocalizedText,ChangePassword %>" CssClass="loginButton" OnClick="btnChangePassword_Click" />
                    </li>
                </ol>
            </fieldset>
        </div>
    </form>
</body>
</html>

