<%@ Page Title="Create Board" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="CreateBoard.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.CreateBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageContent" runat="server">
    <link href="styles/createBoardPage.css" rel="stylesheet" />

    <div class="posts">
        <div class="section">
            <div class="postTitle">
                <div class="element" id="bar">
                    <asp:TextBox ID="tbBoardName" runat="server" CssClass='tbSearch' placeholder="Board Name" MaxLength="35" />
                    <asp:RequiredFieldValidator ID="reqValidBoardName" runat="server" ControlToValidate="tbBoardName" ErrorMessage="*" />
                </div>
                <div class="element" id="newAccount">
                    <asp:Button ID="btnCreateBoard" runat="server" Text="Create the Board" CssClass='btnSearch' Style="background-color: #36a420; border-color: #36a420;"/>
                </div>
            </div>
        </div>
    </div>
    <%
        if (invalidBoardName)
        {
            %>
                <script>
                    alert("This board name already exists!");
                </script>
            <%
        }
    %>
</asp:Content>
