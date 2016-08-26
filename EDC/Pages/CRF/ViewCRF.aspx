<%@ Page Title="Просмотр CRF" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCRF.aspx.cs" Inherits="EDC.Pages.CRF.ViewCRF" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSections" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGroups" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnItems" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Button runat="server" ID="btnSections" OnClick="btnSections_Click" Text="Секции" />
            <asp:Button runat="server" ID="btnGroups" OnClick="btnGroups_Click" Text="Группы" />
            <asp:Button runat="server" ID="btnItems" OnClick="btnItems_Click" Text="Итемы" />

            <asp:GridView runat="server" ID="gvSections" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Label" HeaderText="Лейбл" />
                    <asp:BoundField DataField="Title" HeaderText="Заголовок" />
                    <asp:BoundField DataField="Subtitle" HeaderText="Подзаголовок" />
                    <asp:BoundField DataField="Instructions" HeaderText="Инструкции" />
                    <asp:BoundField DataField="PageNumber" HeaderText="Страница" />
                    <asp:BoundField DataField="Border" HeaderText="Рамка" />
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

            <asp:GridView runat="server" ID="gvGroups" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Identifier" HeaderText="Идентификатор" />
                    <asp:BoundField DataField="Header" HeaderText="Заголовок" />
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

            <asp:GridView runat="server" ID="gvCRF_Fields" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Identifier" HeaderText="Идентификатор" />
                    <asp:BoundField DataField="DescriptionLabel" HeaderText="Описание" />
                    <asp:BoundField DataField="LeftItemText" HeaderText="Текст до" />
                    <asp:BoundField DataField="Units" HeaderText="Ед. Измерения" />
                    <asp:BoundField DataField="RightItemText" HeaderText="Текст после" />
                    <asp:BoundField DataField="Section.Label" HeaderText="Секция" />
                    <asp:BoundField DataField="Group.Identifier" HeaderText="Группа" />
                    <asp:BoundField DataField="ResponseType" HeaderText="Тип запроса" />
                    <asp:BoundField DataField="DataType" HeaderText="Тип данных" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
