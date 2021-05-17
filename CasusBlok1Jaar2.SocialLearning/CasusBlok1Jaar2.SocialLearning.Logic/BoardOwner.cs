using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class BoardOwner : Moderator
    {
        #region Fields
        private readonly IBoardOwnerDAL _ownerDAL;
        #endregion

        #region Constructors
        internal BoardOwner(IBoardOwnerDAL dal, LoginToken token, Board board, int ID, string email, string username, string fullName, int age, int schoolYear)
            : base(dal, token, board, ID, email, username, fullName, age, schoolYear)
        {
            this._ownerDAL = dal;
        }
        #endregion

        #region Functions

        // CANNOT UPDATE MODERATOR LIST. -> Implement update function for board.
        #region Moderators
        /// <summary>
        /// Add new moderator to board.
        /// </summary>
        public void AddModerator(int targetUserID)
        {
            _ownerDAL.AddModerator(this.Board.BoardID, targetUserID, Token);
        }
        /// <summary>
        /// Remove a moderator from the board (remains a member).
        /// </summary>
        public void RemoveModerator(int targetUserID)
        {
            _ownerDAL.RemoveModerator(this.Board.BoardID, targetUserID, Token);
        }
        /// <summary>
        /// Get a list of moderators from the board.
        /// </summary>
        public List<User> GetModerators()
        {
            return this.Board.GetMembers()[1];
        }
        #endregion

        #region Board
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void RemoveSubboard()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void DeleteBoard()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
