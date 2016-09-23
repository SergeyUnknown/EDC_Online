<%@ Page Title="Аудит данных" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Audits.aspx.cs" Inherits="EDC.Pages.Audit.Audits" %>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LegendPlace" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

        <asp:GridView runat="server" ID="gvAudits" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="Пользователь" />
            <asp:BoundField DataField="ActionDate" HeaderText="Дата" DataFormatString="{0:dd.MM.yyyy}" />
            <asp:BoundField DataField="ActionType" HeaderText="Тип поля" />
            <asp:BoundField DataField="ChangesType" HeaderText="Действие" />
            <asp:BoundField DataField="OldValue" HeaderText="Старое значение" />
            <asp:BoundField DataField="NewValue" HeaderText="Новое значение" />
            <asp:BoundField DataField="Subject.Number" HeaderText="Субъект" />
            <asp:BoundField DataField="SubjectEvent.Event.Name" HeaderText="Событие" />
            <asp:BoundField DataField="SubjectsCRF.CRF.Name" HeaderText="ИРК" />
            <asp:BoundField DataField="FieldName" HeaderText="Поле" />
            <asp:CommandField HeaderText="Действия" ButtonType="Image" SelectText="Просмотреть" ShowDeleteButton="True" ShowSelectButton="true" DeleteText="Удалить" DeleteImageUrl="~/Images/delete (3).png" SelectImageUrl="~/Images/magnify (1).png"/>

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
