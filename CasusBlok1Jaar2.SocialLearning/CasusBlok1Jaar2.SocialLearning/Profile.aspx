<%@ Page Title="Profile" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CasusBlok1Jaar2.SocialLearning.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageContent" runat="server">
     <link href="styles/profilePage.css" rel="stylesheet" />

    <div class="banner">
        <img src="https://marshalledc.org/sites/default/files/banner/mountains.jpg" />
        <asp:Button ID="btnSearch" runat="server" Text="Edit Profile" CssClass='btnEdit' />
    </div>

    <div class="profile">
        <div class="username">
            Username
        </div>
        <div class="information">
            <h3>social1</h3>
            <h3>social2</h3>
            <h3>social3</h3>
            <h3>social4</h3>
        </div>
    </div>
</asp:Content>
