<%@ Page Title="<%$ Resources:LocalizedText,SubjectMatrix %>" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubjectsMatrix.aspx.cs" Inherits="EDC.Pages.Subject.SubjectsMatrix" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="LegendPlace" runat="server">
    <asp:Table runat="server" ID="tLegend" CssClass="Legend">

        <asp:TableRow>
            <asp:TableHeaderCell RowSpan="2">
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,IconLegend %>" />
                </asp:Label>
            </asp:TableHeaderCell>
            <asp:TableCell>
               <input type="button" Class="ActionIc Unplaned"/> 
               <asp:Label runat="server">
                   <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,NotStarted %>" />
               </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Start"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,DataEntryStarted %>" />
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc End"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Completed %>" />
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc CheckAll"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,SDVComplete %>" />
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Approve"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Signed %>" />
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Stopped"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Stopped %>" />
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <input type="button" Class="ActionIc Lock"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Locked %>" />
                </asp:Label>
            </asp:TableCell>
            <asp:TableCell runat="server" ID="tcLegendDelete">
                <input type="button" Class="ActionIc Delete"/>
                <asp:Label runat="server">
                    <asp:Localize runat="server" Text="<%$ Resources:LocalizedText,Removed %>" />
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            resizeFixTable();
            $(window).scroll(fixMaxtrixHeaders);
            setCssTopScroll();

        });

        function resizeFixTable() {
            $mainTableRows = $('#MainContent_tMatrix tbody tr');
            $fixTableRows = $('.tableFixColumn').find('tbody tr');

            $fixTableRows.eq(0).height($mainTableRows.eq(0).height() + $mainTableRows.eq(1).height());

            rowCount = $fixTableRows.length - 1;

            for (var i = 0; i <= rowCount; i++) {
                $fixTableRows.eq(i + 1).height($mainTableRows.eq(i + 2).height());
            }
        }

        function fixMaxtrixHeaders() {
            var off = $(window).scrollTop();
            if (off > 130) {

                $scrollPos = $("#topScroll").scrollLeft();
                $("#topScroll").css("position", "fixed");
                $("#topScroll").css("width", $(".scrollDiv").width());
                $("#topScroll").scrollLeft($scrollPos);
                $(".tableMatrixFixHeaders").css("top", $(window).scrollTop() - 126 + 19 + 19);
            }
            else {
                $(".tableMatrixFixHeaders").css("top", 0);
                $("#topScroll").css("position", "relative");
                $("#topScroll").css("width", "auto");
            }
        }

        function setCssTopScroll() {
            $("#topScroll").css("top", 0);
            $("#topScroll").css("overflow-x", "scroll");
            $("#topScroll").css("position", "relative");
            $("#topScroll").css("z-index", "6");
            $("#topScrollContent").css("height", "1px");
            $("#topScrollContent").css("width", $(".scrollDiv")[0].scrollWidth);

            $("#topScroll").on("scroll", function () {
                $(".scrollDiv").scrollLeft($("#topScroll").scrollLeft());
            });

            $(".scrollDiv").on("scroll", function () {
                $("#topScroll").scrollLeft($(".scrollDiv").scrollLeft());
            });
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="topScroll">
                <div id="topScrollContent">
                </div>
            </div>
            <asp:Table runat="server" ID="tMatrixFixColumns" CssClass="tableFixColumn"></asp:Table>
            <div class="scrollDiv">
                <asp:Table runat="server" ID="tMatrix"></asp:Table>
            </div>
            <DownTableControl:DownTable ID="dtInfo" runat="server" ViewButton="false" OnSelectedIndexChanged="dtInfo_SelectedIndexChanged" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
