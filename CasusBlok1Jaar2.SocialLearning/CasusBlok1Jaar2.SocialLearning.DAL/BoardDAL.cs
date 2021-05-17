using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public class BoardDAL : DatabaseDAL, IBoardDAL
    {
        #region Fields
        private UserDAL _userDAL;
        private ModeratorDAL _moderatorDAL;
        private BoardOwnerDAL _ownerDAL;
        private ChannelDAL _channelDAL;
        #endregion

        #region Constructors
        public BoardDAL(string connectionString) : base(connectionString)
        {
        }
        #endregion

        #region Functions
        #region User
        public BoardEntity GetBoard(int boardID, LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC dbo.spBoards_GetBoard @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            int parentBoard = -1;
            int ownerID = -1;
            string name = string.Empty;
            DateTime createdAt = DateTime.Now;

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "BoardID")
                {
                    reader.Read();

                    boardID = reader.GetInt32(0);
                    parentBoard = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                    ownerID = reader.GetInt32(4);
                    name = reader.GetString(2);
                    createdAt = reader.GetDateTime(3);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters.ToArray());

            token.Value = lt;
            return boardID == -1 ? null : new BoardEntity(boardID, parentBoard, ownerID, name, createdAt);
        }
        public UserEntity GetUserRole(int boardID, LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spBoards_GetRole @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            int role = -1;

            int userID = -1;
            string email = null;
            string username = null;

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "Role")
                {
                    reader.Read();

                    role = reader.GetInt32(0);


                    reader.NextResult();
                    reader.Read();

                    userID = reader.GetInt32(0);
                    email = reader.GetString(1);
                    username = reader.GetString(2);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters.ToArray());
            
            token.Value = lt;


            UserEntity result;

            switch (role)
            {
                case 1:
                    result = new BoardOwnerEntity(boardID, userID, token, email, username, null, -1, -1);
                    break;
                case 2:
                    result = new ModeratorEntity(boardID, userID, token, email, username, null, -1, -1);
                    break;
                case 3:
                    result = new UserEntity(userID, token, email, username, null, -1, -1);
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
        #endregion

        #region Boards & Channels
        public IEnumerable<BoardEntity> ViewSubboards(int boardID)
        {
            string query = "EXEC dbo.spBoards_SearchBoards '', @BoardID;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID)
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
        public IEnumerable<BoardEntity> SearchSubboards(int boardID, string searchQuery)
        {
            string query = "EXEC dbo.spBoards_SearchBoards @SearchQuery, @BoardID;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@SearchQuery", searchQuery ?? string.Empty),
                new SqlParameter("@BoardID", boardID)
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
        public IEnumerable<ChannelEntity> ViewChannels(int boardID, LoginToken token)
        {
            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spBoards_ViewChannels @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            List<ChannelEntity> results = new List<ChannelEntity>();

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "ChannelID")
                {
                    while (reader.Read())
                    {
                        results.Add(
                            new ChannelEntity(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.IsDBNull(2) ? true : reader.GetBoolean(2)
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
        public ChannelEntity CreateChannel(int boardID, string name, LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginTOken;" +
                           "EXEC spChannels_CreateChannel @Token OUTPUT, @BoardID, @ChannelName;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@ChannelName", name),
                new SqlParameter("@LoginToken", token.Value)
            };

            int channelID = -1;
            bool activity = true;

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "ChannelID")
                {
                    reader.Read();
                    channelID = reader.GetInt32(0);
                    activity = reader.GetBoolean(2);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return channelID == -1 ? null : new ChannelEntity(channelID, name, activity);
        }
        public void RemoveChannel(int channelID, LoginToken token)
        {
            if (token.Value == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spChannels_RemoveChannel @Token OUTPUT, @ChannelID;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@ChannelID", channelID),
                new SqlParameter("@LoginToken", token.Value)
            };

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                reader.Read();
                lt = reader.GetString(0);

            }, sqlParameters);

            token.Value = lt;
        }
        public IEnumerable<UserEntity>[] ViewMembers(int boardID, LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spBoards_ViewMembers @Token OUTPUT, @BoardID;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BoardID", boardID),
                new SqlParameter("@LoginToken", token.Value)
            };

            List<UserEntity>[] results = new List<UserEntity>[]
            {
                new List<UserEntity>(),
                new List<UserEntity>(),
                new List<UserEntity>()
            };

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "UserID")
                {
                    for (int i=0; i < 3; i++)
                    {
                        while (reader.Read())
                        {
                            results[i].Add(new UserEntity(
                                reader.GetInt32(0),
                                null,
                                reader.GetString(1),
                                reader.GetString(2),
                                null,
                                -1,
                                -1
                            ));
                        }

                        reader.NextResult();
                    }

                    reader.Read();
                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return results;
        }
        #endregion

        #region DAL Objects
        public IUserDAL GetUserDAL()
        {
            if (_userDAL == null)
                _userDAL = new UserDAL(this._connectionString);

            return _userDAL;
        }
        public IModeratorDAL GetModeratorDAL()
        {
            if (_moderatorDAL == null)
                _moderatorDAL = new ModeratorDAL(this._connectionString);

            return _moderatorDAL;
        }
        public IBoardOwnerDAL GetOwnerDAL()
        {
            if (_ownerDAL == null)
                _ownerDAL = new BoardOwnerDAL(this._connectionString);

            return _ownerDAL;
        }
        public IChannelDAL GetChannelDAL()
        {
            if (_channelDAL == null)
                _channelDAL = new ChannelDAL(this._connectionString);

            return _channelDAL;
        }
        #endregion
        #endregion
    }
}
