namespace CasusBlok1Jaar2.SocialLearning.Central
{
    /// <summary>
    /// The login token for a user session
    /// </summary>
    public class LoginToken
    {
        public string Value { get; set; }

        public LoginToken(string value)
        {
            this.Value = value;
        }
    }
}
