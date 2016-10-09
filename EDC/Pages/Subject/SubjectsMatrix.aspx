<%@ Page Title="Матрица субъектов" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsMatrix.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsMatrix" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="LegendPlace" runat="server">
    <asp:Table runat="server" ID="tLegend" CssClass="Legend">

        <asp:TableRow>
            <asp:TableHeaderCell RowSpan="2">
                <asp:Label runat="server">Легенда</asp:Label>
            </asp:TableHeaderCell>
            <asp:TableCell>
               <input type="button" Class="ActionIc Unplaned"/> 
               <asp:Label runat="server">Ввод данных не начат</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Start"/>
                <asp:Label runat="server">Ввод данных начат</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc End"/>
                <asp:Label runat="server">Завершено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Approve"/>
                <asp:Label runat="server">Подписано</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc CheckAll"/>
                <asp:Label runat="server">Сверка проведена</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Stopped"/>
                <asp:Label runat="server">Остановлено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Lock"/>
                <asp:Label runat="server">Заблокировано</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Delete"/>
                <asp:Label runat="server">Удалено</asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Panel runat="server" ID="pScrolls" ScrollBars="Horizontal">
                <asp:Table runat="server" ID="tMatrix"></asp:Table>
            </asp:Panel>
            <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
