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
    public partial class Board : System.Web.UI.Page
    {
        public Logic.Board board;
        public bool isOwner;
        public bool isModerator;
        public List<Logic.User> members;
        public List<Logic.User> moderators;
        public List<Channel> channels;
        public int selectedChannel;
        public Dictionary<Message, Logic.User> messages;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["uid"]) || !int.TryParse(Request.QueryString["uid"], out int boardID))
            {
                Response.Redirect("~/YourBoards.aspx");
                return;
            }

            if (Request.Cookies["LoginToken"] == null)
            {
                Response.Redirect("~/LogIn.aspx");
                return;
            }

            // Add postback functionality to message box
            tbMessage.Attributes.Add("OnKeyPress", "if (event.keyCode == 13) {" + ClientScript.GetPostBackEventReference(tbMessage, String.Empty) + "}");

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

            // Get items
            board = user.ActiveBoards.Where(b => b.BoardID == boardID).FirstOrDefault();

            if (board != null)
            {
                // Members
                members = board.GetMembers()[2];
                moderators = board.GetMembers()[1];

                isOwner = board.GetBoardOwner().UserID == user.UserID;
                isModerator = isOwner || moderators.Count(u => u.UserID == user.UserID) != 0;

                // Channels
                channels = board.ViewChannels();

                // Messages
                Channel channel = null;

                if (!string.IsNullOrEmpty(Request.QueryString["chnl"]) && int.TryParse(Request.QueryString["chnl"], out selectedChannel))
                {
                    channel = channels.Where(c => c.ChannelID == selectedChannel).FirstOrDefault();
                }
                channel = channel ?? (channels.Count > 0 ? channels[0] : null);

                messages = new Dictionary<Message, User>();

                if (channel != null)
                {
                    if (IsPostBack && !string.IsNullOrEmpty(tbMessage.Text))
                    {
                        channel.PostMessage(tbMessage.Text);
                        tbMessage.Text = string.Empty;
                    }

                    foreach(Message message in channel.GetMessages())
                    {
                        messages.Add(message, user.GetUserByID(message.UserID));
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

            // Redirect if board is not found for user
            if (board == null)
            {
                Response.Redirect("~/YourBoards.aspx");
            }
        }
    }
}