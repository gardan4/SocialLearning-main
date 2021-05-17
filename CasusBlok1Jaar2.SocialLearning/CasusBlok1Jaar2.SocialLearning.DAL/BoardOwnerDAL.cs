using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Central;
using Microsoft.Data.SqlClient;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public class BoardOwnerDAL : ModeratorDAL, IBoardOwnerDAL
    {
        #region Constructor
        public BoardOwnerDAL(string connectionString) : base(connectionString)
        {
        }
        #endregion

        #region Functions
        #region Moderators
        public void AddModerator(int boardID, int targetID, LoginToken token)
        {
            if (token == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spBoards_AddModerator @Token OUTPUT, @BoardID, @TargetID;" +
                           "SELECT[Token] = @Token; ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@TargetID", targetID),
                new SqlParameter("@LoginToken", token.Value)
            };

            string lt = null;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();
                lt = reader.GetString(0);
            }, sqlParameters);

            token.Value = lt ?? token.Value;
        }
        public void RemoveModerator(int boardID, int targetID, LoginToken token)
        {
            if (token == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spBoards_RemoveModerator @Token OUTPUT, @BoardID, @TargetID;" +
                           "SELECT[Token] = @Token; ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@TargetID", targetID),
                new SqlParameter("@LoginToken", token.Value)
            };

            string lt = null;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();
                lt = reader.GetString(0);
            }, sqlParameters);

            token.Value = lt ?? token.Value;
        }
        #endregion

        #region Boards
        public void RemoveBoard(int boardID, LoginToken token)
        {
            if (token == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spBoards_RemoveBoard @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token; ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            string lt = null;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();
                lt = reader.GetString(0);
            }, sqlParameters);

            token.Value = lt ?? token.Value;
        }
        #endregion
        #endregion
    }
}
