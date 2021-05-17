using CasusBlok1Jaar2.SocialLearning.DALInterface;
using System;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public class ModeratorDAL : UserDAL, IModeratorDAL
    {
        #region Constructors
        public ModeratorDAL(string connectionString) : base(connectionString)
        {
        }
        #endregion

        #region Functions
        #region Users
        public void WarnUser()
        {
            throw new NotImplementedException();
        }
        public void KickUser()
        {
            throw new NotImplementedException();
        }
        public void BanUser()
        {
            throw new NotImplementedException();
        }
        public void UnbanUser()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
