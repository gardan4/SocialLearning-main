using System;

namespace CasusBlok1Jaar2.SocialLearning.Entities
{
    public class MessageEntity
    {
        #region Properties
        public int MessageID { get; private set; }
        public int UserID { get; private set; }
        public string Body { get; private set; }
        public DateTime SentAt { get; private set; }
        public DateTime EditedAt { get; private set; }
        #endregion

        #region Constructors
        public MessageEntity(int messageID, int userID, string body, DateTime sentAt, DateTime editedAt)
        {
            this.MessageID = messageID;
            this.UserID = userID;
            this.Body = body;
            this.SentAt = sentAt;
            this.EditedAt = editedAt;
        }
        #endregion
    }
}
