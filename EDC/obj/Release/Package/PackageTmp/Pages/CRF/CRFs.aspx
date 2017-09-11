<%@ Page Title="<%$ Resources:LocalizedText,CRFs %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CRFs.aspx.cs" Inherits="EDC.Pages.CRF.CRFs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ImageButton runat="server" ImageUrl="~/Images/table-row-plus-after.png" PostBackUrl="~/CRFs/Add" CssClass="onlyAdd" AlternateText="<%$ Resources:LocalizedText,Add %>" />

    <asp:GridView runat="server" ID="gvCRFs" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvCRFs_RowDeleting" OnSelectedIndexChanging="gvCRFs_SelectedIndexChanging">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,NName %>">
                <ItemTemplate>
                    <%# Eval("RussianName") != null ? Eval("RussianName") : Eval("Name") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Version" HeaderText="Версия" />
            <asp:BoundField DataField="Identifier" HeaderText="Идентификатор" />
            <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:LocalizedText,CreationDate %>" DataFormatString="{0:dd.MM.yyyy}" />
            <asp:BoundField DataField="CreatedBy" HeaderText="Создано" />
            <asp:CommandField HeaderText="<%$ Resources:LocalizedText,Actions %>" ButtonType="Image" SelectText="Просмотреть" ShowDeleteButton="True" ShowSelectButton="true" DeleteText="<%$ Resources:LocalizedText,Remove %>" DeleteImageUrl="~/Images/delete.png" SelectImageUrl="~/Images/magnify.png"/>

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
