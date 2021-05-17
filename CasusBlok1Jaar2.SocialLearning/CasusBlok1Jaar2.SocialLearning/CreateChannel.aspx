<%@ Page Title="Create Channel" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="CreateChannel.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.CreateChannel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageContent" runat="server">
    <link href="styles/createBoardPage.css" rel="stylesheet" />

    <div class="posts">
        <div class="section">
            <div class="postTitle">
                <div class="element" id="bar">
                    <asp:TextBox ID="tbChannelName" runat="server" CssClass='tbSearch' placeholder="Channel Name" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValidChannelName" runat="server" ControlToValidate="tbChannelName" ErrorMessage="*" />
                </div>
                <div class="element" id="newAccount">
                    <asp:Button ID="btnCreateBoard" runat="server" Text="Create the Channel" CssClass='btnSearch' Style="background-color: #36a420; border-color: #36a420;" />
                </div>
            </div>
        </div>
    </div>
    <%
        if (invalidChannelName)
        {
            %>
                <script>
                    alert("This channel name already exists!");
                </script>
            <%
        }
    %>
</asp:Content>
