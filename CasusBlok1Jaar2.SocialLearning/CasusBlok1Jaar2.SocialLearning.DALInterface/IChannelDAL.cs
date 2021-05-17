using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DALInterface
{
    public interface IChannelDAL
    {
        void ToggleChannelActivity(int channelID, LoginToken token);

        MessageEntity PostMessage(int channelID, string body, LoginToken token);
        IEnumerable<MessageEntity> ViewMessages(int channelID, LoginToken token);
        MessageEntity EditMessage(int messageID, string body, LoginToken token);
        void DeleteMessage(int messageID, LoginToken token);
    }
}
