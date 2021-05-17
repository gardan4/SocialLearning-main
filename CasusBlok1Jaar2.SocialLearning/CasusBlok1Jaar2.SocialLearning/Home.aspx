<%@ Page Title="Home" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageContent" runat="server">
    <link href="styles/homePage.css" rel="stylesheet" />
    <link href="styles/topSearchBar.css" rel="stylesheet" />

    <div class="searchBar">
        <div class="pageName">Home</div>
        <div class="searchElements">
            <!--
            <asp:TextBox ID="tbSearch" runat="server" CssClass='tbSearch'>Search</asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Button" CssClass='btnSearch' />-->
            <br />
        </div>
    </div>

    <%-- Deze moeten dan gegenereerd kunnen worden --%>
    <div class="posts">
        <%
            foreach (var board in boards)
            {
                %>
                    <div class="section">
                        <div class="postTitle">
                            <%=board.Name %>
                        </div>
                        <div class="postContent">
                            <%
                                if (board.ViewChannels().Count > 0 && board.ViewChannels()[0].GetMessages().Count > 0)
                                {
                                    %>
                                        Last message in channel "<%=board.ViewChannels()[0].Name %>":<br />
                                        <%=board.ViewChannels()[0].GetMessages()[0].SentAt.ToShortDateString() + " " + board.ViewChannels()[0].GetMessages()[0].SentAt.ToShortTimeString() %> -
                                        <%=board.ViewChannels()[0].GetMessages()[0].Body %>
                                    <%
                                }
                                else
                                {
                                    %>
                                        The channel has seen no activity yet.
                                    <%
                                }
                            %>
                        </div>
                    </div>
                <%
            }
        %>
    </div>
</asp:Content>
