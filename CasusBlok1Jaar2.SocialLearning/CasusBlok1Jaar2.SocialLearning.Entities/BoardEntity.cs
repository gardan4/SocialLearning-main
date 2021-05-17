using System;

namespace CasusBlok1Jaar2.SocialLearning.Entities
{
    public class BoardEntity
    {
        #region Properties
        public int BoardID { get; private set; }
        public int ParentBoardID { get; private set; }
        public int OwnerID { get; private set; }
        public string Name { get; private set; }
        public DateTime DateCreated { get; private set; }
        #endregion

        #region Constructors
        public BoardEntity(int boardID, int parentID, int ownerID, string name, DateTime dateCreated)
        {
            this.BoardID = boardID;
            this.ParentBoardID = parentID;
            this.OwnerID = ownerID;
            this.Name = name;
            this.DateCreated = dateCreated;
        }
        #endregion
    }
}
