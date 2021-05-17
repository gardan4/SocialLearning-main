namespace CasusBlok1Jaar2.SocialLearning.DALInterface
{
    public interface IModeratorDAL : IUserDAL
    {
        void WarnUser();
        void KickUser();
        void BanUser();
        void UnbanUser();
    }
}
