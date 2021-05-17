using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class Message
    {
        #region Fields
        private readonly IChannelDAL _channelDAL;
        private readonly LoginToken _token;
        #endregion

        #region Properties
        public int MessageID { get; private set; }
        public int ChannelID { get; private set; }
        public int UserID { get; private set; }
        public string Body { get; private set; }
        public DateTime SentAt { get; private set; }
        public DateTime EditedAt { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a message object.
        /// </summary>
        internal Message(IChannelDAL channelDal, LoginToken token, int messageID, int userID, string body, DateTime sentAt, DateTime editedAt)
        {
            this._channelDAL = channelDal;
            this._token = token;
            this.MessageID = messageID;
            this.UserID = userID;
            this.Body = body;
            this.SentAt = sentAt;
            this.EditedAt = editedAt;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Edits the message body if signed in user has permission to.
        /// </summary>
        public void EditMessage(string body)
        {
            this.Body = body;
            _channelDAL.EditMessage(this.MessageID, body, _token);
        }
        /// <summary>
        /// Deletes the message if signed-in user has permission to.
        /// </summary>
        public void DeleteMessage()
        {
            _channelDAL.DeleteMessage(this.MessageID, _token);
        }
        #endregion

    }
}
