using Access_Control_Manager.Database;
using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Access_Control_Manager.Repository
{
    public class ApplicationUserRepository : IApplicationUser
    {
        private readonly AccessControlContext _context;
        private readonly ILogger<IApplicationUser> _logger;

        public ApplicationUserRepository(AccessControlContext context, ILogger<IApplicationUser> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await Save();
        }

        public async Task CreateCampus(Campus campus)
        {
            await _context.Campuses.AddAsync(campus);
            await Save();
        }

        public async Task<IEnumerable<User>> GetAllUsers(string searchQuery, string searchBy)
        {

            _logger.LogInformation($"Search Q : {searchQuery}, search By: {searchBy}");
           
            if (!String.IsNullOrEmpty(searchQuery))
            {
                if (!String.IsNullOrEmpty(searchQuery) && searchBy == "staffNumber")
                {
                   return await _context.Users.Where(a => a.StudentStaffNumber.Contains(searchBy)).ToListAsync();
                }
                if (!String.IsNullOrEmpty(searchQuery) && searchBy == "name")
                {
                   return await _context.Users.Where(a => a.Name.Contains(searchBy)).ToListAsync();
                }
                
                
                   return await _context.Users.Where(a => a.Email.Contains(searchQuery)).ToListAsync();
                
            }
            else
            {
                return await _context.Users.ToListAsync();
            }
          
        }

        public Task<User> GetUserByEmail(string email)
        {
            return _context.Users.
                Include(a => a.Campus).
                AsTracking().
               FirstOrDefaultAsync(a => a.Email == email);
        }

        public Task<User> GetUserByEmailAsNoTracking(string email)
        {
            return _context.Users.
                AsNoTracking().
                FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IEnumerable<Campus>> GetCampus()
        {
            return await _context.Campuses.ToListAsync();
        }

        public async Task<string> GetUserRoleByEmail(string email)
        {
            var user = await _context.Users.Where(a => a.Email == email).FirstOrDefaultAsync();
            string role = user.Role;
            return role;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Update(user);
            await Save();
        }
    }
}
