<%@ Page Title="Главная страница" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EDC._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server">
        <Triggers>

        </Triggers>
        <ContentTemplate>
            <asp:DropDownList runat="server" ID="ddlCRF" AutoPostBack="true" OnSelectedIndexChanged="ddlCRF_SelectedIndexChanged"></asp:DropDownList>
            
            <ajaxToolkit:TabContainer runat="server" ID="tcCRF">

            </ajaxToolkit:TabContainer>

            <asp:Panel runat="server" ID="pControls">

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
