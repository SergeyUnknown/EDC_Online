<%@ Page Title="Добавление нового пользователя" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCreateUser.aspx.cs" Inherits="EDC.Pages.Administration.ViewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Label ID="labelStatus" runat="server" Visible="false" Text="" />
                <ol>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbUserName">Имя пользователя</asp:Label>
                        <asp:TextBox runat="server" ID="tbUserName" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbUserName"
                            CssClass="field-validation-error" ErrorMessage="Поле Имя пользователя заполнять обязательно." />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbEmail">Адрес электронной почты</asp:Label>
                        <asp:TextBox runat="server" ID="tbEmail" TextMode="Email" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEmail"
                            CssClass="field-validation-error" ErrorMessage="Поле Электронной почты заполнять обязательно." />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbName">Имя</asp:Label>
                        <asp:TextBox runat="server" ID="tbName" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbName"
                            CssClass="field-validation-error" ErrorMessage="Поле Имя обязательно для заполнения." />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbLastname">Фамилия</asp:Label>
                        <asp:TextBox runat="server" ID="tbLastname" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbLastname"
                            CssClass="field-validation-error" ErrorMessage="Поле Фамилия обязательно для заполнения." />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbPhone">Номер телефона</asp:Label>
                        <asp:TextBox runat="server" ID="tbPhone" TextMode="Phone" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="cblUserRole">Роль</asp:Label>
                        <asp:CheckBoxList runat="server" CssClass="rolesCheck" ID="cblUserRole" AutoPostBack="true">
                        </asp:CheckBoxList>
                    </li>
                    <li runat="server" visible="false" id="liCenters">
                        <asp:Label runat="server" AssociatedControlID="cblCenters">Доступ к центрам</asp:Label>
                        <asp:CheckBoxList runat="server" CssClass="rolesCheck" ID="cblCenters">
                        </asp:CheckBoxList>
                    </li>

                </ol>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click"/>
        <asp:Button ID="btnCancel" runat="server" Text="Отмена" CausesValidation="false" OnClick="btnCancel_Click" />
    </fieldset>
</asp:Content>
