<%@ Page Title="Board Overview" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="AllBoards.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.AllBoard" %>

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
        <%-- Hier begint een board --%>
        <div class="section">
            <div id="newBoard">
                <a href="CreateBoard.aspx">Make a new board</a>
            </div>
        </div>

        <%
            foreach (var board in boardOwnershipDict)
            {
                %>
                    <div class="section">
                        <div class="postTitle">
                            <a <%= board.Value[1] || board.Value[0] ? $"href=\"Board.aspx?uid={board.Key.BoardID}\"" : "" %>><%=board.Key.Name %></a>
                            <%
                                // Buttons don't work. Need asp:repeater for which usage of proper models would be required.
                                if (!board.Value[1])
                                {
                                    if (board.Value[0])
                                    {
                                        %>
                                            <!-- Only be able to leave if you're not the owner, but are member -->
                                            <asp:Button ID="btnLeave" runat="server" Text="Unsubscribe" CssClass='btnLeave' PostBackUrl="~/AllBoards.aspx" />
                                        <%
                                    }
                                    else
                                    {
                                        %>
                                            <!-- Only be able to leave if you're not the owner and not a member -->
                                            <!-- href="AllBoards.aspx?lv_brd=5 -->
                                            <asp:Button ID="btnJoin" runat="server" Text="Subscribe" CssClass='btnLeave' PostBackUrl="~/AllBoards.aspx" />
                                        <%
                                    }
                                }
                            %>
                        </div>
                        <div class="postContent">
                            <!-- Technical limitation, add in later implementation -->
                            <% //Made by: <%= board.Key.GetBoardOwner().Username % ><br /> %>
                            Date Created: <%= board.Key.DateCreated.ToString("dd/MM/yyyy") %>
                        </div>
                    </div>
                <%
            }
        %>
    </div>
</asp:Content>
