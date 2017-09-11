<%@ Page Title="<%$ Resources:LocalizedText,AddSubject %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateEditSubject.aspx.cs" Inherits="EDC.Pages.Subject.CreateEditSubject" %>

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
    <asp:Label ID="labelStatus" runat="server" Visible="false" Text="" />
    <ol class="addSubjectList">
        <li>
            <asp:Label runat="server" AssociatedControlID="tbNumber" Text="<%$ Resources:LocalizedText,SubjectNumber %>" />
            <asp:TextBox runat="server" ID="tbNumber" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNumber" Display="Dynamic"
                CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" />
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="tbDate" Text="<%$ Resources:LocalizedText,DateOfEnrollment %>" />
            <asp:TextBox runat="server" ID="tbDate" />
            <ajaxToolkit:CalendarExtender runat="server" ID="ajaxCalendar" FirstDayOfWeek="Monday" TargetControlID="tbDate" Format="dd.MM.yyyy" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDate" Display="Dynamic"
                CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" />
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="ddlCenters" Text="<%$ Resources:LocalizedText,Site %>" />
            <asp:DropDownList runat="server" ID="ddlCenters" />
        </li>
    </ol>

    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText,Cancel %>" CausesValidation="false" OnClick="btnCancel_Click" />
</asp:Content>
