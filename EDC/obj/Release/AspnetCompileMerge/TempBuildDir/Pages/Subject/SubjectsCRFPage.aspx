<%@ Page Title="Заполнение ИРК пациента №" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsCRFPage.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsCRFPage" %>

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

            <table class="subjectCRFTable" runat="server">
                <tbody>
                    <tr>
                        <th class="EventArrow"></th>
                        <th class="midTD">
                            <asp:Label runat="server" ID="lbInfo" CssClass="SubjectPageInfo" />
                            <input id="inHide" style="display: none" />

                            <table runat="server" id="tQueryStatistic" class="tableQueryStatistic">
                                <tbody>
                                    <tr>
                                        <th colspan="4"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Queries %>" /></th>
                                    </tr>
                                    <tr>
                                        <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,New %>" /><input type="submit" class="notes notesNew" value="" /></th>
                                        <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Updated %>" /><input type="submit" class="notes notesUpdate" value="" /></th>
                                        <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Closed %>" /><input type="submit" class="notes notesClose" value="" /></th>
                                        <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Total %>" /></th>
                                    </tr>
                                    <tr>
                                        <td>0</td>
                                        <td>0</td>
                                        <td>0</td>
                                        <td>0</td>
                                    </tr>
                                </tbody>
                            </table>

                        </th>
                        <th class="EventArrow"></th>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button runat="server" ID="btnPrevSubject" Text="<%$ Resources:LocalizedText,PreviousSubject %>" CssClass="prevSubject" CausesValidation="false" />
                            <%-- располагаться выше таблицы в правом углу--%>
                            <div style="text-align: right">
                                <input runat="server" id="btnStatus" disabled="disabled" />
                                <asp:Label runat="server" ID="lblStatus" />
                            </div>
                            <div style="text-align: right">
                                <asp:Button runat="server" ID="btnEdit" Text="<%$ Resources:LocalizedText,Edit %>" Visible="false" OnClick="btnEdit_Click" EnableViewState="false" />
                                <asp:Button runat="server" ID="btnCheckAll" Text="<%$ Resources:LocalizedText,SDVed %>" Visible="false" OnClick="btnCheckAll_Click" EnableViewState="false" />
                                <asp:Button runat="server" ID="btnApproved" Text="<%$ Resources:LocalizedText,Sign %>" Visible="false" OnClick="btnApproved_Click" EnableViewState="false" />
                                <asp:Button runat="server" ID="btnEnd" Text="<%$ Resources:LocalizedText,DataEntryIsCompleted %>" Visible="false" OnClick="btnEnd_Click" EnableViewState="false" />
                            </div>
                        </td>
                        <td></td>
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
                            <asp:Button runat="server" ID="btnNextSubject" Text="<%$ Resources:LocalizedText,NextSubject %>" CssClass="nextSubject" CausesValidation="false" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <%--ПопАп--%>
            <asp:Panel runat="server" ID="pnlModalPopup" Style="display: none" CssClass="popUp">

                <asp:GridView runat="server" ID="gvNotes" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDataBound="gvNotes_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:LocalizedText,CreationDate %>" />
                        <asp:BoundField DataField="From" HeaderText="<%$ Resources:LocalizedText,CreatedBy %>" />
                        <asp:BoundField DataField="Header" HeaderText="<%$ Resources:LocalizedText,QueryDescription %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Status %>">
                            <ItemTemplate>
                                <%# EDC.Core.Core.QueriesStatus((EDC.Core.QueryStatus)Eval("Status"))  %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="To" HeaderText="<%$ Resources:LocalizedText,AssignedUser %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,LastMessage %>">
                            <ItemTemplate>
                                <%# ((List<EDC.Models.QueryMessage>)Eval("Messages")).Last().Text  %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClosedDate" HeaderText="<%$ Resources:LocalizedText,ResolutionDate %>" />
                        <asp:BoundField DataField="ClosedBy" HeaderText="<%$ Resources:LocalizedText,ResolvedBy %>" />
                        <asp:BoundField DataField="ClosedText" HeaderText="<%$ Resources:LocalizedText,ResolutionDescription %>" />

                        <%--Просмотреть--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Messages %>" OnClick="btnPopUpSeeMessage_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--Ответить--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Answer %>" OnClick="btnAnswer_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--Закрыть заметку--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" Text="<%$ Resources:LocalizedText,CloseT %>" OnClick="btnCloseNote_Click" />
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
                <br />
                <div id="divMessageLog" runat="server" style="text-align:center; display:none;">
                    <a onclick="<%: string.Format("$('#{0}').css('display', 'none')",divMessageLog.ClientID)  %>" style="width:100%; cursor:pointer; text-decoration:none" ><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,HideMessages %>" /></a>
                    <table id="tQueries" runat="server">
                        <tbody>
                            <tr style="color: white;">
                                <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,CreationDate %>" /></th>
                                <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,QueryDescription %>" /></th>
                                <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,From %>" /></th>
                                <th><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Text %>" /></th>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <asp:Button runat="server" ID="btnCreateNote" Text="<%$ Resources:LocalizedText,CreateQuery %>" OnClick="btnCreateNote_Click" />
                <div runat="server" id="divCreate" visible="false">
                    <asp:Label runat="server" AssociatedControlID="tbHeader" ID="lbHeader" Text="<%$ Resources:LocalizedText,MessageSubject %>"/>
                    <asp:TextBox runat="server" ID="tbHeader" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvHeader" ValidationGroup="queryMessage" ControlToValidate="tbHeader" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic"/>                   

                    <asp:Label runat="server" AssociatedControlID="tbNoteText" ID="lbNoteText"><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Text %>" /></asp:Label>
                    <asp:TextBox runat="server" ID="tbNoteText" TextMode="MultiLine" />
                    <asp:RequiredFieldValidator runat="server" ValidationGroup="queryMessage" ControlToValidate="tbNoteText" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic"/>                   
                    </br>
                </div>
                <asp:Button runat="server" ID="btnSaveWindow" OnClick="btnSaveWindow_Click" Visible="false" Text="<%$ Resources:LocalizedText,Save %>" ValidationGroup="queryMessage" />
                <asp:Button runat="server" ID="btnCloseWindow" Text="<%$ Resources:LocalizedText,Close %>" ValidateRequestMode="Disabled" />
            </asp:Panel>

            <%--Причина редактирования--%>
            <asp:Panel runat="server" ID="pnlEditReason" Style="display: none" CssClass="popUp">
                <div runat="server" id="divEditReason">
                    <asp:Label runat="server" AssociatedControlID="tbEditReason" ID="lblEditReason" Text="<%$ Resources:LocalizedText,ReasonForCRFChange %>" />
                    <asp:TextBox runat="server" ID="tbEditReason" TextMode="MultiLine" />
                    </br>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEditReason" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" Display="Dynamic" ValidationGroup="editReason" />
                </div>
                <asp:Button runat="server" ID="btnSaveEditReason" OnClick="btnSaveEditReason_Click" Text="<%$ Resources:LocalizedText,Save %>" ValidationGroup="editReason" />
                <asp:Button runat="server" ID="btnCloseEditReason" Text="<%$ Resources:LocalizedText,Close %>" />
            </asp:Panel>

            <%--Запрос Логин/Пароля--%>
            <asp:Panel runat="server" ID="pnlLogin" Style="display: none" CssClass="popUp">
                <asp:Label runat="server" ID="lblLoginStatus" Visible="false" Text="<%$ Resources:LocalizedText,Error_IncorrectUserNameOrPassword %>" />
                <div runat="server" id="div1" style="text-align: center; width: 250px; height: 180px; padding-top: 15px;">
                    <asp:Label runat="server" AssociatedControlID="tbUserName" ID="lblUserName" Font-Size="Medium" Text="<%$ Resources:LocalizedText,UserName %>" />
                    <asp:TextBox runat="server" ID="tbUserName" EnableViewState="false" Style="display: block; margin: 0 auto; text-align: center; width: 150px;" />
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" ControlToValidate="tbUserName" ValidationGroup="pnlLogin" />
                    <asp:Label runat="server" AssociatedControlID="tbPassword" ID="lblPassword" Font-Size="Medium" Text="<%$ Resources:LocalizedText,Password %>" />
                    <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" EnableViewState="false" Style="display: block; margin: 0 auto; text-align: center; width: 150px;" />
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" ControlToValidate="tbPassword" ValidationGroup="pnlLogin" />
                </div>
                <asp:Button runat="server" ID="btnOkLogin" OnClick="btnOkLogin_Click" Text="Ок" ValidationGroup="pnlLogin" Style="width: 70px; margin-left: 35px;" />
                <asp:Button runat="server" ID="btnCloseLogin" Text="<%$ Resources:LocalizedText,Cancel %>" Style="width: 70px; margin-left: 35px;" />
            </asp:Panel>
                <script type="text/javascript">
                    $('.CRFItemHeader').parent().css('text-align', 'center');
                    $('.CRFItemsubHeader').parent().css('text-align', 'center');
                    $('.CRFItemsubHeader').parent().css('border-right', 'none');
                </script>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
