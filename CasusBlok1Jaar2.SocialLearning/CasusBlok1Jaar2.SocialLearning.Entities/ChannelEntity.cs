namespace CasusBlok1Jaar2.SocialLearning.Entities
{
    public class ChannelEntity
    {
        #region Properties
        public int ChannelID { get; private set; }
        public string Name { get; private set; }
        public bool Activity { get; private set; }
        #endregion

        #region Constructors
        public ChannelEntity(int channelID, string name, bool activity)
        {
            this.ChannelID = channelID;
            this.Name = name;
            this.Activity = activity;
        }
        #endregion
    }
}
