using CasusBlok1Jaar2.SocialLearning.Entities;
using CasusBlok1Jaar2.SocialLearning.Central;
using System.Collections.Generic;

namespace CasusBlok1Jaar2.SocialLearning.DALInterface
{
    public interface IUserDAL
    {
        UserEntity Register(string email, string username, string password, LoginToken token);
        UserEntity Login(string email, string password, LoginToken token);
        UserEntity GetSession(LoginToken token);
        void ChangePassword(string oldPass, string newPass, LoginToken token);
        
        UserEntity GetUser(int userID); // TEMP IMPLEMENTATION

        void SearchUsers();
        void InviteUserContact();
        void ViewContactInvites();
        void AcceptContact();
        void DenyContact();
        void ViewContacts();
        void DeleteContact();

        BoardEntity CreateBoard(string name, LoginToken token, int parentBoard = -1);
        IEnumerable<BoardEntity> ViewBoards();
        IEnumerable<BoardEntity> SearchBoards(string searchQuery);
        void JoinBoard(int boardID, LoginToken token);
        void LeaveBoard(int boardID, LoginToken token);
        IEnumerable<BoardEntity> GetActiveBoards(LoginToken token);

        IProfileDAL GetProfileDAL();
        IBoardDAL GetBoardDAL();
    }
}
