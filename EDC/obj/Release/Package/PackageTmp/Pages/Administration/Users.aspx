<%@ Page Title="Пользователи" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="EDC.Pages.Administration.Users" %>
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

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="labelStatus"></asp:Label><br />
    <asp:Label runat="server" ID="lblUserOnline" EnableViewState ="False">Пользователей в сети: <%: CountUserOnline %></asp:Label><br />

    <asp:GridView runat="server" ID="gvUsers" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDataBound="gvUsers_RowDataBound" OnRowDeleting="gvUsers_RowDeleting" OnRowEditing="gvUsers_RowEditing">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="Имя пользователя" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Name" HeaderText="Имя" />
            <asp:BoundField DataField="Lastname" HeaderText="Фамилия" />
            <asp:BoundField DataField="Phone" HeaderText="Телефон" />
            <asp:BoundField DataField="Role" HeaderText="Роли" />
            <asp:BoundField DataField="Locked" HeaderText="Заблокирован" />
            <asp:CommandField HeaderText="Действия" SelectText="Просмотреть" ShowDeleteButton="True" DeleteText="Удалить" ShowEditButton="True" UpdateText="Обновить" CancelText="Отмена" EditText="Редактировать"/>
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

    <DownTableControl:DownTable ID="dtInfo" runat="server" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
</asp:Content>
