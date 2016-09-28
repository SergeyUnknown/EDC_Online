<%@ Page Title="Добавление Субъекта" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateEditSubject.aspx.cs" Inherits="EDC.Pages.Subject.CreateEditSubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="labelStatus" runat="server" Visible="false" Text="" />
    <ol class="addSubjectList">
        <li>
            <asp:Label runat="server" AssociatedControlID="tbNumber">Номер субъекта</asp:Label>
            <asp:TextBox runat="server" ID="tbNumber" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNumber" Display="Dynamic"
                CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="tbDate">Дата включения в исследование</asp:Label>
            <asp:TextBox runat="server" ID="tbDate" />
            <ajaxToolkit:CalendarExtender runat="server" ID="ajaxCalendar" FirstDayOfWeek="Monday" TargetControlID="tbDate" Format="dd.MM.yyyy" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDate" Display="Dynamic"
                CssClass="field-validation-error" ErrorMessage="Данное поле обязательно для заполнения" />
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="ddlCenters">Мед. центр</asp:Label>
            <asp:DropDownList runat="server" ID="ddlCenters" />
        </li>
    </ol>

    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Отмена" CausesValidation="false" OnClick="btnCancel_Click" />
</asp:Content>
