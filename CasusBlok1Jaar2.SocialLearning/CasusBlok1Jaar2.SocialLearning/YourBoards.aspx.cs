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
    public partial class YourBoards : System.Web.UI.Page
    {
        public Dictionary<Logic.Board, bool> boardOwnershipDict;

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

            // Get items
            List<Logic.Board> boardFilter = new List<Logic.Board>();
            List<Logic.Board> boards = user.ActiveBoards;

            if (IsPostBack && !string.IsNullOrEmpty(tbSearch.Text))
            {
                boardFilter = user.SearchBoards(tbSearch.Text);
                boards.RemoveAll(board => boardFilter.Count(b => b.BoardID == board.BoardID) != 1);
            }

            boardOwnershipDict = new Dictionary<Logic.Board, bool>();
            foreach(Logic.Board board in boards)
            {
                board.GetMembers();
                boardOwnershipDict.Add(board, board.GetBoardOwner().UserID == user.UserID);
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