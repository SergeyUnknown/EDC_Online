<%@ Page Title="Настройка события" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigurationEvent.aspx.cs" Inherits="EDC.Pages.Event.ConfigurationEvent" %>
<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function labelNameClick(elemName)
        {
            $elem = $(elemName);
            if ($elem.attr('checked') == undefined) {
                //$elem.attr('checked', 'checked');
                $elem.click();
            }
            else {
                //$elem.removeAttr('checked');
                $elem.click();
            }
        }
    </script>
</asp:Content>

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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div runat="server" id="divAddedCRF">
                <asp:GridView runat="server" ID="gvCRFs" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvCRFs_RowDeleting" OnSelectedIndexChanging="gvCRFs_SelectedIndexChanging" OnRowDataBound="gvCRFs_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPosition" runat="server" OnSelectedIndexChanged="ddlPosition_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,NName %>">
                            <ItemTemplate>
                                <%# Eval("CRF.RussianName") != null ? Eval("CRF.RussianName") : Eval("CRF.Name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField HeaderText="<%$ Resources:LocalizedText,Actions %>" ButtonType="Image" ShowDeleteButton="True" ShowSelectButton="true" DeleteText="<%$ Resources:LocalizedText,Remove %>" SelectText="Просмотреть" SelectImageUrl="~/Images/magnify.png" DeleteImageUrl="~/Images/delete.png"/>
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

                <asp:Button runat="server" ID="btnAddCRF" Text="<%$ Resources:LocalizedText,Add %>" OnClick="btnAddCRF_Click"></asp:Button>
                <asp:Button runat="server" ID="btnSave" PostBackUrl="~/Events" Text="<%$ Resources:LocalizedText,Save %>" />
            </div>

            <div runat="server" id="divAddCRF">
                <asp:GridView runat="server" ID="gvNewCRFs" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDataBound="gvNewCRFs_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbAdd" OnCheckedChanged="cbAdd_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPosition"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,NName %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblName"><%# Eval("RussianName") != null ? Eval("RussianName") : Eval("Name") %></asp:Label>
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

                <asp:Button runat="server" ID="btnSaveAdd" Text="<%$ Resources:LocalizedText,Save %>" OnClick="btnSaveAdd_Click"></asp:Button>
                <asp:Button runat="server" ID="btnSaveCancel" Text="<%$ Resources:LocalizedText,Cancel %>" OnClick="btnSaveCancel_Click"></asp:Button>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
