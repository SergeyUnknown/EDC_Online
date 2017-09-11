<%@ Page Title="Добавление правил" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRule.aspx.cs" Inherits="EDC.Pages.Rule.AddRule" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LegendPlace" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lblStatus" Text="Файл с правилами успешно загружен" Visible="false"/>
    <br />
    <asp:FileUpload runat="server" ID="fuAddQuery" AllowMultiple="false" />
    <asp:Button runat="server" ID="btnUpload" OnClick="btnUpload_Click" Text="Загрузить" />

    <asp:GridView runat="server" ID="gvErrors" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="RuleAssignmentNumber" HeaderText="Номер правила" />
            <asp:BoundField DataField="OID" HeaderText="OID" />
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
