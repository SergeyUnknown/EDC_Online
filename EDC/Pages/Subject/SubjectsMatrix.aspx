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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Table runat="server" ID="tMatrix"></asp:Table>

    <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
</asp:Content>
