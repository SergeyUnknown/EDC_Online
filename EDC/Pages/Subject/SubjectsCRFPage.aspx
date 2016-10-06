<%@ Page Title="Настройка ИРК субъекта" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsCRFPage.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsCRFPage" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function setChanged() {
            document.getElementById('inHide').innerHTML = 'true';
        }

        function checkChanged(url) {
            if ($("#inHide").text() != '') {
                if (confirm('Имеются не сохранённые данные, Вы уверены, что хотите покинуть форму?')) {
                    window.location.href = url;
                }
            }
            else
                window.location.href = url;
        }

        function setActiveTab(sender, args) {
            $.cookie("activeTabIndex", sender.get_activeTabIndex(), { path: '/' });

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

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:UpdatePanel runat="server" ID="up1">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCreateNote" />
        </Triggers>
        <ContentTemplate>
<<<<<<< HEAD
            <table class="subjectCRFTable">
                <tbody>
                    <tr>
                        <th class="EventArrow"></th>
                        <th class="midTD">
                            <asp:Label runat="server" ID="lbInfo" CssClass="SubjectPageInfo" />

                            <input id="inHide" style="display: none" />
                        </th>
                        <th class="EventArrow"></th>
                    </tr>
                    <tr>
                        <td>

                        </td>
                        <td >
                            <asp:Button runat="server" ID="btnPrevSubject" Text="Предыдущий субъект" CssClass="prevSubject" CausesValidation="false" />
                        </td>
                        <td>
                            <%-- располагаться выше таблицы в правом углу--%>
                            <asp:Button runat="server" ID="btnCheckAll" Text="Сверить" Visible="false" OnClick="btnCheckAll_Click" EnableViewState="false" />
                            <asp:Button runat="server" ID="btnApproved" Text="Подписать" Visible="false" OnClick="btnApproved_Click" EnableViewState="false" />
                            <asp:Button runat="server" ID="btnEnd" Text="Ввод данных завершен" Visible="false" OnClick="btnEnd_Click" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="EventArrow">
                            <asp:Button runat="server" ID="btnPrevCRFInEvent" CssClass="prevEvent" CausesValidation="false" />
                        </td>
                        <td>
                            <ajaxToolkit:TabContainer runat="server" CssClass="tcCRF" ID="tcCRF" OnClientActiveTabChanged="setActiveTab" AutoPostBack="true">
                            </ajaxToolkit:TabContainer>
                        </td>
                        <td class="EventArrow">
                            <asp:Button runat="server" ID="btnNextCRFInEvent" CssClass="nextEvent" CausesValidation="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btnNextSubject" Text="Следующий субъект" CssClass="nextSubject" CausesValidation="false" />
                        </td>
                    </tr>
                </tbody>
            </table>
=======

            <asp:Button runat="server" ID="btnPrevCRFInEvent" CssClass="prevEvent" CausesValidation="false" />

            <div id="centerDiv">
                <asp:Label runat="server" ID="lbInfo" CssClass="SubjectPageInfo" />
                <input id="inHide" style="display: none" />
                <asp:Button runat="server" ID="btnPrevSubject" Text="Предыдущий субъект" CssClass="prevSubject" CausesValidation="false" />
                <%-- располагаться выше таблицы в правом углу--%>
                <div style="text-align:right">
                    <asp:Button runat="server" ID="btnCheckAll" Text="Сверить" Visible="false" OnClick="btnCheckAll_Click" EnableViewState="false" />
                    <asp:Button runat="server" ID="btnApproved" Text="Подписать" Visible="false" OnClick="btnApproved_Click" EnableViewState="false" />
                    <asp:Button runat="server" ID="btnEnd" Text="Ввод данных завершен" Visible="false" OnClick="btnEnd_Click" EnableViewState="false" />
                </div>
                <ajaxToolkit:TabContainer runat="server" CssClass="tcCRF" ID="tcCRF" OnClientActiveTabChanged="setActiveTab" AutoPostBack="true">
                </ajaxToolkit:TabContainer>
                <asp:Button runat="server" ID="btnNextSubject" Text="Следующий субъект" CssClass="nextSubject" CausesValidation="false" />
            </div>

            <asp:Button runat="server" ID="btnNextCRFInEvent" CssClass="nextEvent" CausesValidation="false" />

>>>>>>> refs/remotes/origin/master
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
