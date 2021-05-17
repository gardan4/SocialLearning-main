using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class Profile
    {
        #region Fields
        private readonly IProfileDAL _profileDAL;
        private readonly LoginToken _token;
        private readonly User _user;
        #endregion

        #region Properties


        //public int UserID { get; set; }
        //public string Email { get; set; }

        //public int SocialMediaGroupID { get; set; }
        //public string Type { get; set; }
        //public string Link { get; set; }


        //public List<int> InterestsIDList { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a profile object.
        /// </summary>
        internal Profile(IProfileDAL profileDAL, User user)
        {
            this._profileDAL = profileDAL;
            this._token = user.Token;
            this._user = user;

            LoadUserDetails();
        }
        #endregion

        #region Functions
        #region User Information
        /// <summary>
        /// Updates a user's profile details
        /// </summary>
        public bool UpdateProfile(string password, string email, string username, string fullName, int age, int schoolYear)
        {
            string lt = _token.Value;
            _profileDAL.UpdateProfile(password, new UserEntity(
                    _user.UserID,
                    _user.Token,
                    email,
                    username,
                    fullName,
                    age,
                    schoolYear
                ), _token);

            if (lt != _token.Value)
            {
                LoadUserDetails();
                return true;
            }

            return false;
        }
        #endregion

        #region Privacy
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void TogglePrivacy()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void ToggleBoardSharing()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Interests
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void AddInterest()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void RemoveInterest()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void GetInterests()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Contact Details
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void GetContactDetails()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void ManageContactDetails()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void ManageSocialMedia()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void ManageSocialMediaGroups()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Loads the user's details from profile into a user.
        /// </summary>
        private void LoadUserDetails()
        {
            UserEntity entity = _profileDAL.GetProfile(_token, _user.UserID);

            _user.UpdateUserDetails(entity.Email, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
        }
        #endregion
    }
}
