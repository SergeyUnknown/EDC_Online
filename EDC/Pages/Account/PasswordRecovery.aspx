<%@ Page Title="<%$ Resources:LocalizedText,PasswordRecovery %>" Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="EDC.Pages.Account.PasswordRecovery" %>

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
<body style="height: 100%" class="loginBody">

    <form runat="server">
        <div class="LoginWindow">
            <asp:Image runat="server" ImageUrl="~/Images/mdplogo2.png" CssClass="logoImgLogin" />
            <p class="validation-summary-errors loginError" id="pFailureText"><asp:Literal runat="server" ID="FailureText" /></p>
            <asp:ScriptManager runat="server"/>

            <p runat="server" id="pLOK" style="color:#7ac0da;font-weight: bold;font-size: 1em; text-align:center; display:none;">
                <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PasswordHasBeenSentToEmail %>" />
            </p>
            <fieldset runat="server" id="fsLoginForm" class="loginForm">
                <ol class="loginInfo">
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbUserName"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,UserName %>"/></asp:Label>
                        <asp:TextBox runat="server" ID="tbUserName"/>
                        </br>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbUserName" CssClass="field-validation-error align-middle" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" />
                    </li>
                    <li id="li1">
                        <asp:Label runat="server" AssociatedControlID="tbQuestion"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Question %>"/></asp:Label>
                        <asp:TextBox runat="server" ID="tbQuestion" ReadOnly="true" />
                        </br>
                    </li>
                    <li id="li2">
                        <asp:Label runat="server" AssociatedControlID="tbAnswer"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,AnswerT %>"/></asp:Label>
                        <asp:TextBox runat="server" ID="tbAnswer" />
                        </br>
                    </li>
                    <li>
                        <asp:Button ID="btnNext" runat="server" OnClientClick="checkVisible" Text="<%$ Resources:LocalizedText,Next %>" CssClass="loginButton" OnClick="btnNext_Click" />
                    </li>
                </ol>
            </fieldset>
        </div>
    </form>
    <script>
        $(document).ready(function () {
                checkVisible();
        });

        function checkVisible() {
            var $tbun = $("#<%: tbUserName.ClientID %>");
            var $lbFail = $("#pFailureText");
            if ($tbun.val() == "" || $lbFail.text() != "") {
                $("#li1").css("display", "none");
                $("#li2").css("display", "none");
            }
            else {
                $("#li1").css("display", "block");
                $("#li2").css("display", "block");
            }

            var $pLOK = $("#<%: pLOK.ClientID %>");
            if ($pLOK.css("display") == "block") {
                $("#fsLoginForm").css("display", "none");
                setTimeout(function () {
                    window.location.href = "<%: SiteURL %>";
                }, 3000);
            }
        }
    </script>
</body>
</html>

