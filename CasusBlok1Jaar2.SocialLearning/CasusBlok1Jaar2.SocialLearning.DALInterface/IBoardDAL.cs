using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DALInterface
{
    public interface IBoardDAL
    {
        BoardEntity GetBoard(int boardID, LoginToken token);
        UserEntity GetUserRole(int boardID, LoginToken token);

        IEnumerable<BoardEntity> ViewSubboards(int boardID);
        IEnumerable<BoardEntity> SearchSubboards(int boardID, string searchQuery);
        IEnumerable<ChannelEntity> ViewChannels(int boardID, LoginToken token);
        ChannelEntity CreateChannel(int boardID, string name, LoginToken token);
        void RemoveChannel(int channelID, LoginToken token);
        IEnumerable<UserEntity>[] ViewMembers(int boardID, LoginToken token);

        IUserDAL GetUserDAL();
        IModeratorDAL GetModeratorDAL();
        IBoardOwnerDAL GetOwnerDAL();
        IChannelDAL GetChannelDAL();
    }
}
