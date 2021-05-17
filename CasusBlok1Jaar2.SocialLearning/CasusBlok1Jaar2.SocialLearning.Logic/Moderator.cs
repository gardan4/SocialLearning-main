using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class Moderator : User
    {
        #region Fields
        private readonly IModeratorDAL _moderatorDAL;
        #endregion

        #region Properties
        public Board Board { get; private set; }
        #endregion

        #region Constructor
        internal Moderator(IModeratorDAL dal, LoginToken token, Board board, int ID, string email, string username, string fullName, int age, int schoolYear)
            : base(dal, token, ID, email, username, fullName, age, schoolYear)
        {
            this._moderatorDAL = dal;
            this.Board = board;
        }
        #endregion

        #region Functions
        #region Board
        /// <summary>
        /// Creates a channel for the board the user moderates.
        /// </summary>
        public bool CreateChannel(string channelName)
        {
            return this.Board.CreateChannel(channelName);
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void RemoveChannel()
        {
            //this.Board.RemoveChannel();
            throw new NotImplementedException();
        }
        #endregion

        #region Moderation
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void WarnUser()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void KickUser()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void BanUser()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void UnbanUser()
        {

        }
        #endregion
        #endregion
    }
}
