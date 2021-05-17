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
    public partial class AllBoard : System.Web.UI.Page
    {
        public Dictionary<Logic.Board, bool[]> boardOwnershipDict;

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
            List<Logic.Board> boards = IsPostBack && !string.IsNullOrEmpty(tbSearch.Text) ? user.SearchBoards(tbSearch.Text) : user.ViewBoards();

            // !TEMPORARY IMPLEMENTATION!
            // Join every board
            foreach (Logic.Board board in boards)
            {
                user.JoinBoard(board.BoardID);
            }
            // !END TEMPORARY IMPLEMENTATION!

            boardOwnershipDict = new Dictionary<Logic.Board, bool[]>();
            foreach (Logic.Board board in boards)
            {
                boardOwnershipDict.Add(board, new bool[] { board.GetMembers()?[2].Count(member => member.UserID == user.UserID) > 0, board.GetBoardOwner()?.UserID == user.UserID });
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