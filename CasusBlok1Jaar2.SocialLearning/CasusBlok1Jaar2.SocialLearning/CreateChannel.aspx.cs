using CasusBlok1Jaar2.SocialLearning.Central;
using CasusBlok1Jaar2.SocialLearning.DAL;
using CasusBlok1Jaar2.SocialLearning.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CasusBlok1Jaar2.SocialLearning
{
    public partial class CreateChannel : System.Web.UI.Page
    {
        public bool invalidChannelName = false;

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

            int boardID = 0;

            if (string.IsNullOrEmpty(Request.QueryString["brd"]) || !int.TryParse(Request.QueryString["brd"], out boardID))
            {
                Response.Redirect("~/YourBoards.aspx");
                return;
            }

            // Get items
            Logic.Board board = user.ActiveBoards.Where(b => b.BoardID == boardID).FirstOrDefault();

            if (board == null)
            {
                Response.Redirect("~/YourBoards.aspx");
                return;
            }

                
            if (IsPostBack && !string.IsNullOrEmpty(tbChannelName.Text))
            {
                Validate();

                if (IsValid)
                {
                    invalidChannelName = !board.CreateChannel(tbChannelName.Text);

                    if (!invalidChannelName)
                    {
                        Channel channel = board.ViewChannels().Where(c => c.Name == tbChannelName.Text).FirstOrDefault();

                        // Update cookie
                        HttpCookie another_cookie = new HttpCookie("LoginToken")
                        {
                            Value = user.Token.Value,
                            Expires = DateTime.Now.AddHours(1)
                        };
                        Response.SetCookie(another_cookie);

                        Response.Redirect($"~/Board.aspx?uid={boardID}&chnl={channel.ChannelID}");
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