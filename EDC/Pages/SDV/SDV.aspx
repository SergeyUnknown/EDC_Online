<%@ Page Title="Сверка первичных данных" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SDV.aspx.cs" Inherits="EDC.Pages.SDV.SDV" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LegendPlace" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lblInfo" Visible="false" Text="Отсутствуют ИРК, для которых можно произвести сверку с первичными данными"/>
    <asp:GridView runat="server" ID="gvApprovedSubjectsCRF" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Subject.Number" HeaderText="Номер субъекта" />
            <asp:BoundField DataField="Event.Name" HeaderText="Событие" />
            <asp:BoundField DataField="CRF.Name" HeaderText="ИРК" />
            <asp:TemplateField HeaderText="Просмотреть">
                <ItemTemplate>
                    <input type="button" value=" " onclick="<%# string.Format("window.open('{0}');",GetRedirectURL(Container.DataItem)) %>" />
                </ItemTemplate>
            </asp:TemplateField>
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
