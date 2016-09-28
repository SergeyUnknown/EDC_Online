<%@ Page Title="Настройка исследования" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigurationTrials.aspx.cs" Inherits="EDC.Pages.Administration.ConfigurationTrials" %>

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
    <ol class="StatusResearch vertical">
        <li>
            <asp:Label runat="server" AssociatedControlID="rbStatus">Статус исследования</asp:Label>
            <asp:RadioButtonList runat="server" ID="rbStatus" CssClass="radibuttonStatus">
                <asp:ListItem Text="Дизайн" Value="Design"/>
                <asp:ListItem Text="Активно" Value="Enable"/>
                <asp:ListItem Text="Заблокировано" Value="Disable" />
            </asp:RadioButtonList>
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="tbStudyName">Название исследования</asp:Label>
            <asp:TextBox runat="server" ID="tbStudyName"/>
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="tbProtocolID">Номер протокола</asp:Label>
            <asp:TextBox runat="server" ID="tbProtocolID"/>
        </li>
    </ol>
    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Сохранить" />
</asp:Content>
