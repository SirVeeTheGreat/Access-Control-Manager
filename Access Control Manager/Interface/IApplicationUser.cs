using Access_Control_Manager.Models;

namespace Access_Control_Manager.Interface
{
    public interface IApplicationUser
    {
        Task<IEnumerable<User>> GetAllUsers(string searchQuery, string searchBy);

        Task CreateUser(User user);

        Task UpdateUser(User user);

        Task<User> GetUserByEmail(string email);

        Task<string> GetUserRoleByEmail(string email);

        Task<User> GetUserByEmailAsNoTracking(string email);

        Task<IEnumerable<Campus>> GetCampus();

        Task CreateCampus(Campus campus);

        Task Save();

    }
}
