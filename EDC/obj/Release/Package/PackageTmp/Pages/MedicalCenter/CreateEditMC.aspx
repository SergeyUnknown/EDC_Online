<%@ Page Title="Добавление медицинского центра" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateEditMC.aspx.cs" Inherits="EDC.Pages.MedicalCenter.CreateEditMC" %>

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
                        <asp:Label runat="server" AssociatedControlID="tbName">Название</asp:Label>
                        <asp:TextBox runat="server" ID="tbName" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbNumber">Номер центра</asp:Label>
                        <asp:TextBox runat="server" ID="tbNumber" TextMode="Number" min="1"  />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNumber"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbCountry">Страна</asp:Label>
                        <asp:TextBox runat="server" ID="tbCountry" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbCountry"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbRegion">Регион</asp:Label>
                        <asp:TextBox runat="server" ID="tbRegion" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbRegion"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbCity">Город</asp:Label>
                        <asp:TextBox runat="server" ID="tbCity" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbCity"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbStreet">Улица</asp:Label>
                        <asp:TextBox runat="server" ID="tbStreet" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbStreet"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbHouse">Дом</asp:Label>
                        <asp:TextBox runat="server" ID="tbHouse" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbHouse"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbPhone">Телефон</asp:Label>
                        <asp:TextBox runat="server" ID="tbPhone" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbPhone"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbPI">Главный исследователь</asp:Label>
                        <asp:TextBox runat="server" ID="tbPI" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbPI"
                            CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
                    </li>
                </ol>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Отмена" CausesValidation="false" OnClick="btnCancel_Click" />
    </fieldset>
</asp:Content>
