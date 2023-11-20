using Access_Control_Manager.Models;

namespace Access_Control_Manager.Interface
{
    public interface IStudent
    {

        Task<IEnumerable<Student>> GetAllStudents(long studentNumber);

        Task<Student> GetStudentByStudentNumber(long studentNumber);

        Task<Student> GetStudentSummary(long studentNumber);

        Task AddStudent(Student student);

        Task CheckInStudent(long studentNumber);

        Task<bool> CheckIfStudentExists(long studentNumber);

        Task CheckOutStudent(string qrcode);

        Task<Student> GetStudentByQrCode(string qrCode);

        Task<IEnumerable<StudentRecord>> GetStudentRecords(long studentNumber);

        Task Save();

    }
}
