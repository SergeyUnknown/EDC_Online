﻿<%@ Page Title="Выполнить вход" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EDC.Account.Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
        <asp:Login runat="server" ID="LoginControl" ViewStateMode="Disabled" DisplayRememberMe="false" RenderOuterTable="false" OnLoggingIn="Unnamed_LoggingIn">
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset class="loginForm">
                    <legend>Форма входа</legend>
                    <ol>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="UserName">Имя пользователя</asp:Label>
                            <asp:TextBox runat="server" ID="UserName" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="Поле имени пользователя заполнять обязательно." />
                        </li>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="Password">Пароль</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="Поле пароля заполнять обязательно." />
                        </li>
                    </ol>
                    <asp:Button runat="server" CommandName="Login" Text="Выполнить вход" CssClass="loginButton" />
                </fieldset>
            </LayoutTemplate>
        </asp:Login>

</asp:Content>
