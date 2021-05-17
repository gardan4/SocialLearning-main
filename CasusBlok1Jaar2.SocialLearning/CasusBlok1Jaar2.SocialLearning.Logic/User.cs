using CasusBlok1Jaar2.SocialLearning.DALInterface;
using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.Logic
{
    public class User
    {
        #region Fields
        private readonly IUserDAL _userDAL;

        private List<Board> _activeBoards;
        #endregion

        #region Properties
        // Signed in identification
        public LoginToken Token { get; private set; }
        public bool IsSignedIn { get { return !string.IsNullOrEmpty(Token?.Value); } }
        
        // User details
        public int UserID { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string FullName { get; private set; }
        public int Age { get; private set; }
        public int SchoolYear { get; private set; }

        // Boards
        public List<Board> ActiveBoards {
            get {
                if (_activeBoards == null)
                    RefreshActiveBoards();

                return _activeBoards;
        }}

        // NOT IMPLEMENTED
        public List<int> FriendshipID { get; set; }
        public List<int> InvitingUserID { get; set; }
        public List<int> InviteeUserID { get; set; }
        public string Status { get; set; }

        //public List<int> ModeratorBoardID { get; set; }
        //!!! Never safe a password in a field for longer than needed, not even hashed -> // Gehaste string Password
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a signed in user object.
        /// </summary>
        internal User(IUserDAL dal, LoginToken token, int ID, string email, string username, string fullName, int age, int schoolYear)
        {
            this._userDAL = dal;
            this.Token = token;
            this.UserID = ID;
            this.Email = email;
            this.Username = username;
            this.FullName = fullName;
            this.Age = age;
            this.SchoolYear = schoolYear;
        }
        internal User(IUserDAL dal, int ID, string email, string username, string fullName, int age, int schoolYear)
            : this(dal, null, ID, email, username, fullName, age, schoolYear) { }
        #endregion

        #region Factories
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <returns>Returns null if failed, or a user object on success.</returns>
        public static User Register(IUserDAL uDal, string email, string username, string password)
        {
            LoginToken token = new LoginToken(null);

            UserEntity entity = uDal.Register(email, username, password, token);

            return entity == null ? null : new User(uDal, token, entity.UserID, entity.Username, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
        }
        /// <summary>
        /// Signs into a user.
        /// </summary>
        /// <returns>Returns null if failed, or a user object on success.</returns>
        public static User Login(IUserDAL uDal, string email, string password)
        {
            LoginToken token = new LoginToken(null);

            UserEntity entity = uDal.Login(email, password, token);

            return entity == null ? null : new User(uDal, token, entity.UserID, entity.Username, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
        }
        /// <summary>
        /// Gets a user based on the login token.
        /// </summary>
        /// <returns>Returns null if failed, or a user object on success.</returns>
        public static User GetSession(IUserDAL uDal, LoginToken token)
        {
            UserEntity entity = uDal.GetSession(token);

            return entity == null ? null : new User(uDal, token, entity.UserID, entity.Username, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
        }
        #endregion

        #region Functions
        /// <summary>
        /// !TEMPORARY! Gets a user (non-signed in) based on id.
        /// </summary>
        public User GetUserByID(int userID)
        {
            UserEntity entity = _userDAL.GetUser(userID);

            return entity == null ? null : new User(_userDAL, null, entity.UserID, entity.Username, entity.Username, entity.FullName, entity.Age, entity.SchoolYear);
        }

        #region Profile
        /// <summary>
        /// Gets the user's profile page.
        /// </summary>
        public Profile GetProfile()
        {
            return new Profile(_userDAL.GetProfileDAL(), this);
        }
        internal void UpdateUserDetails(string email, string username, string fullName, int age, int schoolYear)
        {
            this.Email = email;
            this.Username = username;
            this.FullName = fullName;
            this.Age = age;
            this.SchoolYear = schoolYear;
        }
        public bool ChangePassword(string oldPass, string newPass)
        {
            string lt = Token.Value;
            _userDAL.ChangePassword(oldPass, newPass, Token);

            return lt != Token.Value;
        }
        #endregion

        #region Boards
        /// <summary>
        /// Get a list of all available boards.
        /// </summary>
        public List<Board> ViewBoards()
        {
            IEnumerable<BoardEntity> boardEntities = _userDAL.ViewBoards();
            List<Board> boards = new List<Board>();

            foreach (BoardEntity entity in boardEntities)
            {
                boards.Add(new Board(
                    _userDAL.GetBoardDAL(),
                    Token,
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
        /// Get a list of available boards, filtered by string literal.
        /// </summary>
        public List<Board> SearchBoards(string searchString)
        {
            IEnumerable<BoardEntity> boardEntities = _userDAL.SearchBoards(searchString);
            List<Board> boards = new List<Board>();

            foreach (BoardEntity entity in boardEntities)
            {
                boards.Add(new Board(
                    _userDAL.GetBoardDAL(),
                    Token,
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
        /// Make a new board for signed in user.
        /// </summary>
        public bool MakeBoard(string name, int parentID = -1)
        {
            if (!IsSignedIn)
                return false;

            BoardEntity entity = _userDAL.CreateBoard(name, Token, parentID);

            if (entity == null)
                return false;

            Board board = new Board(_userDAL.GetBoardDAL(), Token, entity.BoardID, entity.ParentBoardID, entity.OwnerID, entity.Name, entity.DateCreated);

            if (_activeBoards != null)
                _activeBoards.Add(board);

            return true;
        }
        /// <summary>
        /// Join a board for signed in user.
        /// </summary>
        public void JoinBoard(int boardID)
        {
            if (!IsSignedIn)
                return;

            string lt = Token.Value;
            _userDAL.JoinBoard(boardID, Token);

            // If the user token has changed, attempt was successful, refresh boards
            if (lt != Token.Value)
                RefreshActiveBoards();
        }
        /// <summary>
        /// Leave a board for signed in user.
        /// </summary>
        public void LeaveBoard(int boardID)
        {
            if (!IsSignedIn)
                return;

            string lt = Token.Value;
            _userDAL.LeaveBoard(boardID, Token);

            // If the user token has changed, attempt was successful, refresh boards
            if (lt != Token.Value)
                RefreshActiveBoards();
        }
        #endregion

        #region Contacts
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public List<int> SearchUSers(string SearchString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void ViewContacts()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void ViewInvites()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void InviteUser()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void AcceptInvite()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// !NOT IMPLEMENTED!
        /// </summary>
        public void DenyInvite()
        {
            throw new NotImplementedException();
        }
        #endregion


        /*
        public void SendMessage()
        {
            throw new NotImplementedException();
        }

        public void GetMessages()
        {
            throw new NotImplementedException();
        }

        public void EditMessage()
        {
            throw new NotImplementedException();
        }

        */
        #endregion

        #region Methods
        /// <summary>
        /// Load the active boards from database into memory.
        /// </summary>
        private void RefreshActiveBoards()
        {
            IEnumerable<BoardEntity> boardEntities = _userDAL.GetActiveBoards(Token);
            _activeBoards = new List<Board>();

            foreach (BoardEntity entity in boardEntities)
            {
                _activeBoards.Add(new Board(
                    _userDAL.GetBoardDAL(),
                    Token,
                    entity.BoardID,
                    entity.ParentBoardID,
                    entity.OwnerID,
                    entity.Name,
                    entity.DateCreated
                ));
            }
        }
        #endregion
    }
}
