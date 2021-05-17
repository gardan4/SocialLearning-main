using CasusBlok1Jaar2.SocialLearning.Central;

namespace CasusBlok1Jaar2.SocialLearning.Entities
{
    public class UserEntity
    {
        #region Properties
        public int UserID { get; private set; }
        public LoginToken Token { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string FullName { get; private set; }
        public int Age { get; private set; }
        public int SchoolYear { get; private set; }
        #endregion

        #region Constructors
        public UserEntity(int userID, LoginToken token, string email, string username, string fullname, int age, int schoolYear)
        {
            this.UserID = userID;
            this.Token = token;
            this.Email = email;
            this.Username = username;
            this.FullName = fullname;
            this.Age = age;
            this.SchoolYear = schoolYear;
        }
        #endregion
    }
}
