<%@ Page Title="Добавить CRF" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCRF.aspx.cs" Inherits="EDC.Pages.CRF.AddCRF" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload runat="server" ID="fuAddCRF" AllowMultiple="false" />
    <asp:Button runat="server" ID="btnUpload" OnClick="btnUpload_Click" Text="Загрузить" />

    <asp:GridView runat="server" ID="gvErrors" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="SectionName" HeaderText="Название раздела" />
            <asp:BoundField DataField="Row" HeaderText="Строка" />
            <asp:BoundField DataField="Column" HeaderText="Столбец" />
            <asp:BoundField DataField="ErrorMessage" HeaderText="Ошибка" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>

</asp:Content>
