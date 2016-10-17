<%@ Page Title="Аудит данных" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Audits.aspx.cs" Inherits="EDC.Pages.Audit.Audits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function filter() {
            var $username = $("#<%: tbUserName.ClientID  %>").val();
            var $dateMin = $("#<%: tbDateMin.ClientID  %>").val();
            var $dateMax = $("#<%: tbDateMax.ClientID  %>").val();
            var $page = $("#<%: tbPage.ClientID %>").val();
            var urlParams = "";
            if ($username != "")
                urlParams = '&userName=' + $username;

            if ($dateMin != "")
                urlParams = urlParams.concat('&dateMin=' + $dateMin);

            if ($dateMax != "")
                urlParams = urlParams.concat('&dateMax=' + $dateMax);

            if ($page != "")
                urlParams = urlParams.concat('&page=' + $page);

            if (urlParams != "")
            {
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
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LegendPlace" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Label runat="server" ID="lblUserName" AssociatedControlID="tbUserName" Text="Имя пользователя:" />
            <asp:TextBox runat="server" ID="tbUserName" />

            <asp:Label runat="server" ID="lblDateMin" AssociatedControlID="tbDateMin" Text="Дата от" />
            <asp:TextBox runat="server" ID="tbDateMin" />
            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateMin" Format="dd.MM.yyyy" TodaysDateFormat="dd.MM.yyyy" />

            <asp:Label runat="server" ID="lblDateMax" AssociatedControlID="tbDateMax" Text="до" />
            <asp:TextBox runat="server" ID="tbDateMax" />
            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateMax" Format="dd.MM.yyyy" TodaysDateFormat="dd.MM.yyyy" />

            <asp:Label runat="server" ID="lblPage" AssociatedControlID="tbPage" Text="Страница:" />
            <asp:TextBox runat="server" ID="tbPage"/>
            <ajaxToolkit:NumericUpDownExtender ID="nudPage" runat="server" TargetControlID ="tbPage" Minimum="1" Width="141" />
            <asp:Button runat="server" ID="btnSearch" OnClientClick="filter()" OnClick="btnSearch_Click" Text="Поиск" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblMessage" Visible="false" />
            <asp:GridView runat="server" ID="gvAudits" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="UserName" HeaderText="Пользователь" />
                    <asp:BoundField DataField="ActionDate" HeaderText="Дата" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="ActionDate" HeaderText="Время" DataFormatString="{0:HH:mm:ss}" />
                    <asp:TemplateField HeaderText="Тип">
                        <ItemTemplate><%# EDC.Core.GetAuditActionTypeRusName((EDC.Core.AuditActionType)Eval("ActionType")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Действие">
                        <ItemTemplate><%# EDC.Core.GetAuditChangesTypeRusName((EDC.Core.AuditChangesType)Eval("ChangesType")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Subject.Number" HeaderText="Субъект" />
                    <asp:BoundField DataField="SubjectEvent.Event.Name" HeaderText="Событие" />
                    <asp:TemplateField HeaderText="ИРК">
                        <ItemTemplate><%# GetCRFName((EDC.Models.SubjectsCRF)Eval("SubjectCRF")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FieldName" HeaderText="Поле" />
                    <asp:BoundField DataField="OldValue" HeaderText="Старое значение" />
                    <asp:BoundField DataField="NewValue" HeaderText="Новое значение" />
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
