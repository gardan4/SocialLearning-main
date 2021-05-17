using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public class ProfileDAL : DatabaseDAL, IProfileDAL
    {
        #region Constructors
        public ProfileDAL(string connectionString) : base(connectionString)
        {
        }
        #endregion

        #region Functions
        #region Account Data
        public UserEntity GetProfile(LoginToken token, int targetUserID = -1)
        {
            if (token.Value == null)
                return null;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           $"EXEC spUsers_GetProfile @Token OUTPUT{(targetUserID == -1 ? "" : ", @TargetID")};" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@LoginToken", token.Value)
            };

            if (targetUserID != -1)
                sqlParameters.Add(new SqlParameter("@TargetID", targetUserID));

            int userID = -1;
            string username = null;
            string email = null;
            string fullname = null;
            int age = -1;
            int schoolYear = -1;

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "UserID")
                {
                    reader.Read();
                    userID = reader.GetInt32(0);
                    email = reader.GetString(1);
                    username = reader.GetString(2);
                    fullname = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    age = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                    schoolYear = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);

                    reader.NextResult();
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters.ToArray());

            token.Value = lt;
            return userID == -1 ? null : new UserEntity(userID, targetUserID == -1 ? token : null, email, username, fullname, age, schoolYear);
        }
        public void UpdateProfile(string password, UserEntity profile, LoginToken token)
        {
            password = GetHashString(password);

            if (token.Value == null)
                return;

            string query = "DECLARE @Token nvarchar(256);" +
                           "SET @Token = @LoginToken;" +
                           "EXEC spUsers_UpdateProfile @Token OUTPUT, @Pass" +
                           $"{(string.IsNullOrEmpty(profile.Email) ? "" : ", @Email = @Email")}" +
                           $"{(string.IsNullOrEmpty(profile.Username) ? "" : ", @Username = @Username")}" +
                           $"{(string.IsNullOrEmpty(profile.FullName) ? "" : ", @Fullname = @Fullname")}" +
                           $"{(profile.Age == -1 ? "" : ", @Age = @Age")}" +
                           $"{(profile.SchoolYear == -1 ? "" : ", @SchoolYear = @SchoolYear")};" +
                           "SELECT[Token] = @Token;";

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Pass", password),
                new SqlParameter("@LoginToken", token.Value)
            };

            if (!string.IsNullOrEmpty(profile.Email))
                sqlParameters.Add(new SqlParameter("@Email", profile.Email));
            if (!string.IsNullOrEmpty(profile.Username))
                sqlParameters.Add(new SqlParameter("@Username", profile.Username));
            if (!string.IsNullOrEmpty(profile.FullName))
                sqlParameters.Add(new SqlParameter("@Fullname", profile.FullName));
            if (profile.Age != -1)
                sqlParameters.Add(new SqlParameter("@Age", profile.Age));
            if (profile.SchoolYear != -1)
                sqlParameters.Add(new SqlParameter("@SchoolYear", profile.SchoolYear));

            string lt = token.Value;

            ExecuteQuery(query, (reader) =>
            {
                if (reader.GetName(0) == "UserID")
                {
                    reader.Read();

                    lt = reader.GetString(0);
                }
            }, sqlParameters.ToArray());

            token.Value = lt;
            return;
        }
        #endregion

        #region Interests
        public void AddInterest()
        {
            throw new NotImplementedException();
        }
        public void ViewInterests()
        {
            throw new NotImplementedException();
        }
        public void RemoveInterest()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Contact Details
        public void GetContactDetails()
        {
            throw new NotImplementedException();
        }

        public void SetContactDetails()
        {
            throw new NotImplementedException();
        }
        public void RemoveContactDetails()
        {
            throw new NotImplementedException();
        }

        public void AddSocialMediaGroup()
        {
            throw new NotImplementedException();
        }
        public void EditSocialMediaGroup()
        {
            throw new NotImplementedException();
        }
        public void RemoveSocialMediaGroup()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
