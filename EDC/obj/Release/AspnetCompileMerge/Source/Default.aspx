<%@ Page Title="<%$ Resources:LocalizedText,HomePage %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EDC._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><%: Title %></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
                <div style="text-align:center; margin:0 auto; max-width:50%">
                    <h1 style="color:#3590c9;"><%: Protocol %></h1>
                    <h1 style="color:#3590c9;"><%: StudyName %></h1>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
