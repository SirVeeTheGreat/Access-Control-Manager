using Access_Control_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Access_Control_Manager.Database
{
    public class AccessControlContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Student>Students { get; set; }

        public DbSet<Campus> Campuses { get; set; } 

        public DbSet<DeviceRecord> DeviceRecords { get; set; }

        public DbSet<StudentRecord> StudentRecords { get; set; }    

        public DbSet<Lab> Labs { get; set; }    

        public DbSet<Device> Devices { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<StaffActivityLog> StaffActivityLogs { get; set; } 

        public AccessControlContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            modelBuilder.Entity<Lab>()
             .HasOne<Campus>(a => a.Campus)
             .WithMany(z => z.Labs)
             .HasForeignKey(y => y.CampusId);

            modelBuilder.Entity<StaffActivityLog>()
                .HasOne<User>(a => a.User)
                .WithMany(z => z.ActivityLog)
                .HasForeignKey(y => y.UserId);


            modelBuilder.Entity<Activity>()
            .HasOne<StaffActivityLog>(a => a.StaffActivityLog)
            .WithMany(z => z.Activities)
            .HasForeignKey(y => y.StaffActivityLogId);

            modelBuilder.Entity<Device>()
                .HasOne<Student>(z => z.Student)
                .WithMany(a => a.Devices)
                .HasForeignKey(y => y.StudentNumber);

            modelBuilder.Entity<DeviceRecord>()
                .HasOne<StudentRecord>(a => a.Student )
                .WithMany(y => y.DeviceRecords)
                .HasForeignKey(a => a.StudentRecordId);

            modelBuilder.Entity<User>()
                .HasOne<Campus>(s => s.Campus)
                .WithMany(c => c.Users)
                .HasForeignKey(s => s.CampusId);

        }



    }
}
