using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;

namespace CasusBlok1Jaar2.SocialLearning.DALInterface
{
    public interface IProfileDAL
    {
        UserEntity GetProfile(LoginToken token, int targetUserID = -1);
        void UpdateProfile(string password, UserEntity profile, LoginToken token);

        void AddInterest();
        void ViewInterests();
        void RemoveInterest();

        void GetContactDetails();
        void SetContactDetails();
        void RemoveContactDetails();
        void AddSocialMediaGroup();
        void EditSocialMediaGroup();
        void RemoveSocialMediaGroup();
    }
}