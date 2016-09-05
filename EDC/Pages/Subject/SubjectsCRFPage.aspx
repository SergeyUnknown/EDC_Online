<%@ Page Title="Просмотр ИРК субъекта" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsCRFPage.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsCRFPage" %>
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

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server"> 
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <ajaxToolkit:TabContainer runat="server" ID="tcCRF" AutoPostBack="true">

            </ajaxToolkit:TabContainer>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
