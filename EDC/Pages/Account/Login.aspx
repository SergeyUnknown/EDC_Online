<%@ Page Title="Выполнить вход" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EDC.Account.Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="LoginWindow">
    <hgroup class="title align-middle">
        <h1><%: Title %></h1>
    </hgroup>
        <asp:Login runat="server" ID="LoginControl" ViewStateMode="Disabled" DisplayRememberMe="false" RenderOuterTable="false" OnLoggingIn="Unnamed_LoggingIn">
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset class="loginForm">
                    <legend>Форма входа</legend>
                    <ol class="loginInfo">
                        <li>
                            <asp:Label runat="server" AssociatedControlID="UserName">Имя пользователя</asp:Label>
                            <asp:TextBox runat="server" ID="UserName" />
                            </br>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error align-middle" ErrorMessage="Поле имени пользователя заполнять обязательно."  Display="Dynamic"/>
                        </li>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="Password">Пароль</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                            </br>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error align-middle" ErrorMessage="Поле пароля заполнять обязательно." Display="Dynamic"/>
                        </li>
                        <li>
                            </br>
                            <asp:Button runat="server" CommandName="Login" Text="Выполнить вход" CssClass="loginButton" />
                        </li>
                    </ol>
                    
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
    </div>
</asp:Content>
