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
    <ol>
        <li>
            <asp:Label runat="server" AssociatedControlID="rbStatus">Статус исследования</asp:Label>
            <asp:RadioButtonList runat="server" ID="rbStatus">
                <asp:ListItem Text="Дизайн" Value="Design"/>
                <asp:ListItem Text="Активно" Value="Enable"/>
                <asp:ListItem Text="Заблокировано" Value="Disable" />
            </asp:RadioButtonList>
        </li>


    </ol>
</asp:Content>
