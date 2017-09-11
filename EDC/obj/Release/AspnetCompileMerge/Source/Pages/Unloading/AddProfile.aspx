<%@ Page Title="<%$ Resources:LocalizedText,CreateDataset %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddProfile.aspx.cs" Inherits="EDC.Pages.Unloading.AddProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblStatus" CssClass="lblStatus lblStatusError" Text="" Visible="false"/>
            <asp:Literal runat="server" ID="lStatus"/>
            <div id="divConfig" runat="server" style="overflow:auto">
                <ol>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbName" Text="<%$ Resources:LocalizedText,NName %>" />
                        <asp:TextBox runat="server" ID="tbName" CssClass="unloadingFieldsWidth" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName"
                            CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="tbDescription" Text="<%$ Resources:LocalizedText,Description %>" />
                        <asp:TextBox runat="server" ID="tbDescription" CssClass="unloadingFieldsWidth" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDescription"
                            CssClass="field-validation-error" ErrorMessage="<%$ Resources:LocalizedText,RequiredField %>" />
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="ddlCrfStatus" Text="<%$ Resources:LocalizedText,Status %>" />
                        <asp:DropDownList runat="server" ID="ddlCrfStatus" CssClass="unloadingFieldsWidth">
                            <asp:ListItem Text="<%$ Resources:LocalizedText,All %>" Value="All" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="SDV" Value="IsCheck"></asp:ListItem>
                            <asp:ListItem Text="Завершенные" Value="IsEnd"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>
                        <asp:Label runat="server" AssociatedControlID="cblCenters" Text="<%$ Resources:LocalizedText,Site %>" />
                        <asp:CheckBoxList runat="server" ID="cblCenters" CssClass="unloadingFieldsWidth unloadingCBL"/>
                    </li>
                </ol>
                <asp:Button runat="server" ID="btnConfigOk" OnClick="btnConfigOk_Click" Text="<%$ Resources:LocalizedText,Next %>" />
                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText,Cancel %>" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

            <div id="divSelectItems" runat="server" visible="false" style="overflow:auto">
                <div id="divEvents" style="float: left; width: 32%; margin-left: 10px;">
                    <asp:Table runat="server" ID="tEvents" CssClass="tUnloading">
                        <asp:TableFooterRow>
                            <asp:TableHeaderCell><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Event %>" /></asp:TableHeaderCell>
                        </asp:TableFooterRow>
                    </asp:Table>
                    <asp:Button runat="server" Text="<%$ Resources:LocalizedText,SelectAll %>" ID="btnSelectAllEvents" OnClick="btnSelectAllEvents_Click" Visible="false" />
                    <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Deselect %>" ID="btnUnselectAllEvents" OnClick="btnUnselectAllEvents_Click" Visible="false" />
                </div>

                <div id="divCRFs" style="float: left; width: 32%; margin-left: 10px;">
                    <asp:Table runat="server" ID="tCRFs" CssClass="tUnloading">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,CRF %>" /></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    <asp:Button runat="server" Text="<%$ Resources:LocalizedText,SelectAll %>" ID="btnSelectAllCRFs" OnClick="btnSelectAllCRFs_Click" Visible="false" />
                    <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Deselect %>" ID="btnUnselectAllCRFs" OnClick="btnUnselectAllCRFs_Click" Visible="false" />
                </div>
                <div id="divCrfItems" style="float: left; width: 33%; margin-left: 10px;">
                    <asp:Table runat="server" ID="tItems" CssClass="tUnloading">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell><asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Field %>" /></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    <asp:Button runat="server" Text="<%$ Resources:LocalizedText,SelectAll %>" ID="btnSelectAllItems" OnClick="btnSelectAllItems_Click" Visible="false" />
                    <asp:Button runat="server" Text="<%$ Resources:LocalizedText,Deselect %>" ID="btnUnselectAllItems" OnClick="btnUnselectAllItems_Click" Visible="false" />
                </div>
            </div>
            <asp:Button runat="server" ID="btnProfileOk" OnClick="btnProfileOk_Click" Text="<%$ Resources:LocalizedText,Save %>" Visible="false" />
            <asp:Button ID="btnCancel2" runat="server" Text="<%$ Resources:LocalizedText,Cancel %>" CausesValidation="false" OnClick="btnCancel_Click" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
