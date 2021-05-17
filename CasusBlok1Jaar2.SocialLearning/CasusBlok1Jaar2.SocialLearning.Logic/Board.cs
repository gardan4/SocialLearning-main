using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class Board
    {
        #region Fields
        private readonly IBoardDAL _boardDAL;
        private readonly LoginToken _token;

        private int _parentBoardID;
        private Board _parentBoard;

        private int _ownerUserID;
        private User _ownerUser;
        
        private List<Channel> _channels;
        private List<User> _members;
        private List<User> _moderators;
        #endregion

        #region Properties
        // Board details
        public int BoardID { get; private set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        // Not Implemented
        public List<Moderator> BoardModeratorList;
        public List<int> BoardUsersIDList = new List<int>();
        public List<int> BoardBannedUserIDList = new List<int>();

        #endregion

        #region Constructors#
        /// <summary>
        /// Creates a board object.
        /// </summary>
        internal Board(IBoardDAL bDAL, LoginToken token, int boardID, int parentBoardID, int ownerID, string name, DateTime dateCreated)
        {
            this._boardDAL = bDAL;
            this._token = token;
            this.BoardID = boardID;
            this._parentBoardID = parentBoardID;
            this._ownerUserID = ownerID;
            this.Name = name;
            this.DateCreated = dateCreated;
        }
        #endregion


        #region Functions
        #region Board Details
        /// <summary>
        /// Get the board's parent board if such board exists.
        /// </summary>
        public Board GetParentBoard()
        {
            if (_parentBoard == null)
            {
                if (_parentBoardID == -1)
                    return null;

                BoardEntity entity = _boardDAL.GetBoard(_parentBoardID, _token);
                _parentBoard = new Board(_boardDAL, _token, entity.BoardID, entity.ParentBoardID, entity.OwnerID, entity.Name, entity.DateCreated);
            }

            return _parentBoard;
        }
        /// <summary>
        /// !PARTIALLY IMPLEMENTED! Only gets owner after GetMembers has been called.
        /// </summary>
        public User GetBoardOwner()
        {
            if (_ownerUser == null)
            {
                // No DAL procedure exists yet to get owner.
            }

            return _ownerUser;
        }
        /// <summary>
        /// Gets a signed in user's object of member type <see cref="User"/>, <see cref="Moderator"/> or <see cref="BoardOwner"/>, or null if not a member.
        /// </summary>
        public User GetUserRole()
        {
            UserEntity entity = _boardDAL.GetUserRole(this.BoardID, _token);

            Type role = entity?.GetType();

            if (role == typeof(BoardOwnerEntity))
                return new BoardOwner(_boardDAL.GetOwnerDAL(), _token, this, entity.UserID, entity.Email, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
            else if (role == typeof(ModeratorEntity))
                return new Moderator(_boardDAL.GetModeratorDAL(), _token, this, entity.UserID, entity.Email, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
            else if (role == typeof(UserEntity))
                return new User(_boardDAL.GetUserDAL(), _token, entity.UserID, entity.Email, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);

            return null;
        }
        /// <summary>
        /// Get's the board's members
        /// </summary>
        public List<User>[] GetMembers()
        {
            if (_members == null || _moderators == null || _ownerUser == null)
            {
                IEnumerable<UserEntity>[] userEntities = _boardDAL.ViewMembers(this.BoardID, _token);

                _members = new List<User>();
                _moderators = new List<User>();

                if (userEntities[0].Count() > 0)
                {
                    UserEntity ownerEntity = userEntities[0].ToArray()[0];

                    _ownerUser = new User(_boardDAL.GetUserDAL(), ownerEntity.UserID, ownerEntity.Email, ownerEntity.Username, ownerEntity.FullName, ownerEntity.Age, ownerEntity.SchoolYear);
                }
                
                foreach (UserEntity entity in userEntities[1])
                {
                    _moderators.Add(new User(_boardDAL.GetUserDAL(), entity.UserID, entity.Email, entity.Username, entity.FullName, entity.Age, entity.SchoolYear));
                }

                foreach (UserEntity entity in userEntities[2])
                {
                    _members.Add(new User(_boardDAL.GetUserDAL(), entity.UserID, entity.Email, entity.Username, entity.FullName, entity.Age, entity.SchoolYear));
                }
            }

            return new List<User>[]
            {
                new List<User>()
                {
                    _ownerUser
                },
                _moderators,
                _members
            };
        }
        #endregion

        #region Channels
        public List<Channel> ViewChannels()
        {
            if (_channels != null)
                return _channels;

            IEnumerable<ChannelEntity> channelEntities = _boardDAL.ViewChannels(this.BoardID, _token);
            _channels = new List<Channel>();

            foreach (ChannelEntity entity in channelEntities)
            {
                _channels.Add(new Channel(
                    _boardDAL.GetChannelDAL(),
                    _token,
                    entity.ChannelID,
                    entity.Name,
                    entity.Activity
                ));
            }

            return _channels;
        }

        /// <summary>
        /// Create a new channel on the board.
        /// </summary>
        public bool CreateChannel(string name)
        {
            ChannelEntity entity = _boardDAL.CreateChannel(this.BoardID, name, _token);

            if (entity == null)
                return false;

            Channel board = new Channel(_boardDAL.GetChannelDAL(), _token, entity.ChannelID, entity.Name, entity.Activity);

            if (_channels != null)
                _channels.Add(board);

            return true;
        }
        /// <summary>
        /// Removes a channel for the board.
        /// </summary>
        public void RemoveChannel(int channelID)
        {            
            _boardDAL.RemoveChannel(channelID, _token);
        }
        #endregion

        #region Subboards
        /// <summary>
        /// Gets the board's available child boards
        /// </summary>
        public List<Board> GetSubboards()
        {
            IEnumerable<BoardEntity> boardEntities = _boardDAL.ViewSubboards(this.BoardID);
            List<Board> boards = new List<Board>();

            foreach (BoardEntity entity in boardEntities)
            {
                boards.Add(new Board(
                    _boardDAL,
                    _token,
                    entity.BoardID,
                    entity.ParentBoardID,
                    entity.OwnerID,
                    entity.Name,
                    entity.DateCreated
                ));
            }

            return boards;
        }
        /// <summary>
        /// Searches with the board's child boards.
        /// </summary>
        public List<Board> SearchSubboards(string searchString)
        {
            IEnumerable<BoardEntity> boardEntities = _boardDAL.SearchSubboards(this.BoardID, searchString);
            List<Board> boards = new List<Board>();

            foreach (BoardEntity entity in boardEntities)
            {
                boards.Add(new Board(
                    _boardDAL,
                    _token,
                    entity.BoardID,
                    entity.ParentBoardID,
                    entity.OwnerID,
                    entity.Name,
                    entity.DateCreated
                ));
            }

            return boards;
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void RemoveSubboard(int RemoveBoardID, string RemoveBoardName)
        {
            //Class1.BoardClassList.Remove(new Board(RemoveBoardID, SignedInBoardID.ToString(), OwnerUserID, RemoveBoardName, DateCreated));
            //Class1.UserClassList[User.SignedInUserID].ActiveBoardsID.Remove(RemoveBoardID);
            throw new NotImplementedException();
        }
        #endregion

        // Later implementation
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void RemoveMember()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
