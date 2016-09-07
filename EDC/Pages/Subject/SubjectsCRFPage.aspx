<%@ Page Title="Просмотр ИРК субъекта" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsCRFPage.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsCRFPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server" ID="up1">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Button runat="server" ID="btnPrevSubject" Text="Предыдущий субъект" ToolTip="Предыдущий субъект" />
            <asp:Button runat="server" ID="btnPrevCRFInEvent" Text="Предыдущая CRF" ToolTip="Предыдущая CRF" />

            <ajaxToolkit:TabContainer runat="server" ID="tcCRF">
            </ajaxToolkit:TabContainer>

            <asp:Button runat="server" ID="btnNextCRFInEvent" Text="Следующая CRF" ToolTip="Следующая CRF" />
            <asp:Button runat="server" ID="btnNextSubject" Text="Следующий субъект" ToolTip="Следующий субъект" />


            <asp:Panel runat="server" ID="pnlModalPopup" Style="display: none">
                <asp:GridView runat="server" ID="gvNotes" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="CreationDate" HeaderText="Дата создания" />
                        <asp:BoundField DataField="FromUser" HeaderText="От" />
                        <asp:BoundField DataField="Header" HeaderText="Заголовок" />
                        <asp:BoundField DataField="Text" HeaderText="Текст" />
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

                <asp:Label runat="server" AssociatedControlID="tbHeader">Заголовок</asp:Label>
                <asp:TextBox runat="server" ID="tbHeader" />
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbHeader"
                    CssClass="field-validation-error" ErrorMessage="Поле обязательно для заполнения" />--%>
                <asp:Label runat="server" AssociatedControlID="tbNoteText">Текст заметки</asp:Label>
                <asp:TextBox runat="server" ID="tbNoteText" TextMode="MultiLine" />
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNoteText"
                    CssClass="field-validation-error" ErrorMessage="Поле обязательно для заполнения" />--%>
                </br>
                <asp:Button runat="server" ID="btnSaveWindow" OnClick="btnSaveWindow_Click" Text="Сохранить" />
                <asp:Button runat="server" ID="btnCloseWindow" Text="Отмена" />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
