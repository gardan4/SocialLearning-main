<%@ Page Title="Social Learning - Register" Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.Register" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/loginPage.css" rel="stylesheet" />
    <title>Social Learning - Register</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="loginForm"  style="margin: 10% 0% 5% 30%;">
            <div class="logo" style="margin: 15% 0% 0% 0;">
                <img src="https://yacenter.org/wp-content/uploads/2016/01/logo_placeholder-300x167.png"/>
            </div>

            <div class="section">
                <div class="element">
                    <h1 style="font-weight:700; margin: 2px;">Sign up!</h1>
                    <p style="margin: 2px; font-size:12px">It's quick and easy</p>
                </div>
                <div class="element">
                    <asp:TextBox ID="tbEmail" runat="server" CssClass='tbSearch' placeholder="Email Address" MaxLength="75" />
                    <asp:RequiredFieldValidator ID="reqValidEmail" ControlToValidate="tbEmail" runat="server" ErrorMessage="*" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="regExValidEmail" ControlToValidate="tbEmail" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="*" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="regExLengthValidEmail" ControlToValidate="tbEmail" runat="server" ValidationExpression="^[a-zA-Z0-9\S\s]{1,75}" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:TextBox ID="tbUsername" runat="server" CssClass='tbSearch' placeholder="Username" MaxLength="35" />
                    <asp:RequiredFieldValidator ID="reqValidUsername" ControlToValidate="tbUsername" runat="server" ErrorMessage="*" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="regExLengthValidUsername" ControlToValidate="tbName" runat="server" ValidationExpression="^[a-zA-Z0-9\S\s]{1,35}" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:TextBox ID="tbName" runat="server" CssClass='tbSearch' placeholder="Full Name" MaxLength="75" />
                    <asp:RequiredFieldValidator ID="reqValidName" ControlToValidate="tbName" runat="server" ErrorMessage="*" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="regExLengthValidName" ControlToValidate="tbName" runat="server" ValidationExpression="^[a-zA-Z0-9\S\s]{1,75}" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:TextBox ID="tbPassword" runat="server" CssClass='tbSearch' TextMode="Password" placeholder="Password" MaxLength="128" />
                    <asp:RequiredFieldValidator ID="reqValidPassword" ControlToValidate="tbPassword" runat="server" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:TextBox ID="tbBirthday" runat="server" CssClass='tbSearch' TextMode="Date" placeholder="Date of Birth (dd/mm/yyyy)" />
                    <asp:RequiredFieldValidator ID="reqValidBirthday" ControlToValidate="tbBirthday" runat="server" ErrorMessage="*" Display="Dynamic" />
                    <asp:RangeValidator ID="rValidBirthday" ControlToValidate="tbBirthday" runat="server" MinimumValue="01/01/1950" MaximumValue="01/01/2020" Type="Date" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:TextBox ID="tbSchoolYear" runat="server" CssClass='tbSearch' TextMode="Number" Min="1" Max="4" Step="1" placeholder="School Year" />
                    <asp:RequiredFieldValidator ID="reqValidSchoolYear" ControlToValidate="tbSchoolYear" runat="server" ErrorMessage="*" Display="Dynamic" />
                    <asp:RangeValidator ID="rValidSchoolYear" ControlToValidate="tbSchoolYear" runat="server" MinimumValue="1" MaximumValue="4" Type="Integer" ErrorMessage="*" Display="Dynamic" />
                </div>
                <div class="element">
                    <asp:Button ID="btnSubmit" runat="server" Text="Sign up" CssClass='btnSearch' PostBackUrl="~/Register.aspx" />
                </div>
                <div class="element" id="newAccount">
                    <asp:Button ID="btnBack" runat="server" Text="Back to Login" CssClass='btnSearch' Style="background-color: #36a420; border-color: #36a420;" OnClick="btnBack_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </form>
    <%
        if (invalidRegistration)
        {
            %>
                <script>
                    alert("The username or email you entered was already in use!");
                </script>
            <%
        };
    %>
</body>
</html>

