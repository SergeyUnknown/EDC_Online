<%@ Page Title="<%$ Resources:LocalizedText,Users %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="EDC.Pages.Administration.Users" %>
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

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
    <asp:Label runat="server" ID="labelStatus"></asp:Label><br />
    <asp:Label runat="server" ID="lblUserOnline" EnableViewState ="False"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,UsersAreOnline %>" /> <%: CountUserOnline %></asp:Label><br />

    <asp:GridView runat="server" ID="gvUsers" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False" OnRowDataBound="gvUsers_RowDataBound" OnRowDeleting="gvUsers_RowDeleting" OnRowEditing="gvUsers_RowEditing">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:LocalizedText,UserName %>" />
            <asp:BoundField DataField="Email" HeaderText="<%$ Resources:LocalizedText,Email %>" />
            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedText,Name %>" />
            <asp:BoundField DataField="Lastname" HeaderText="<%$ Resources:LocalizedText,Surname %>" />
            <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:LocalizedText,PhoneNumber %>" />
            <asp:BoundField DataField="Role" HeaderText="<%$ Resources:LocalizedText,Roles %>" />
            <asp:BoundField DataField="Locked" HeaderText="<%$ Resources:LocalizedText,LockedN %>" />
            <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Actions %>" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="ibEdit" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="~/Images/edit.png" Text="<%$ Resources:LocalizedText,Edit %>" />
                    &nbsp;<asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/Images/delete.png" Text="<%$ Resources:LocalizedText,Remove %>" OnClientClick="return confirm('Вы уверены, что хотите удалить данного пользователя?')" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    <DownTableControl:DownTable ID="dtInfo" runat="server" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
</asp:Content>
