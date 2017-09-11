<%@ Page Title="<%$ Resources:LocalizedText,ManageUP %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="EDC.Account.Manage" %>
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
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <section id="manageAccountForm">
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="message-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="errorMessage" Visible="false" ViewStateMode="Disabled">
            <p class="field-validation-error"><%: ErrorMessage %></p>
        </asp:PlaceHolder>

        <table class="manageAccountTable">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblUserName" AssociatedControlID="tbUserName"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,UserName %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbUserName" ReadOnly="true" EnableViewState="false" />
                </td>
            </tr> <%--Имя пользователя--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblName" AssociatedControlID="tbName"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Name %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbName" EnableViewState="false" />
                </td>
            </tr> <%--Имя--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblLastname" AssociatedControlID="tbLastname"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,SurName %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbLastname" EnableViewState="false"/>
                </td>
            </tr> <%--Фамилия--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblEmail" AssociatedControlID="tbEmail"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Email %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbEmail" TextMode="Email" EnableViewState="false" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEmail"
                        CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>"
                        ValidationGroup="ChangeInfo" display="Dynamic"/>
                </td>
            </tr> <%--Email--%>

            <tr>
                <td>
                    <asp:Label runat="server" ID="lblPhone" AssociatedControlID="tbPhone"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PhoneNumber %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbPhone" TextMode="Phone" EnableViewState="false" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPhone"
                        CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>"
                        ValidationGroup="ChangeInfo" display="Dynamic"/>
                </td>
            </tr> <%--Phone--%>

            <tr>
                <td>
                    <asp:Label runat="server" ID="lblQuestion" AssociatedControlID="tbQuestion"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PasswordChallengeQuestion %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbQuestion" EnableViewState="false" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbQuestion"
                        CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>"
                        ValidationGroup="ChangeInfo" display="Dynamic"/>
                </td>
            </tr> <%--Контрольный вопрос--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblAnswer" AssociatedControlID="tbAnswer"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PasswordChallengeAnswer %>"/>:</asp:Label>
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
                    <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="tbCurrentPassword"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,CurrentPassword %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbCurrentPassword" CssClass="passwordEntry" TextMode="Password" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbCurrentPassword"
                        CssClass="field-validation-error" ErrorMessage="Поле текущего пароля заполнять обязательно."
                        ValidationGroup="ChangeInfo" display="Dynamic"/>
                </td>
            </tr> <%--Текущий пароль--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="tbNewPassword"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,NewPassword %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbNewPassword" CssClass="passwordEntry" TextMode="Password" />
                </td>
                <td>
                </td>
            </tr> <%--Новый пароль--%>
            
            <tr>
                <td>
                    <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,PasswordConfirmation %>"/>:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry" TextMode="Password" />
                </td>
                <td>
                    <asp:CompareValidator runat="server" ControlToCompare="tbNewPassword" ControlToValidate="ConfirmNewPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="<%$ Resources:LocalizedText,PasswordsEnteredDoNotMatch %>"
                        ValidationGroup="ChangeInfo" />
                </td>
            </tr> <%--Подтверждение--%>
        </table>
        <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Save %>" ID="btnSaveChange" ValidationGroup="ChangeInfo" OnClick="btnSaveChange_Click" />
        <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Cancel %>" ID="btnCancel" OnClick="btnCancel_Click" />
    </section>
</asp:Content>
