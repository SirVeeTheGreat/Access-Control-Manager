using Access_Control_Manager.Database;
using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Access_Control_Manager.Repository
{
    public class DeviceRepository : IDevice
    {

        private readonly AccessControlContext _context;

        public DeviceRepository(AccessControlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task AddDevice(Device device)
        {
            await _context.Devices.AddAsync(device);
            await Save();
        }

        public Task<IEnumerable<Device>> GetStudentDeviceByStudentNumber(long studentNumber)
        {
            throw new NotImplementedException();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
