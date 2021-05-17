<%@ Page Title="Board" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Board.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.Board" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageContent" runat="server">
    <link href="styles/boardPage.css" rel="stylesheet" />
    <link href="styles/topSearchBar.css" rel="stylesheet" />


    <div class="searchBar">
        <div class="pageName">Board Name</div>
        <!-- There is no search option. Maybe in a later implementation -->
        <div class="searchElements">
            <!--
            <asp:TextBox ID="tbSearch" runat="server" CssClass='tbSearch'>Search</asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Button" CssClass='btnSearch' />
            -->
            <br />
        </div>
    </div>

    <div class="boardView">
        <%-- List of channels --%>
        <div class="leftNav">
            <%-- Channelname clickable --%>
            <%
                foreach(var channel in channels)
                {
                    %>
                        <div class="<%=channel.ChannelID == selectedChannel ? "selectedChannel" : "channel" %>" onclick="location.href='Board.aspx?uid=<%=board.BoardID %>&chnl=<%=channel.ChannelID %>';"><%=channel.Name %></div>
                    <%
                }
            %>
            <%=isModerator ? $"<div class=\"channel\"><a href=\"CreateChannel.aspx?brd={board.BoardID}\">New Channel</a></div>" : "" %>
        </div>

        <%-- Center part with posts --%>
        <div class="Main" style="display: flex; flex-direction: column; width: 100%;">
            <div class="Main">
                <%-- Post --%>
                
                <%
                    foreach(var message in messages)
                    {
                        %>
                            <div class="post">
                                <div class="postContent"><b><%=message.Value.Username %></b> - <%=message.Key.SentAt.ToShortDateString() + " " + message.Key.SentAt.ToShortTimeString() %></div>
                                <div class="postTitle">
                                    <%=message.Key.Body %>
                                    <!-- Need asp:repeater for working implementation
                                    <div class="dropdown">
                                        <img src="images\Naamloos-1.png" />
                                        <div class="dropdown-content">
                                            <p>Remove Message</p>
                                            <p>Edit Message</p>
                                        </div>
                                    </div>
                                    -->
                                </div>
                            </div>
                        <%
                    }
                %>
            </div>              
            <div class="chatBox">
                <asp:TextBox ID="tbMessage" runat="server" CssClass='message' placeholder="Message..." MaxLength="250" />
            </div>
        </div>

        <div class="rightNav">
            <div class="user"><%=board.GetBoardOwner().Username %> [Owner]</div>
            <%
                foreach(var user in moderators)
                {
                    %>
                        <div class="user"><%=user.Username %> [Mod]</div>
                    <%
                }
            %>
            <%
                foreach(var user in members)
                {
                    %>
                        <div class="user" <%=isModerator ? "onclick=\"btnClick()\"" : "" %> ><%=user.Username %></div>
                    <%
                }
            %>
        </div>
    </div>

    <div id="userModal" class="modal">
        <!-- Modal content -->
        <div class="modalContentUser">
            <span class="close">&times;</span>
            <h4>Username</h4>
            <div class="btns">
                <asp:Button ID="Button1" runat="server" Text="Warm User" CssClass='btnUser' />
                <asp:Button ID="Button2" runat="server" Text="Kick User" CssClass='btnUser' />
                <asp:Button ID="Button3" runat="server" Text="Ban User" CssClass='btnUser' />
            </div>
        </div>
    </div>

    <div id="channelModal" class="modal">
        <!-- Modal content -->
        <div class="modalContentUser">
            <span class="close">&times;</span>
            <h4>ChannelName</h4>
            <div class="btns">
                <asp:Button ID="Button4" runat="server" Text="Delete Channel" CssClass='btnChannel' />
                <asp:Button ID="Button5" runat="server" Text="Change name" CssClass='btnChannel' />
            </div>
        </div>
    </div>

    <script src="scripts/modal.js"></script>
</asp:Content>
