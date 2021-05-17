using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CasusBlok1Jaar2.SocialLearning
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SignOff(object sender, EventArgs e)
        {
            HttpCookie antiCookie = new HttpCookie("LoginToken")
            {
                Expires = DateTime.Now.AddHours(-1)
            };
            Response.SetCookie(antiCookie);
            Response.Redirect("~/LogIn.aspx");
        }
    }
}