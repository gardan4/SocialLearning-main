<%@ Page Title="Your Boards" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="YourBoards.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.YourBoards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageContent" runat="server">
    <link href="styles/allBoardsPage.css" rel="stylesheet" />
    <link href="styles/topSearchBar.css" rel="stylesheet" />

    <%-- Search bar enzo --%>
    <div class="searchBar">
        <div class="pageName">Your Boards</div>
        <div class="searchElements">
            <asp:TextBox ID="tbSearch" runat="server" CssClass='tbSearch' placeholder="Search..." AutoPostBack="true" />
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass='btnSearch' />
        </div>
    </div>

    <%-- div voor alle borden --%>
    <div class="allBoards">
        <%
            if (boardOwnershipDict.Count == 0)
            {
                if (string.IsNullOrEmpty(tbSearch.Text))
                {
                    %>
                        You currently have no boards yet.
                    <%
                }
                else
                {
                    %>
                        There are no boards matching the search input "<%=tbSearch.Text %>".
                    <%
                }
            }
        %>

        <%-- Hier begint een board --%>
        <%
            else
            {
                foreach (var board in boardOwnershipDict)
                {
                    %>
                        <div class="section">
                            <div class="postTitle">
                                <a href="Board.aspx?uid=<%=board.Key.BoardID %>"><%=board.Key.Name %></a>
                                <%
                                    if (!board.Value)
                                    {
                                        %>
                                            <!-- Only be able to leave if you're not the owner -->
                                            <asp:Button runat="server" Text="Unsubscribe" CssClass='btnLeave' />
                                        <%
                                    }
                                %>
                            </div>
                            <div class="postContent">
                                Made by: <%= board.Key.GetBoardOwner().Username %><br />
                                Date Created: <%= board.Key.DateCreated.ToString("dd/MM/yyyy") %>
                            </div>
                        </div>
                    <%
                }
            }
        %>
    </div>
</asp:Content>
