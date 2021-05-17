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
    public partial class LogIn : System.Web.UI.Page
    {
        public bool invalidLogin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Validate();

                if (IsValid)
                {
                    Logic.User user = Logic.User.Login(new UserDAL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString), tbEmail.Text, tbPassword.Text);

                    if (user != null)
                    {
                        HttpCookie cookie = new HttpCookie("LoginToken")
                        {
                            Value = user.Token.Value,
                            Expires = DateTime.Now.AddHours(1)
                        };
                        Response.SetCookie(cookie);

                        Response.Redirect("~/Home.aspx");
                    }
                    else
                    {
                        invalidLogin = true;
                    }
                }
            }

            if (Request.Cookies["LoginToken"] != null)
            {
                Response.Redirect("~/Home.aspx");
            }
        }

        protected void btnNewAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}