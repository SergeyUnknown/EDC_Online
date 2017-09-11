﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownTable.ascx.cs" Inherits="EDC.Controls.DownTable" %>

<style>
    table.dt {
        width: 100%;
        margin-top: 0.75em;
    }

    img {
        margin-top: 4px;
        width: 24px;
        height: 24px;
    }

    td.dtleft {
        width: 20%;
    }
    td.dtleft a img{
        margin-top:4px;
    }

    td.dtcenter {
        width: 40%;
        text-align: center;
    }

        td.dtcenter a img:hover {
            border-radius: 20px;
            box-shadow: 0 2px 2px 1px rgba(0, 0, 0, 0.26);
        }

        td.dtcenter a img {
            border-radius: 20px;
            border: 1px solid #e2e2e2;
            padding: 0px;
            transition: border-radius 0.1s ease;
            margin-top: 4px;
            margin-left: 1px;
            margin-right: 1px;
        }

    td.dtright1 {
        width: 25%;
        text-align: right;
        padding-right: 0px;
    }

    td.dtright1 select {
        padding: 1px 1px 0px 1px;
    }

    td.dtrigth2 {
        width: 15%;
        text-align: right;
    }
    /**/
</style>

<table class="dt">
    <tr>
        <td class="dtleft">
            <asp:LinkButton runat="server" ID="LinkButtonAddNew" CssClass="addButton">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/table-row-plus-after.png" /></asp:LinkButton>
        </td>
        <td class="dtcenter">
            <asp:LinkButton runat="server" ID="LinkButtonFirstPage">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/step-backward-2.png" /></asp:LinkButton>
            <asp:LinkButton runat="server" ID="LinkButtonPrevPage">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/step-backward.png" /></asp:LinkButton>
            <asp:LinkButton runat="server" ID="LinkButtonNextPage">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/step-forward.png" /></asp:LinkButton>
            <asp:LinkButton runat="server" ID="LinkButtonLastPage">
                <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/step-forward-2.png" /></asp:LinkButton>
        </td>
        <td class="dtright1">
            <asp:DropDownList CssClass="pageCount" ID="ddlItemCount" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCount_SelectedIndexChanged" />
            <asp:Image runat="server" ImageUrl="~/Images/records-count.png" />
        </td>
        <td class="dtrigth2">
            <asp:Label runat="server" ID="pageInfo"></asp:Label>
        </td>
    </tr>
</table>