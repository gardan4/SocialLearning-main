using CasusBlok1Jaar2.SocialLearning.Central;

namespace CasusBlok1Jaar2.SocialLearning.DALInterface
{
    public interface IBoardOwnerDAL : IModeratorDAL
    {
        void AddModerator(int boardID, int targetID, LoginToken token);
        void RemoveModerator(int boardID, int targetID, LoginToken token);
        void RemoveBoard(int boardID, LoginToken token);
    }
}
