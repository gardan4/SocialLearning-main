using CasusBlok1Jaar2.SocialLearning.Central;

namespace CasusBlok1Jaar2.SocialLearning.Entities
{
    public class BoardOwnerEntity : ModeratorEntity
    {
        #region Properties

        #endregion

        #region Constructors
        public BoardOwnerEntity(int boardID, int userID, LoginToken token, string email, string username, string fullname, int age, int schoolYear)
            : base(boardID, userID, token, email, username, fullname, age, schoolYear)
        {

        }
        #endregion
    }
}
