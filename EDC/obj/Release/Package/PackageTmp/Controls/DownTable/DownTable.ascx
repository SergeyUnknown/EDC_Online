<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownTable.ascx.cs" Inherits="EDC.Controls.DownTable" %>

<style>
    table.dt {
    width:100%;
}
td a img {
    width:25px;
    height:25px;
}

td.dtleft {
    width:20%;
}
td.dtcenter {
    width:40%;
    text-align:center;
}

td.dtcenter a img:hover{
    transform:scale(1.5);
}

td.dtright1 {
    width:25%;
    text-align:right;
    padding-right:0px;
}
td.dtrigth2 {
    width:15%;
    text-align:right;
}
</style>

<table class="dt">
        <tr>
            <td class="dtleft">
                <asp:HyperLink ID="hlButton" CssClass="addButton" runat="server" NavigateUrl="~/">""</asp:HyperLink>
            </td>
            <td class="dtcenter">
                <asp:LinkButton runat="server" ID="LinkButtonFirstPage"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/FirstPage.png" /></asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonPrevPage"><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/PreviousPage.png"/></asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonNextPage"><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/NextPage.png"/></asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonLastPage"><asp:Image ID="Image4" runat="server" ImageUrl="~/Images/LastPage.png"/></asp:LinkButton>
            </td>
            <td class="dtright1">
                <asp:DropDownList CssClass="pageCount" ID="ddlItemCount" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCount_SelectedIndexChanged"/>
                <asp:label ID="Label1" runat="server">Записей на страницу</asp:label>
            </td>
            <td class="dtrigth2">
                <asp:Label runat="server" ID="pageInfo"></asp:Label>
            </td>
        </tr>
    </table>
