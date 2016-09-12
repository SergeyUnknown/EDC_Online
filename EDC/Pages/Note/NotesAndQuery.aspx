<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotesAndQuery.aspx.cs" Inherits="EDC.Pages.Note.NotesAndQuery" %>

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
    <asp:UpdatePanel runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>

            <asp:GridView runat="server" ID="gvNotes" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Subject.Number" HeaderText="Номер субъекта" />
                    <asp:BoundField DataField="Type" HeaderText="Тип" />
                    <asp:BoundField DataField="Status" HeaderText="Статус" />
                    <asp:BoundField DataField="CreationDate" HeaderText="Дата создания" />
                    <asp:BoundField DataField="Event.Name" HeaderText="Событие" />
                    <asp:BoundField DataField="CRF.Name" HeaderText="ИРК" />
                    <asp:BoundField DataField="CRF_Item.Name" HeaderText="Запись" />
                    <asp:BoundField DataField="SubjectItem.Value" HeaderText="Значение записи" />
                    <asp:BoundField DataField="Text" HeaderText="Текст заметки" />
                    <asp:BoundField DataField="ForUser" HeaderText="Назначено" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
