using Access_Control_Manager.Models;

namespace Access_Control_Manager.Interface
{
    public interface IAccount
    {
        Task SignUp(SignUp signUp);

        Task<string> Login(Models.Login login);
    }
}
