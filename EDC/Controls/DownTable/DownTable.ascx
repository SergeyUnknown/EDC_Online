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
    border-radius:10px;
    background-color:#7EA2D4;
    box-shadow:0 2px 2px 1px rgba(0, 0, 0, 0.26);
}

td.dtcenter a img{
    background-color:#7ac0da;
    border-radius:5px;
    transition:border-radius 0.1s ease;
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
                <asp:LinkButton runat="server" ID="LinkButtonFirstPage"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/skip-backward (3).png" /></asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonPrevPage"><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/backward.png"/></asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonNextPage"><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/forward (3).png"/></asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonLastPage"><asp:Image ID="Image4" runat="server" ImageUrl="~/Images/skip-forward (2).png"/></asp:LinkButton>
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
