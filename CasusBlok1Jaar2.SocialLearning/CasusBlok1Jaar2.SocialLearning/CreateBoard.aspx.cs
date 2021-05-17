using CasusBlok1Jaar2.SocialLearning.Central;
using CasusBlok1Jaar2.SocialLearning.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CasusBlok1Jaar2.SocialLearning
{
    public partial class CreateBoard : System.Web.UI.Page
    {
        public bool invalidBoardName = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["LoginToken"] == null)
            {
                Response.Redirect("~/LogIn.aspx");
                return;
            }

            // Get user
            LoginToken loginToken = new LoginToken(Request.Cookies["LoginToken"].Value);
            Logic.User user = Logic.User.GetSession(new UserDAL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString), loginToken);

            if (user == null)
            {
                HttpCookie antiCookie = new HttpCookie("LoginToken")
                {
                    Expires = DateTime.Now.AddHours(-1)
                };
                Response.SetCookie(antiCookie);
                Response.Redirect("~/LogIn.aspx");
                return;
            }

            if (IsPostBack && !string.IsNullOrEmpty(tbBoardName.Text))
            {
                Validate();

                if (IsValid)
                {
                    invalidBoardName = !user.MakeBoard(tbBoardName.Text);

                    if (!invalidBoardName)
                    {
                        Logic.Board board = user.ActiveBoards.Where(b => b.Name == tbBoardName.Text).FirstOrDefault();

                        board.CreateChannel("Default");

                        // Update cookie
                        HttpCookie another_cookie = new HttpCookie("LoginToken")
                        {
                            Value = user.Token.Value,
                            Expires = DateTime.Now.AddHours(1)
                        };
                        Response.SetCookie(another_cookie);

                        Response.Redirect($"~/YourBoards.aspx");
                    }
                }
            }

            // Update cookie
            HttpCookie cookie = new HttpCookie("LoginToken")
            {
                Value = user.Token.Value,
                Expires = DateTime.Now.AddHours(1)
            };
            Response.SetCookie(cookie);
        }
    }
}