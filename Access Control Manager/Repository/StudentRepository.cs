using Access_Control_Manager.Database;
using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Access_Control_Manager.Repository
{
    public class StudentRepository : IStudent
    {
        private readonly AccessControlContext _context;

        public StudentRepository(AccessControlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudents(long studentNumber)
        {
            var students = await _context.Students.ToListAsync();
            string searchString = studentNumber.ToString();
    
            if (!String.IsNullOrEmpty(searchString))
            {
             
                var filteredStudents = students.Where(s => s.StudentNumber.ToString().Contains(searchString));
                return filteredStudents;
            }

            return students;

        }


        public async Task<Student> GetStudentByStudentNumber(long studentNumber)
        {
            return await _context.Students.FirstOrDefaultAsync(a => a.StudentNumber == studentNumber);
        }

        public async Task<Student> GetStudentSummary(long studentNumber)
        {
            return await _context.Students
                .Include(a => a.Devices)
                .FirstOrDefaultAsync(z => z.StudentNumber == studentNumber);
        }

        public async Task AddStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await Save();
        }

        public async Task<bool> CheckIfStudentExists(long studentNumber)
        {
            bool flag = false;

            var student = await _context.Students.FirstOrDefaultAsync(a => a.StudentNumber == studentNumber);

            if (student != null)
            {
                return flag = true; 
            }

            return flag;

        }

        public async Task CheckInStudent(long studentNumber)
        {
            var student = await _context.Students.FirstOrDefaultAsync(a => a.StudentNumber == studentNumber);
            student.CheckIn = DateTime.Now;
            student.CheckedOut = false;
            await Save();

        }

        public async Task CheckOutStudent(string qrcode)
        {
            var student = await _context.Students.Include(a => a.Devices).FirstOrDefaultAsync(a => a.UniqueGeneratedCode == qrcode);

            student.CheckOut = DateTime.Now;
            student.CheckedOut = true;
            student.UniqueGeneratedCode = null;

            StudentRecord studentRecord = new StudentRecord
            {
                StudentNumber = student.StudentNumber,
                Campus = student.Campus,
                AccessedCampus = student.CheckIn,
                TimeIn = student.CheckIn,
                TimeOut = student.CheckOut,
            };

            await _context.StudentRecords.AddAsync(studentRecord);
            await Save();
            var studRecord = await _context.StudentRecords.Select(a => new
            {
                a.StudentNumber,
                a.Id,
            }).FirstOrDefaultAsync(a => a.StudentNumber == student.StudentNumber);

            if (student.Devices.Any())
            {
                foreach (var item in student.Devices)
                {
                    DeviceRecord record = new DeviceRecord
                    {
                        StudentRecordId = studRecord.Id,
                        Model = item.Model?? "None",
                        Manufacture = item.Manufacture ?? "None",
                        SerialNumber = item.SerialNumber ?? "None",
                        Accessories = item.Accessories ?? "None",
                        Created = DateTime.Now,
                        Type = item.Type ?? "None",
                    };

                    await _context.DeviceRecords.AddAsync(record);
                    _context.Devices.Remove(item);
                }
            }

            await Save();    
        }

        public async Task<Student> GetStudentByQrCode(string qrCode)
        {
            return await _context.Students
                .Include(y => y.Devices)
                .FirstOrDefaultAsync(a => a.UniqueGeneratedCode == qrCode);
        }

        public async Task<IEnumerable<StudentRecord>> GetStudentRecords(long studentNumber)
        {
            return await _context.StudentRecords
                .Include(a => a.DeviceRecords)
                .Where(a => a.StudentNumber == studentNumber).ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


    }
}
