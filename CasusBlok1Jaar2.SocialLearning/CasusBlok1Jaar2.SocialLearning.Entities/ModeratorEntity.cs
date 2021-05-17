using CasusBlok1Jaar2.SocialLearning.Central;

namespace CasusBlok1Jaar2.SocialLearning.Entities
{
    public class ModeratorEntity : UserEntity
    {
        #region Properties
        public int BoardID { get; private set; }
        #endregion

        #region Constructors
        public ModeratorEntity(int boardID, int userID, LoginToken token, string email, string username, string fullname, int age, int schoolYear)
            : base(userID, token, email, username, fullname, age, schoolYear)
        {
            this.BoardID = boardID;
        }
        #endregion
    }
}
