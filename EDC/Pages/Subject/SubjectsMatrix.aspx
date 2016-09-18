<%@ Page Title="Матрица субъектов" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsMatrix.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsMatrix" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="LegendPlace" runat="server">
    <asp:Table runat="server" ID="tLegend" CssClass="Legend">

        <asp:TableRow>
            <asp:TableHeaderCell RowSpan="2">
                <asp:Label runat="server">Легенда</asp:Label>
            </asp:TableHeaderCell>
            <asp:TableCell>
               <asp:Image runat="server" CssClass="NEWCLASS"/> 
               <asp:Label runat="server">Ввод данных не начат</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Clip Start"/>
                <asp:Label runat="server">Ввод данных начат</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Clip End"/>
                <asp:Label runat="server">Завершено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Approve"/>
                <asp:Label runat="server">Подписано</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data CheckAll"/>
                <asp:Label runat="server">Сверка проведена</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Stopped"/>
                <asp:Label runat="server">Остановлено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Lock"/>
                <asp:Label runat="server">Заблокировано</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Delete"/>
                <asp:Label runat="server">Удалено</asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel runat="server" ID="pScrolls" ScrollBars="Horizontal" style="max-width:910px">
        <asp:Table runat="server" ID="tMatrix"></asp:Table>
    </asp:Panel>
    <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />

</asp:Content>
