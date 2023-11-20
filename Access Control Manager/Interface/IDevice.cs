using Access_Control_Manager.Models;

namespace Access_Control_Manager.Interface
{
    public interface IDevice
    {

        Task<IEnumerable<Device>> GetAllDevices();

        Task AddDevice(Device device);

        Task<IEnumerable<Device>> GetStudentDeviceByStudentNumber(long studentNumber);

        Task Save();

    }
}
