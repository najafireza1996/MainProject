
namespace Common.Exceptions.Users
{
    public class UsernameAlreadyExistsException : CustomException
    {
        public UsernameAlreadyExistsException(string username)
            : base($"Username {username} Already Exits!")
        {
        }
    }
}
