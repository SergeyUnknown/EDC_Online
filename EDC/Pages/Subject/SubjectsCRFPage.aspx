﻿<%@ Page Title="Просмотр ИРК субъекта" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsCRFPage.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsCRFPage" %>

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

    <script type="text/javascript">
        function setChanged() {
            document.getElementById('inHide').innerHTML = 'true';
        }
    </script>

    <script type="text/javascript">
        function checkChanged(url) {
            if ($("#inHide").text() != '') {
                if (confirm('Имеются не сохранённые данные, Вы уверены, что хотите покинуть форму?')) {
                    window.location.href = url;
                }
            }
            else
                window.location.href = url;
        }
    </script>

    <asp:UpdatePanel runat="server" ID="up1">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCreateNote" />
        </Triggers>
        <ContentTemplate>
            <asp:Label runat="server" ID="lbInfo" CssClass="SubjectPageInfo" />

            <input id="inHide" style="display: none" />

            <asp:Button runat="server" ID="btnPrevSubject" CssClass="prevSubject" Text="Предыдущий субъект" CausesValidation="false" />
            <asp:Button runat="server" ID="btnPrevCRFInEvent" CssClass="prevEvent" Text="Предыдущая форма" CausesValidation="false" />
            <%-- располагаться выше таблицы в правом углу--%>
            <asp:Button runat="server" ID="btnCheckAll" Text="Сверить" Visible="false" OnClick="btnCheckAll_Click" />
            <asp:Button runat="server" ID="btnApproved" Text="Подписать" Visible="false" OnClick="btnApproved_Click" />
            <asp:Button runat="server" ID="btnEnd" Text="Ввод данных завершен" Visible="false" OnClick="btnEnd_Click" />

            <ajaxToolkit:TabContainer runat="server" ID="tcCRF" OnActiveTabChanged="tcCRF_ActiveTabChanged" AutoPostBack="true">
            </ajaxToolkit:TabContainer>

            <asp:Button runat="server" ID="btnNextCRFInEvent" CssClass="nextEvent" Text="Следующая форма" CausesValidation="false" />
            <asp:Button runat="server" ID="btnNextSubject" CssClass="nextSubject" Text="Следующий субъект" CausesValidation="false" />


            <asp:Panel runat="server" ID="pnlModalPopup" Style="display: none">
                <asp:GridView runat="server" ID="gvNotes" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDataBound="gvNotes_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="CreationDate" HeaderText="Дата создания" />
                        <asp:BoundField DataField="FromUser" HeaderText="От" />
                        <asp:BoundField DataField="PreviousNote.Header" HeaderText="Ответ на" />
                        <asp:BoundField DataField="Header" HeaderText="Заголовок" />
                        <asp:BoundField DataField="Text" HeaderText="Текст" />
                        <asp:BoundField DataField="Status" HeaderText="Статус" />

                        <%--Ответить--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Ответить" OnClick="btnAnswer_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--Закрыть заметку--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Закрыть" OnClick="btnCloseNote_Click" />
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

                <asp:Button runat="server" ID="btnCreateNote" Text="Создать заметку" OnClick="btnCreateNote_Click" />
                <div runat="server" id="divCreate" visible="false">
                    <asp:Label runat="server" AssociatedControlID="tbHeader" ID="lbHeader">Заголовок</asp:Label>
                    <asp:TextBox runat="server" ID="tbHeader" />

                    <asp:Label runat="server" AssociatedControlID="tbNoteText" ID="lbNoteText">Текст заметки</asp:Label>
                    <asp:TextBox runat="server" ID="tbNoteText" TextMode="MultiLine" />
                    </br>
                </div>
                <asp:Button runat="server" ID="btnSaveWindow" OnClick="btnSaveWindow_Click" Visible="false" Text="Сохранить" />
                <asp:Button runat="server" ID="btnCloseWindow" Text="Закрыть" />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
