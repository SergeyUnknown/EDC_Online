<%@ Page Title="Субъекты" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Subjects.aspx.cs" Inherits="EDC.Pages.Subject.Subjects" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function filter() {
            var $dateMin = $("#<%: tbDateMin.ClientID  %>").val();
            var $dateMax = $("#<%: tbDateMax.ClientID  %>").val();
            var $createdBy = $("#<%: tbCreatedBy.ClientID %>").val();
            var $page = $("#<%: tbPage.ClientID %>").val();
            var urlParams = "";

            if ($dateMin != "")
                urlParams = urlParams.concat('&dateMin=' + $dateMin);

            if ($dateMax != "")
                urlParams = urlParams.concat('&dateMax=' + $dateMax);

            if ($createdBy != "")
                urlParams = urlParams.concat('&createdBy=' + $createdBy);

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
<asp:Content runat="server" ContentPlaceHolderID="LegendPlace">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Label runat="server" ID="lblDateMin" AssociatedControlID="tbDateMin" Text="Дата включения от" />
            <asp:TextBox runat="server" ID="tbDateMin" />
            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateMin" Format="dd.MM.yyyy" TodaysDateFormat="dd.MM.yyyy" />

            <asp:Label runat="server" ID="lblDateMax" AssociatedControlID="tbDateMax" Text="до" />
            <asp:TextBox runat="server" ID="tbDateMax" />
            <ajaxToolkit:CalendarExtender runat="server" TargetControlID="tbDateMax" Format="dd.MM.yyyy" TodaysDateFormat="dd.MM.yyyy" />

            <asp:Label runat="server" ID="lblCreatedBy" Text="Создал:" AssociatedControlID="tbCreatedBy" />
            <asp:TextBox runat="server" ID="tbCreatedBy" />

            <asp:Label runat="server" ID="lblPage" AssociatedControlID="tbPage" Text="Страница" />
            <asp:TextBox runat="server" ID="tbPage" />
            <ajaxToolkit:NumericUpDownExtender ID="nudPage" runat="server" TargetControlID="tbPage" Minimum="1" Width="141" />

            <asp:Button runat="server" ID="btnSearch" OnClientClick="filter()" OnClick="btnSearch_Click" Text="Поиск" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:GridView runat="server" ID="gvSubjects" CellPadding="4" ForeColor="#333333" GridLines="Both" AutoGenerateColumns="False" OnRowDeleting="gvSubjects_RowDeleting" OnRowEditing="gvSubjects_RowEditing" OnRowDataBound="gvSubjects_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
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
