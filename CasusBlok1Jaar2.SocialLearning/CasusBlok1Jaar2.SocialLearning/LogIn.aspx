<%@ Page Title="Social Learning - Login" Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/loginPage.css" rel="stylesheet" />
    <title>Social Learning - Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="loginForm">
            <div class="logo">
                <img src="images/logo_placeholder-300x167.jpg" />
            </div>

            <div class="section">
                <div class="element">
                    <asp:TextBox ID="tbEmail" runat="server" CssClass='tbSearch' placeholder="Email Address" MaxLength="75" />
                    <asp:RequiredFieldValidator ID="reqValidEmail" ControlToValidate="tbEmail" runat="server" ErrorMessage="*" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="regExValidEmail" ControlToValidate="tbEmail" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="*" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="regExLengthValidEmail" ControlToValidate="tbEmail" runat="server" ValidationExpression="^[a-zA-Z0-9\S\s]{1,75}" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:TextBox ID="tbPassword" runat="server" CssClass='tbSearch' TextMode="Password" placeholder="Password" MaxLength="128" />
                    <asp:RequiredFieldValidator ID="reqValidPassword" ControlToValidate="tbPassword" runat="server" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:Button ID="btnSubmit" runat="server" Text="Log In" CssClass='btnSearch' />
                    <!--<a href="test" style="font-size: 12px;">password recovery</a>-->
                </div>
                <div class="element" id="newAccount">
                    <asp:Button ID="btnNewAccount" runat="server" Text="Create an account" CssClass='btnSearch' Style="background-color: #36a420; border-color: #36a420;" OnClick="btnNewAccount_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </form>
    <%
        if (invalidLogin)
        {
            %>
                <script>
                    alert("The email and password combination did not match!");
                </script>
            <%
        };
    %>
</body>
</html>
