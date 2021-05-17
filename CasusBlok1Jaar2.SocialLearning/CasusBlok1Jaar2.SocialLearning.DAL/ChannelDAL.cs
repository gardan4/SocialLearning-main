using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public class ChannelDAL : DatabaseDAL, IChannelDAL
    {
        #region Constructors
        public ChannelDAL(string connectionString) : base(connectionString)
        {
        }
        #endregion

        #region Functions
        public void ToggleChannelActivity(int channelID, LoginToken token)
        {
            if (token.Value == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spChannels_ToggleActivity @Token OUTPUT, @ChannelID;" +
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

        public MessageEntity PostMessage(int channelID, string body, LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spChannels_PostMessage @Token OUTPUT, @ChannelID, @Message;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Message", body),
                new SqlParameter("@ChannelID", channelID),
                new SqlParameter("@LoginToken", token.Value)
            };

            int messageID = -1;
            int userID = -1;
            DateTime sentAt = DateTime.Now.AddYears(1000);

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "MessageID")
                {
                    reader.Read();

                    messageID = reader.GetInt32(0);
                    userID = reader.GetInt32(1);
                    sentAt = reader.GetDateTime(3);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return messageID == -1 ? null : new MessageEntity(messageID, userID, body, sentAt, DateTime.MaxValue);
        }
        public IEnumerable<MessageEntity> ViewMessages(int channelID, LoginToken token)
        {
            if (token.Value == null)
                return new List<MessageEntity>();

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spChannels_ViewMessages @Token OUTPUT, @ChannelID;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@ChannelID", channelID),
                new SqlParameter("@LoginToken", token.Value)
            };

            List<MessageEntity> messages = new List<MessageEntity>();

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "MessageID")
                {
                    while(reader.Read())
                    {
                        messages.Add(new MessageEntity(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetString(2),
                            reader.GetDateTime(3),
                            reader.IsDBNull(4) ? DateTime.MaxValue : reader.GetDateTime(4)
                        ));
                    }

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return messages;
        }
        public MessageEntity EditMessage(int messageID, string body, LoginToken token)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spChannels_EditMessage @Token OUTPUT, @MessageID, @Message;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Message", body),
                new SqlParameter("@MessageID", messageID),
                new SqlParameter("@LoginToken", token.Value)
            };
            
            int userID = -1;
            DateTime sentAt = DateTime.MaxValue;
            DateTime editedAt = DateTime.MaxValue;

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "MessageID")
                {
                    reader.Read();
                    
                    userID = reader.GetInt32(1);
                    sentAt = reader.GetDateTime(3);
                    editedAt = reader.GetDateTime(4);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters);

            token.Value = lt;
            return messageID == -1 ? null : new MessageEntity(messageID, userID, body, sentAt, editedAt);
        }
        public void DeleteMessage(int messageID, LoginToken token)
        {
            if (token.Value == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spChannels_DeleteMessage @Token OUTPUT, @MessageID;" +
                           "SELECT[Token] = @Token;";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@MessageID", messageID),
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
        #endregion
    }
}
