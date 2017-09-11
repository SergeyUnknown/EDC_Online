<%@ Page Title="<%$ Resources:LocalizedText,ExtractData %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Profiles.aspx.cs" Inherits="EDC.Pages.Unloading.Profiles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
    <asp:UpdatePanel runat="server" ID="up">
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvUnloadingProfiles" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:LocalizedText,CreationDate %>" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:LocalizedText,CreatedBy %>" />
                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedText,NName %>" />
                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:LocalizedText,Description %>" />
                    <asp:BoundField DataField="CrfStatus" HeaderText="<%$ Resources:LocalizedText,Status %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Sites %>">
                        <ItemTemplate>
                            <%# GetCentersName((EDC.Models.UnloadingProfile)Container.DataItem) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Extract %>">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnUnload" CssClass="inputBtnCircle" OnClick="btnUnload_Click" ImageUrl="~/Images/unloading.png" CommandArgument="<%# (Container.DataItem as EDC.Models.UnloadingProfile).UnloadingProfileID %>" Text="Выгрузить" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Remove %>">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" OnClick="btnDelete_Click" ImageUrl="~/Images/delete.png" CommandArgument="<%# (Container.DataItem as EDC.Models.UnloadingProfile).UnloadingProfileID %>" OnClientClick="return window.confirm('Будут удалены все файлы выгрузки данного профиля. \r\nПродолжить?')" />
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
    <asp:Button runat="server" ID="btnAddProfile" Text="<%$ Resources:LocalizedText,CreateDataset %>" OnClick="btnAddProfile_Click" />
</asp:Content>
