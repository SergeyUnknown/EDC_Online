<%@ Page Title="Медицинские центры" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MedicalCenters.aspx.cs" Inherits="EDC.Pages.MedicalCenter.MedicalCenters" %>

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
    <asp:Label CssClass="lable_error" ID="ErrorLable" runat="server" Text="" Width="950px" Visible="false"></asp:Label>
    <asp:Table runat="server" CssClass="tableSearch">
        <asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="tbName" runat="server" placeholder="Название" Width="200"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="tbRegion" runat="server" placeholder="Регион" Width="200"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="tbCity" runat="server" placeholder="Город" Width="200"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:LinkButton ID="btnSearch" runat="server" Text="Искать" OnClick="btnSearch_Click"></asp:LinkButton>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:GridView runat="server" class="tableMedicalCenters" ID="gvMedicalCenters" AutoPostBack="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" OnRowEditing="gvMedicalCenters_RowEditing" OnRowDeleting="gvMedicalCenters_RowDeleting">
        <AlternatingRowStyle BackColor="White" />
        <Columns>

            <asp:BoundField HeaderText="Номер" DataField="Number" />
            <asp:BoundField HeaderText="<%$ Resources:LocalizedText,NName %>" DataField="Name" />
            <asp:TemplateField HeaderText="Адрес">
                <ItemTemplate>
                    <%# Eval("Country") %>, <%# Eval("Region") %>, <%# Eval("City") %>, <%# Eval("Street") %>, <%# Eval("House") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Главный исследователь" DataField="PrincipalInvestigator" />
            <asp:BoundField HeaderText="Телефон" DataField="Phone" />

            <asp:CommandField HeaderText="<%$ Resources:LocalizedText,Actions %>" ShowEditButton="true" ButtonType="Image" ShowDeleteButton="true" EditText="<%$ Resources:LocalizedText,Edit %>" DeleteText="<%$ Resources:LocalizedText,Remove %>" DeleteImageUrl="~/Images/delete.png" EditImageUrl="~/Images/edit.png"/>

        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#78C1E9" Font-Bold="True" ForeColor="White" />
        <PagerSettings Mode="NumericFirstLast" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>

    <asp:ImageButton runat="server" ImageUrl="~/Images/table-row-plus-after.png" PostBackUrl="~/MedicalCenters/Create" CssClass="onlyAdd" />

</asp:Content>
