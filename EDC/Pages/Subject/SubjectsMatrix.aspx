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
                <asp:Image runat="server" CssClass="Clip Unplaned"/>
                <asp:Label runat="server">Не запланировано </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Planed"/>
                <asp:Label runat="server">Запланировано </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Clip Start"/>
                <asp:Label runat="server">Можно вводить данные</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Stopped"/>
                <asp:Label runat="server">Остановлено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Clip Empty"/>
                <asp:Label runat="server">Пропущено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Clip End"/>
                <asp:Label runat="server">Закончено</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Check"/>
                <asp:Label runat="server">Первоначальный ввод 
                    данных завершен</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Unhide"/>
                <asp:Label runat="server">Закодирован</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Hide"/>
                <asp:Label runat="server">Не закодирован</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data CheckAll"/>
                <asp:Label runat="server">Данные проверены</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Approve"/>
                <asp:Label runat="server">Подписано</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Lock"/>
                <asp:Label runat="server">Заблокировано</asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Image runat="server" CssClass="Data Delete"/>
                <asp:Label runat="server">Удален</asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
    
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:Table runat="server" ID="tMatrix"></asp:Table>
    <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />

</asp:Content>
