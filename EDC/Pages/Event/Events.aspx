﻿<%@ Page Title="События" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="EDC.Pages.Event.Events" %>

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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvEvents" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvEvents_RowDeleting" OnSelectedIndexChanging="gvEvents_SelectedIndexChanging" OnRowEditing="gvEvents_RowEditing" OnRowDataBound="gvEvents_RowDataBound">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="№ п/п">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlPos" runat="server" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedText,NName %>" />
                    <asp:BoundField DataField="Identifier" HeaderText="Идентификатор" />
                    <asp:TemplateField HeaderText="Обязательно">
                        <ItemTemplate>
                            <asp:Label runat="server"><%# ((bool)Eval("Required"))?"Да":"Нет"  %></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DateCreation" HeaderText="<%$ Resources:LocalizedText,CreationDate %>" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:LocalizedText,CreatedBy %>" />
                    <asp:CommandField HeaderText="<%$ Resources:LocalizedText,Actions %>" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" EditImageUrl="~/Images/edit.png" SelectImageUrl="~/Images/settings.png" ShowSelectButton="true" ShowDeleteButton="True" ShowEditButton="true" DeleteText="<%$ Resources:LocalizedText,Remove %>" EditText="<%$ Resources:LocalizedText,Edit %>" SelectText="Настроить"/>
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
    <DownTableControl:DownTable ID="dtInfo" runat="server" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
</asp:Content>
