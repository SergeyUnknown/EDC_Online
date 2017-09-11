<%@ Page Title="<%$ Resources:LocalizedText,Queries %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Querys.aspx.cs" Inherits="EDC.Pages.Query.Querys" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function filter() {
            var $param1 = $("#<%: tbSubjectNumber.ClientID  %>").val();
            var $param2 = $("#<%: tbForUser.ClientID  %>").val();
            var $param3 = $("#<%: ddlQueryStatus.ClientID  %>").val();
            var urlParams = "";
            if ($param1 != "")
                urlParams = '&subjectNumber=' + $param1;

            if ($param2 != "")
                urlParams = urlParams.concat('&forUser=' + $param2);

            if ($param3 != "")
                urlParams = urlParams.concat('&queryStatus=' + $param3);

            if (urlParams != "") {
                urlParams = urlParams.slice(1, urlParams.length);
                urlParams = "?" + urlParams;
            }

            if (urlParams != "")
                if (window.location.href.indexOf('\?') >= 0)
                    window.history.pushState(null, null, window.location.href.slice(0, window.location.href.indexOf('\?')) + urlParams);
                else
                    window.history.pushState(null, null, window.location.href + urlParams);
            else
                if (window.location.href.indexOf('\?') >= 0)
                    window.history.pushState(null, null, window.location.href.slice(0, window.location.href.indexOf('\?')));
                else
                    window.history.pushState(null, null, window.location.href);
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

<asp:Content ContentPlaceHolderID="LegendPlace" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Label runat="server" ID="lblSubjectNumber" AssociatedControlID="tbSubjectNumber" Text="<%$ Resources:LocalizedText,SubjectNumber %>" />
            <asp:TextBox runat="server" ID="tbSubjectNumber" />

            <asp:Label runat="server" ID="lblForUser" AssociatedControlID="tbForUser" Text="Назначенный пользователь:" />
            <asp:TextBox runat="server" ID="tbForUser" />

            <asp:Label runat="server" ID="lblQueryStatus" AssociatedControlID="ddlQueryStatus" Text="<%$ Resources:LocalizedText,Status %>" />
            <asp:DropDownList runat="server" ID="ddlQueryStatus">
                <asp:ListItem Text="<%$ Resources:LocalizedText,Select %>" Value="" />
                <asp:ListItem Text="<%$ Resources:LocalizedText,New %>" Value="New" />
                <asp:ListItem Text="<%$ Resources:LocalizedText,Updated %>" Value="Updated" />
                <asp:ListItem Text="<%$ Resources:LocalizedText,Closed %>" Value="Closed" />
            </asp:DropDownList>

            <asp:Button runat="server" ID="btnSearch" OnClientClick="filter()" OnClick="btnSearch_Click" Text="<%$ Resources:LocalizedText,Search %>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
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
            <br />
            <asp:GridView runat="server" ID="gvNotes" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
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
                    <asp:BoundField DataField="Subject.Number" HeaderText="<%$ Resources:LocalizedText,SubjectNumber %>" />
                    <asp:BoundField DataField="Event.Name" HeaderText="<%$ Resources:LocalizedText,Event %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,CRF %>">
                        <ItemTemplate>
                            <%# GetCRFName((EDC.Models.CRF)Eval("CRF"))  %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CRFItem.DescriptionLabel" HeaderText="<%$ Resources:LocalizedText,Field %>" />
                    <asp:BoundField DataField="SubjectItem.Value" HeaderText="<%$ Resources:LocalizedText,Value %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,LastMessage %>">
                        <ItemTemplate>
                            <%# (Eval("Messages") as List<EDC.Models.QueryMessage>).Last().Text  %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ImageUrl="~/Images/magnify.png" OnClientClick=<%# string.Format("window.open(\'{0}\')", GetRedirectURL((EDC.Models.SubjectsItem)Eval("SubjectItem"),(EDC.Models.Query)Container.DataItem)) %> />
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

            <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
