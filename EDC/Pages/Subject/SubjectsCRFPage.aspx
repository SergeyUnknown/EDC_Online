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
    <asp:UpdatePanel runat="server" ID="up1">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Button runat="server" ID="btnPrevSubject" Text="Предыдущий субъект" ToolTip="Предыдущий субъект" />
            <asp:Button runat="server" ID="btnPrevCRFInEvent" Text="Предыдущая CRF" ToolTip="Предыдущая CRF" />

                <ajaxToolkit:TabContainer runat="server" ID="tcCRF">
                </ajaxToolkit:TabContainer>
            
            <asp:Button runat="server" ID="btnNextCRFInEvent" Text="Следующая CRF" ToolTip="Следующая CRF" />
            <asp:Button runat="server" ID="btnNextSubject" Text="Следующий субъект" ToolTip="Следующий субъект" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
