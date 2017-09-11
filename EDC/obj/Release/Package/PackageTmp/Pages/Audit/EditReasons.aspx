﻿<%@ Page Title="Причины изменения ИРК" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditReasons.aspx.cs" Inherits="EDC.Pages.Audit.EditReasons" %>

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
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvEditReason" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:LocalizedText,User %>" />
                    <asp:BoundField DataField="ActionDate" HeaderText="<%$ Resources:LocalizedText,Date %>" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="ActionDate" HeaderText="<%$ Resources:LocalizedText,Time %>" DataFormatString="{0:HH:mm:ss}" />
                    <asp:BoundField DataField="Subject.Number" HeaderText="<%$ Resources:LocalizedText,Subject %>" />
                    <asp:BoundField DataField="SubjectCRF.Event.Name" HeaderText="<%$ Resources:LocalizedText,Event %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,CRF %>">
                        <ItemTemplate><%# GetCRFName((EDC.Models.SubjectsCRF)Eval("SubjectCRF")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EditReason" HeaderText="Причины изменения ИРК" />
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
