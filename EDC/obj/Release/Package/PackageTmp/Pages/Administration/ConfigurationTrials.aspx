<%@ Page Title="<%$ Resources:LocalizedText,StudySettings %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigurationTrials.aspx.cs" Inherits="EDC.Pages.Administration.ConfigurationTrials" %>

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
    <ol class="StatusResearch vertical">
        <li>
            <asp:Label runat="server" AssociatedControlID="rbStatus"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,StudyStatus %>" /></asp:Label>
            <asp:RadioButtonList runat="server" ID="rbStatus" CssClass="radibuttonStatus">
                <asp:ListItem Text="<%$ Resources:LocalizedText,Design %>" Value="Design"/>
                <asp:ListItem Text="<%$ Resources:LocalizedText,Active %>" Value="Enable"/>
                <asp:ListItem Text="<%$ Resources:LocalizedText,Locked %>" Value="Disable" />
            </asp:RadioButtonList>
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="tbStudyName" Text="<%$ Resources:LocalizedText,StudyName %>"/>
            <asp:TextBox runat="server" ID="tbStudyName"/>
        </li>
        <li>
            <asp:Label runat="server" AssociatedControlID="tbProtocolID" Text="<%$ Resources:LocalizedText,StudyProtocol %>"/>
            <asp:TextBox runat="server" ID="tbProtocolID"/>
        </li>
    </ol>
    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="<%$ Resources:LocalizedText,Save %>" />
</asp:Content>
