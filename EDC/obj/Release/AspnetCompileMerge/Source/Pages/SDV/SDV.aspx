<%@ Page Title="<%$ Resources:LocalizedText,SourseDataVerification %>" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SDV.aspx.cs" Inherits="EDC.Pages.SDV.SDV" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
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
    <asp:Label runat="server" ID="lblInfo" Visible="false" Text="<%$ Resources:LocalizedText,ThereAreNoCRFsAvailableForSDV %>"/>
    <asp:GridView runat="server" ID="gvApprovedSubjectsCRF" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Subject.Number" HeaderText="<%$ Resources:LocalizedText,SubjectNumber %>" />
            <asp:BoundField DataField="Event.Name" HeaderText="<%$ Resources:LocalizedText,Event %>" />
            <asp:BoundField DataField="CRF.Name" HeaderText="<%$ Resources:LocalizedText,CRF %>" />
            <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,View %>">
                <ItemTemplate>
                    <a onclick="<%# string.Format("window.open('{0}');",GetRedirectURL(Container.DataItem)) %>">
                        <asp:ImageButton runat="server" ImageUrl="~/Images/magnify.png" />
                    </a>
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
