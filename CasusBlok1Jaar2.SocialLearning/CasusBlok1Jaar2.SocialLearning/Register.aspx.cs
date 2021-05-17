using CasusBlok1Jaar2.SocialLearning.DAL;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CasusBlok1Jaar2.SocialLearning
{
    public partial class Register : System.Web.UI.Page
    {
        public bool invalidRegistration = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            rValidBirthday.MaximumValue = DateTime.Now.Date.ToString("dd/MM/yy");

            if (IsPostBack)
            {
                Validate();

                if (IsValid)
                {
                    Logic.User user = Logic.User.Register(new UserDAL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString), tbEmail.Text, tbUsername.Text, tbPassword.Text);

                    if (user != null)
                    {
                        Logic.Profile profile = user.GetProfile();

                        DateTime bday = DateTime.Parse(tbBirthday.Text);
                        int age = (DateTime.Now - bday).Days / 365;

                        profile.UpdateProfile(tbPassword.Text, tbEmail.Text, tbUsername.Text, tbName.Text, age, int.Parse(tbSchoolYear.Text));

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
                        invalidRegistration = true;
                    }
                }
            }

            if (Request.Cookies["LoginToken"] != null)
            {
                Response.Redirect("~/Home.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("LogIn.aspx");
        }
    }
}