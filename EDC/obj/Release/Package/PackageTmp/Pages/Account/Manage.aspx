<%@ Page Title="Настройки учетной записи" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="EDC.Account.Manage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>

    <section id="manageAccountForm">
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="message-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="errorMessage" Visible="false" ViewStateMode="Disabled">
            <p class="field-validation-error"><%: ErrorMessage %></p>
        </asp:PlaceHolder>

        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblUserName" AssociatedControlID="tbUserName">Имя пользователя:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbUserName" ReadOnly="true" EnableViewState="false" />
                </td>
            </tr> <%--Имя пользователя--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblName" AssociatedControlID="tbName">Имя:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbName" EnableViewState="false" />
                </td>
            </tr> <%--Имя--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblLastname" AssociatedControlID="tbLastname">Фамилия:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbLastname" EnableViewState="false"/>
                </td>
            </tr> <%--Фамилия--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblEmail" AssociatedControlID="tbEmail">Электронная почта:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbEmail" TextMode="Email" EnableViewState="false" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEmail"
                        CssClass="field-validation-error" ErrorMessage="Поле Email заполнять обязательно."
                        ValidationGroup="ChangeInfo" />
                </td>
            </tr> <%--Email--%>

            <tr>
                <td>
                    <asp:Label runat="server" ID="lblPhone" AssociatedControlID="tbPhone">Номер телефона:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbPhone" TextMode="Phone" EnableViewState="false" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPhone"
                        CssClass="field-validation-error" ErrorMessage="Поле Номер телефона заполнять обязательно."
                        ValidationGroup="ChangeInfo" />
                </td>
            </tr> <%--Phone--%>

            <tr>
                <td>
                    <asp:Label runat="server" ID="lblQuestion" AssociatedControlID="tbQuestion">Контрольный вопрос:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbQuestion" EnableViewState="false" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbQuestion"
                        CssClass="field-validation-error" ErrorMessage="Поле контрольный вопрос заполнять обязательно."
                        ValidationGroup="ChangeInfo" />
                </td>
            </tr> <%--Контрольный вопрос--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblAnswer" AssociatedControlID="tbAnswer">Ответ:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbAnswer" EnableViewState="false" />
                </td>
                <td>
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tbAnswer"
                        CssClass="field-validation-error" ErrorMessage="Поле ответ на контрольный вопрос заполнять обязательно."
                        ValidationGroup="ChangeInfo" />--%>
                </td>
            </tr> <%--Ответ--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="tbCurrentPassword">Текущий пароль:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbCurrentPassword" CssClass="passwordEntry" TextMode="Password" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbCurrentPassword"
                        CssClass="field-validation-error" ErrorMessage="Поле текущего пароля заполнять обязательно."
                        ValidationGroup="ChangeInfo" />
                </td>
            </tr> <%--Текущий пароль--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="tbNewPassword">Новый пароль:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbNewPassword" CssClass="passwordEntry" TextMode="Password" />
                </td>
                <td>
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tbNewPassword"
                        CssClass="field-validation-error" ErrorMessage="Поле нового пароля заполнять обязательно."
                        ValidationGroup="ChangeInfo" />--%>
                </td>
            </tr> <%--Новый пароль--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword">Подтверждение пароля:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry" TextMode="Password" />
                </td>
                <td>
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="Поле подтверждения пароля заполнять обязательно."
                        ValidationGroup="ChangeInfo" />--%>
                    <asp:CompareValidator runat="server" ControlToCompare="tbNewPassword" ControlToValidate="ConfirmNewPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="Новый пароль и его подтверждение не совпадают."
                        ValidationGroup="ChangeInfo" />
                </td>
            </tr> <%--Подтверждение--%>
        </table>
        <asp:Button runat="server" Text="Сохранить изменения" ID="btnSaveChange" ValidationGroup="ChangeInfo" OnClick="btnSaveChange_Click" />
        <asp:Button runat="server" Text="Отмена" ID="btnCancel" OnClick="btnCancel_Click" />
    </section>
</asp:Content>
