using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class Channel
    {
        #region Fields
        private readonly IChannelDAL _channelDAL;
        private readonly LoginToken _token;

        private List<Message> _messages;
        #endregion

        #region Properties
        public int ChannelID { get; private set; }
        public string Name { get; private set; }
        public bool Activity { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a channel object.
        /// </summary>
        internal Channel(IChannelDAL channelDAL, LoginToken token, int channelID, string name, bool activity)
        {
            this._channelDAL = channelDAL;
            this._token = token;
            this.ChannelID = channelID;
            this.Name = name;
            this.Activity = activity;
        }
        #endregion

        #region Functions
        #region Messages
        /// <summary>
        /// Gets the channel's list of messages in order from newest to oldest.
        /// </summary>
        public List<Message> GetMessages()
        {
            if (_messages == null)
            {
                IEnumerable<MessageEntity> messageEntities = _channelDAL.ViewMessages(this.ChannelID, _token);
                _messages = new List<Message>();

                foreach (MessageEntity entity in messageEntities)
                {
                    _messages.Add(new Message(
                        _channelDAL,
                        _token,
                        entity.MessageID,
                        entity.UserID,
                        entity.Body,
                        entity.SentAt,
                        entity.EditedAt
                    ));
                }
            }
            return _messages;
        }
        /// <summary>
        /// Post a message to the channel as the signed in user.
        /// </summary>
        public Message PostMessage(string body)
        {
            MessageEntity entity = _channelDAL.PostMessage(this.ChannelID, body, _token);
            Message message = new Message(_channelDAL, _token, entity.MessageID, entity.UserID, entity.Body, entity.SentAt, entity.EditedAt);

            if (_messages != null)
            {
                _messages.Insert(0, message);
            }

            return message;
        }
        #endregion

        #region Channel Details
        /// <summary>
        /// Toggles the channels activity mode. !NOT ENFORCED IN DATABASE MESSAGING SYSTEM!
        /// </summary>
        public void ToggleActivity()
        {
            Activity = !Activity;
            _channelDAL.ToggleChannelActivity(this.ChannelID, _token);
        }
        #endregion
        #endregion
    }
}
