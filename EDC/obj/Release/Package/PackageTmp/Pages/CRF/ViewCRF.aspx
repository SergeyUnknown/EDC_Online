<%@ Page Title="Просмотр CRF" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCRF.aspx.cs" Inherits="EDC.Pages.CRF.ViewCRF" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function onButtonClick(index) {
            var $sections = $('#<%: gvSections.ClientID %>');
            var $groups = $('#<%: gvGroups.ClientID %>');
            var $items = $('#<%: gvCRF_Fields.ClientID %>');
            switch (index) {
                case 0:
                    $sections.show();
                    $groups.hide();
                    $items.hide();
                    break;
                case 1:
                    $sections.hide();
                    $groups.show();
                    $items.hide();
                    break;
                case 2:
                    $sections.hide();
                    $groups.hide();
                    $items.show();
                    break;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
    <script type="text/javascript">
        $(document).ready(function () {
            var $sections = $("#<%: gvSections.ClientID %>");
            var $groups = $("#<%: gvGroups.ClientID %>");
            var $items = $("#<%: gvCRF_Fields.ClientID %>");
            $sections.show();
            $groups.hide();
            $items.hide();
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table style="width:50%">
                <tbody>
                    <tr>
                       <td>OID:</td>
                        <td><asp:Label runat="server" ID="lblOID"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <br />
            <input type="button" onclick="onButtonClick(0)" id="btnSections" value="Секции" />
            <input type="button" onclick="onButtonClick(1)" id="btnGroups" value="Группы" />
            <input type="button" onclick="onButtonClick(2)" id="btnItems" value="Итемы" />

            <asp:GridView runat="server" ID="gvSections" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Label" HeaderText="Лейбл" />
                    <asp:BoundField DataField="Title" HeaderText="Заголовок" />
                    <asp:BoundField DataField="Subtitle" HeaderText="Подзаголовок" />
                    <asp:BoundField DataField="Instructions" HeaderText="Инструкции" />
                    <asp:BoundField DataField="PageNumber" HeaderText="Страница" />
                    <asp:BoundField DataField="Border" HeaderText="Рамка" />
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

            <asp:GridView runat="server" ID="gvGroups" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Identifier" HeaderText="Идентификатор" />
                    <asp:BoundField DataField="Header" HeaderText="Заголовок" />
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

            <asp:GridView runat="server" ID="gvCRF_Fields" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Identifier" HeaderText="Идентификатор" />
                    <asp:BoundField DataField="DescriptionLabel" HeaderText="Описание" />
                    <asp:BoundField DataField="LeftItemText" HeaderText="Текст до" />
                    <asp:BoundField DataField="Units" HeaderText="Ед. Измерения" />
                    <asp:BoundField DataField="RightItemText" HeaderText="Текст после" />
                    <asp:BoundField DataField="Section.Label" HeaderText="Секция" />
                    <asp:BoundField DataField="Group.Identifier" HeaderText="Группа" />
                    <asp:BoundField DataField="ResponseType" HeaderText="Тип запроса" />
                    <asp:BoundField DataField="ResponseValuesOrCalculation" HeaderText="Варианты ответа" />
                    <asp:BoundField DataField="DataType" HeaderText="Тип данных" />
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
