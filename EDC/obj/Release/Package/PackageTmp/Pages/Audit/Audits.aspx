<%@ Page Title="<%$ Resources:LocalizedText,StudyAuditLog %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Audits.aspx.cs" Inherits="EDC.Pages.Audit.Audits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .navButton a{
            text-decoration:none;
        }
        .navButton img {
            width: 24px;
            height: 24px;
            padding: 1px;
            background-color: #7ac0da;
            border-radius: 2px;
            transition: border-radius 0.1s ease;
        }
        .navButton img:hover {
            border-radius: 10px;
            background-color: #7EA2D4;
            box-shadow: 0 2px 2px 1px rgba(0, 0, 0, 0.26);
        }
    </style>
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

    <script type="text/javascript">
        function navButtonClick(e) {
            $tb = $("#<%: tbPage.ClientID%>");
            $max = "<%: nudPage.Maximum %>";
            switch(e.id)
            {
                case "LinkButtonFirstPage":
                    $tb.val('1');
                    break;
                case "LinkButtonPrevPage":
                    if ($tb.val() > 1)
                        $tb.val(parseInt($tb.val())-1);
                    break;
                case "LinkButtonNextPage":
                    if ($tb.val() < $max)
                        $tb.val(parseInt($tb.val())+1);
                    break;
                case "LinkButtonLastPage":
                    $tb.val($max);
                    break;
            }
            $("#<%: btnSearch.ClientID %>").click();
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
            <asp:Label runat="server" ID="lblUserName" AssociatedControlID="tbUserName" Text="<%$ Resources:LocalizedText,UserName %>" />
            <asp:TextBox runat="server" ID="tbUserName" />

            <asp:Label runat="server" ID="lblDateMin" AssociatedControlID="tbDateMin" Text="<%$ Resources:LocalizedText,DateSince %>" />
            <asp:TextBox runat="server" ID="tbDateMin" />
            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateMin" Format="dd.MM.yyyy" TodaysDateFormat="dd.MM.yyyy" />

            <asp:Label runat="server" ID="lblDateMax" AssociatedControlID="tbDateMax" Text="<%$ Resources:LocalizedText,till %>" />
            <asp:TextBox runat="server" ID="tbDateMax" />
            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateMax" Format="dd.MM.yyyy" TodaysDateFormat="dd.MM.yyyy" />

            <asp:Label runat="server" ID="lblPage" AssociatedControlID="tbPage" Text="<%$ Resources:LocalizedText,Page %>" />
            <asp:TextBox runat="server" ID="tbPage" />
            <ajaxToolkit:NumericUpDownExtender ID="nudPage" runat="server" TargetControlID="tbPage" Minimum="1" Width="141" />
            <asp:Button runat="server" ID="btnSearch" OnClientClick="filter()" OnClick="btnSearch_Click" Text="<%$ Resources:LocalizedText,Search %>" />
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
                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:LocalizedText,User %>" />
                    <asp:BoundField DataField="ActionDate" HeaderText="<%$ Resources:LocalizedText,Date %>" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="ActionDate" HeaderText="<%$ Resources:LocalizedText,Time %>" DataFormatString="{0:HH:mm:ss}" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Type %>">
                        <ItemTemplate><%# EDC.Core.Core.GetAuditActionTypeRusName((EDC.Core.AuditActionType)Eval("ActionType")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,Action %>">
                        <ItemTemplate><%# EDC.Core.Core.GetAuditChangesTypeRusName((EDC.Core.AuditChangesType)Eval("ChangesType")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Subject.Number" HeaderText="<%$ Resources:LocalizedText,Subject %>" />
                    <asp:BoundField DataField="SubjectEvent.Event.Name" HeaderText="<%$ Resources:LocalizedText,Event %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText,CRF %>">
                        <ItemTemplate><%# GetCRFName((EDC.Models.SubjectsCRF)Eval("SubjectCRF")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FieldName" HeaderText="<%$ Resources:LocalizedText,Field %>" />
                    <asp:BoundField DataField="OldValue" HeaderText="<%$ Resources:LocalizedText,OldValue %>" />
                    <asp:BoundField DataField="NewValue" HeaderText="<%$ Resources:LocalizedText,NewValue %>" />
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

            <div style="text-align: center;" class="navButton">
                <a id="LinkButtonFirstPage" onclick="navButtonClick(this)">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/step-backward-2.png" />
                </a>
                <a ID="LinkButtonPrevPage" onclick="navButtonClick(this)">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/step-backward.png" />
                </a>
                <a ID="LinkButtonNextPage" onclick="navButtonClick(this)">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/step-forward.png" />
                </a>
                <a ID="LinkButtonLastPage" onclick="navButtonClick(this)">
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/step-forward-2.png" />
                </a>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
