<%@ Page Title="Субъекты" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Subjects.aspx.cs" Inherits="EDC.Pages.Subject.Subjects" %>

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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:GridView runat="server" ID="gvSubjects" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvSubjects_RowDeleting" OnRowEditing="gvSubjects_RowEditing" OnRowDataBound="gvSubjects_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="SubjectID" HeaderText="ID" />
            <asp:TemplateField HeaderText="Номер">
                <ItemTemplate><%# IsDeleted((string)Eval("Number"),(bool)Eval("IsDeleted")) %></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MedicalCenter.Name" HeaderText="Мед. Центр" />
            <asp:BoundField DataField="InclusionDate" HeaderText="Дата включения" DataFormatString="{0:dd.MM.yyyy}" />
            <asp:BoundField DataField="CreatedBy" HeaderText="Создал" />
            <asp:BoundField DataField="CreationDate" HeaderText="Дата создания" />

            <%--Редактировать--%>
            <asp:CommandField HeaderText="" ButtonType="Image" ShowEditButton="True" EditText="Редактировать" EditImageUrl="~/Images/pencil-box (1).png" />

            <%--Удалить--%>
            <asp:CommandField HeaderText="" ButtonType="Image" ShowDeleteButton="True" DeleteText="Удалить" DeleteImageUrl="~/Images/delete (3).png" />

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
    <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
</asp:Content>
