<%@ Page Title="Добавление события" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateEditEvent.aspx.cs" Inherits="EDC.Pages.Event.CreateEditEvent" %>

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
                            CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbIdentifier">Идентификатор</asp:Label>
                        <asp:TextBox runat="server" ID="tbIdentifier" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbIdentifier"
                            CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbIdentifier">Обязательное событие?</asp:Label>
                        <asp:DropDownList runat="server" ID="ddlRequired" />
                    </li>
                </ol>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText,Cancel %>" CausesValidation="false" OnClick="btnCancel_Click" />
    </fieldset>
</asp:Content>
