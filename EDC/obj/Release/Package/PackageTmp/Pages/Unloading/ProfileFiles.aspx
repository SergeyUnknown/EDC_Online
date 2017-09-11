<%@ Page Title="<%$ Resources:LocalizedText,ExtractData %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileFiles.aspx.cs" Inherits="EDC.Pages.Unloading.ProfileFiles" %>

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


<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="ulUnloadingType">
        <li>
            <asp:Label runat="server" AssociatedControlID="btnRunExcelUnloading"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,ExcelFile %>" /></asp:Label>
            <asp:LinkButton runat="server" ID="btnRunExcelUnloading" OnClick="btnRunExcelUnloading_Click" Text="<%$ Resources:LocalizedText,StartDataExtraction %>" />
        </li>
<%--        <li>
            <asp:Label runat="server" AssociatedControlID="btnRunODMv132Unloading">ODM файл версии 1.3.2</asp:Label>
            <asp:LinkButton runat="server" ID="btnRunODMv132Unloading" OnClick="btnRunODMv132Unloading_Click" Text="Запустить процесс выгрузки" />
        </li>--%>
    </ul>
    <asp:GridView runat="server" ID="gvUnloadingFiles" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvUnloadingFiles_RowDeleting">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="FileName" HeaderText="<%$ Resources:LocalizedText,NName %>" DataFormatString="{0:dd.MM.yyyy}" />
            <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:LocalizedText,CreatedBy %>" />
            <asp:BoundField DataField="CreatedDate" HeaderText="<%$ Resources:LocalizedText,CreationDate %>" />
            <asp:BoundField DataField="FileSize" HeaderText="<%$ Resources:LocalizedText,Size_byte %>" />

            <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Download %>">
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="btnDownload" ImageUrl="~/Images/download.png" OnClick="btnDownload_Click" CommandArgument="<%# (Container.DataItem as EDC.Models.UnloadingFile).UnloadingFileID %>" Text="Скачать" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Remove %>">
                <ItemTemplate>
                    <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/Images/delete.png" Text="<%$ Resources:LocalizedText,Remove %>" OnClientClick="return confirm('Вы уверены, что хотите удалить данный файл?')" />
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
