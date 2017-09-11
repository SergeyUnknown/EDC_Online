<%@ Page Title="Правила" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rules.aspx.cs" Inherits="EDC.Pages.Rule.Rules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
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
    <asp:UpdatePanel runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvRules" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvRules_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="OID" HeaderText="OID" />
                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedText,NName %>" />
                    <asp:BoundField DataField="AddedDate" HeaderText="Дата добавления" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="Target" HeaderText="Цель" />
                    <asp:BoundField DataField="ErrorMessage" HeaderText="Сообщение" />
                    <asp:BoundField DataField="Expression" HeaderText="Выражение" />
                    <asp:BoundField DataField="IfExpressionEvaluates" HeaderText="Отображается при" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,CRF %>">
                        <ItemTemplate>
                            <asp:Label runat="server"><%# string.IsNullOrWhiteSpace((string)Eval("Item.CRF.RussianName"))?Eval("Item.CRF.Name"):Eval("Item.CRF.RussianName") %></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Item.Name" HeaderText="Итем" />

                    <%--Удалить--%>
                    <asp:CommandField HeaderText="<%$ Resources:LocalizedText,Remove %>" ButtonType="Image" ShowDeleteButton="True" DeleteText="<%$ Resources:LocalizedText,Remove %>" DeleteImageUrl="~/Images/delete.png" />

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

            <DownTableControl:DownTable ID="dtInfo" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
