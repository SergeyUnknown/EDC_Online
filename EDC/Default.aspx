<%@ Page Title="Главная страница" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EDC._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlModal" style="display:none">
                <label>aaaaa</label>
                <asp:Button runat="server" OnClick="Unnamed_Click" Text="Close" />
            </asp:Panel>

            <ajaxToolkit:ModalPopupExtender runat="server" ID="modalPopup" PopupControlID="pnlModal" TargetControlID="btnShow" DropShadow="true" >
                
            </ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" Text="ShowModalPopup"/>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
