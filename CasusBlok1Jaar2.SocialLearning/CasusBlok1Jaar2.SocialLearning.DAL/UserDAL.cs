using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public class UserDAL : DatabaseDAL, IUserDAL
    {
        #region Fields
        private ProfileDAL _profileDAL;
        private BoardDAL _boardDAL;
        #endregion

        #region Constructors
        public UserDAL(string connectionString) : base(connectionString)
        {
        }
        #endregion

        #region Functions
        /// <summary>
        /// !TEMPORARY IMPLEMENTATION!
        /// </summary>
        public UserEntity GetUser(int userID)
        {
            string query = "SELECT * FROM dbo.Users USR WHERE USR.UserID = @UID;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@UID", userID)
            };

            string email = null;
            string username = null;
            string name = null;
            int age = -1;
            int schoolYear = -1;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();
                email = reader.GetString(2);
                username = reader.GetString(3);
                name = reader.IsDBNull(5) ? null : reader.GetString(5);
                age = reader.IsDBNull(6) ? -1 : reader.GetInt32(6);
                schoolYear = reader.IsDBNull(7) ? -1 : reader.GetInt32(6);
            }, sqlParameters);
            
            return userID == -1 ? null : new UserEntity(userID, null, email, username, name, age, schoolYear);
        }

        #region Account
        public UserEntity Register(string email, string username, string password, LoginToken token)
        {
            password = GetHashString(password);

            string query = "DECLARE @Token nvarchar(256);" +
                           "EXEC dbo.spUsers_Register @Email, @Uname, @Pass, @Token OUTPUT;" +
                           "SELECT [Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Uname", username),
                new SqlParameter("@Pass", password)
            };

            int userID = -1;
            string lt = null;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "UserID")
                {
                    reader.Read();
                    userID = reader.GetInt32(0);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return userID == -1 ? null : new UserEntity(userID, token, email, username, null, -1, -1);
        }
        public UserEntity Login(string email, string password, LoginToken token)
        {
            password = GetHashString(password);

            string query = "DECLARE @Token nvarchar(256);" +
                           "EXEC dbo.spUsers_Login @Email, @Pass, @Token OUTPUT;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Pass", password)
            };

            int userID = -1;
            string username = null;
            string lt = null;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "UserID")
                {
                    reader.Read();
                    userID = reader.GetInt32(0);
                    username = reader.GetString(2);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return userID == -1 ? null : new UserEntity(userID, token, email, username, null, -1, -1);
        }
        public UserEntity GetSession(LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC dbo.spUsers_GetSession @Token OUTPUT;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@LoginToken", token.Value)
            };

            int userID = -1;
            string username = null;
            string email = null;
            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "UserID")
                {
                    reader.Read();
                    userID = reader.GetInt32(0);
                    email = reader.GetString(1);
                    username = reader.GetString(2);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return userID == -1 ? null : new UserEntity(userID, token, email, username, null, -1, -1);
        }
        public void ChangePassword(string oldPass, string newPass, LoginToken token)
        {
            if (token == null)
                return;

            oldPass = GetHashString(oldPass);
            newPass = GetHashString(newPass);

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC dbo.spUsers_UpdatePassword @Token OUTPUT, @OldPass, @NewPass;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@LoginToken", token.Value),
                new SqlParameter("@OldPass", oldPass),
                new SqlParameter("@NewPass", newPass)
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

        #region Contacts
        public void SearchUsers()
        {
            throw new NotImplementedException();
        }
        public void InviteUserContact()
        {
            throw new NotImplementedException();
        }
        public void ViewContactInvites()
        {
            throw new NotImplementedException();
        }
        public void AcceptContact()
        {
            throw new NotImplementedException();
        }
        public void DenyContact()
        {
            throw new NotImplementedException();
        }
        public void ViewContacts()
        {
            throw new NotImplementedException();
        }
        public void DeleteContact()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Boards
        public BoardEntity CreateBoard(string name, LoginToken token, int parentBoard = -1)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           $"EXEC dbo.spBoards_CreateBoard @Token OUTPUT, @BoardName{(parentBoard == -1 ? "" : ", @ParentID")};" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@BoardName", name),
                new SqlParameter("@LoginToken", token.Value)
            };

            if (parentBoard != -1)
                sqlParameters.Add(new SqlParameter("@ParentID", parentBoard));

            int boardID = -1;
            int ownerID = -1;
            DateTime createdAt = DateTime.Now;

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "BoardID")
                {
                    reader.Read();
                    boardID = reader.GetInt32(0);
                    ownerID = reader.GetInt32(2);
                    createdAt = reader.GetDateTime(4);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters.ToArray());

            token.Value = lt;
            return boardID == -1 ? null : new BoardEntity(boardID, parentBoard, ownerID, name, createdAt);
        }
        public IEnumerable<BoardEntity> ViewBoards()
        {
            string query = "EXEC dbo.spBoards_SearchBoards;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                
            };

            List<BoardEntity> results = new List<BoardEntity>();

            ExecuteQuery(query, (reader) =>
            {
                while(reader.Read())
                {
                    results.Add(
                        new BoardEntity(
                            reader.GetInt32(0),
                            reader.IsDBNull(1) ? -1 : reader.GetInt32(1),
                            reader.GetInt32(4),
                            reader.GetString(2),
                            reader.GetDateTime(3)
                        )
                    );
                }
            }, sqlParameters);

            return results;
        }
        public IEnumerable<BoardEntity> SearchBoards(string searchQuery)
        {
            string query = "EXEC dbo.spBoards_SearchBoards @SearchQuery;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@SearchQuery", searchQuery ?? string.Empty)
            };

            List<BoardEntity> results = new List<BoardEntity>();

            ExecuteQuery(query, (reader) =>
            {
                while (reader.Read())
                {
                    results.Add(
                        new BoardEntity(
                            reader.GetInt32(0),
                            reader.IsDBNull(1) ? -1 : reader.GetInt32(1),
                            reader.GetInt32(4),
                            reader.GetString(2),
                            reader.GetDateTime(3)
                        )
                    );
                }
            }, sqlParameters);

            return results;
        }
        public void JoinBoard(int boardID, LoginToken token)
        {
            if (token.Value == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC dbo.spBoards_JoinBoard @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();

                lt = reader.GetString(0);
            }, sqlParameters.ToArray());

            token.Value = lt;
        }
        public void LeaveBoard(int boardID, LoginToken token)
        {
            if (token.Value == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC dbo.spBoards_LeaveBoard @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();

                lt = reader.GetString(0);
            }, sqlParameters.ToArray());

            token.Value = lt;
        }
        public IEnumerable<BoardEntity> GetActiveBoards(LoginToken token)
        {
            if (token == null)
                return new List<BoardEntity>();

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC dbo.spBoards_ViewActiveBoards @Token OUTPUT;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@LoginToken", token.Value)
            };

            List<BoardEntity> results = new List<BoardEntity>();

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "BoardID")
                {
                    while (reader.Read())
                    {
                        results.Add(
                            new BoardEntity(
                                reader.GetInt32(0),
                                reader.IsDBNull(1) ? -1 : reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetString(3),
                                reader.GetDateTime(4)
                            )
                        );
                    }

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;

            return results;
        }
        #endregion

        #region DAL Objects
        public IProfileDAL GetProfileDAL()
        {
            if (_profileDAL == null)
                _profileDAL = new ProfileDAL(this._connectionString);

            return _profileDAL;
        }
        public IBoardDAL GetBoardDAL()
        {
            if (_boardDAL == null)
                _boardDAL = new BoardDAL(this._connectionString);

            return _boardDAL;
        }
        #endregion
        #endregion
    }
}
