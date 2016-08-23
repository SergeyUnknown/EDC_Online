<%@ Page Title="Выполнить вход" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EDC.Account.Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
        <asp:Login runat="server" ViewStateMode="Disabled" RenderOuterTable="false">
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
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
                        <li>
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Запомнить меня</asp:Label>
                        </li>
                    </ol>
                    <asp:Button runat="server" CommandName="Login" Text="Выполнить вход" />
                </fieldset>
            </LayoutTemplate>
        </asp:Login>

</asp:Content>
