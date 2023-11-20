﻿// <auto-generated />
using System;
using Access_Control_Manager.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Access_Control_Manager.Migrations
{
    [DbContext(typeof(AccessControlContext))]
    partial class AccessControlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Access_Control_Manager.Models.Activity", b =>
                {
                    b.Property<int>("key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("key"), 1L, 1);

                    b.Property<string>("ActivityDoneByStuff")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StaffActivityLogId")
                        .HasColumnType("int")
                        .HasColumnName("StaffActivityId");

                    b.HasKey("key");

                    b.HasIndex("StaffActivityLogId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Campus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Campuses");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Accessories")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Manufacture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("StudentNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("StudentNumber");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StudentNumber");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.DeviceRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Accessories")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Manufacture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentRecordId")
                        .HasColumnType("int")
                        .HasColumnName("StudentRecordId");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StudentRecordId");

                    b.ToTable("DeviceRecords");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CampusId")
                        .HasColumnType("int")
                        .HasColumnName("CampusId");

                    b.Property<string>("RoomID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CampusId");

                    b.ToTable("Labs");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.StaffActivityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CurrentlyLogInStaffNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLogoutTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    b.Property<bool>("isLoggedIn")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("StaffActivityLogs");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Student", b =>
                {
                    b.Property<long>("StudentNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Campus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("datetime2");

                    b.Property<bool>("CheckedOut")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateRegistered")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasDevice")
                        .HasColumnType("bit");

                    b.Property<string>("QRCodePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniqueGeneratedCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentNumber");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.StudentRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("AccessedCampus")
                        .HasColumnType("datetime2");

                    b.Property<string>("Campus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("StudentNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("StudentNumber");

                    b.Property<DateTime>("TimeIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeOut")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("StudentNumber");

                    b.ToTable("StudentRecords");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountStatus")
                        .HasColumnType("int");

                    b.Property<int>("CampusId")
                        .HasColumnType("int")
                        .HasColumnName("CampusId");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentStaffNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CampusId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Activity", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.StaffActivityLog", "StaffActivityLog")
                        .WithMany("Activities")
                        .HasForeignKey("StaffActivityLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StaffActivityLog");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Device", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.Student", "Student")
                        .WithMany("Devices")
                        .HasForeignKey("StudentNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.DeviceRecord", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.StudentRecord", "Student")
                        .WithMany("DeviceRecords")
                        .HasForeignKey("StudentRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Lab", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.Campus", "Campus")
                        .WithMany("Labs")
                        .HasForeignKey("CampusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campus");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.StaffActivityLog", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.User", "User")
                        .WithMany("ActivityLog")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.StudentRecord", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.User", b =>
                {
                    b.HasOne("Access_Control_Manager.Models.Campus", "Campus")
                        .WithMany("Users")
                        .HasForeignKey("CampusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campus");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Campus", b =>
                {
                    b.Navigation("Labs");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.StaffActivityLog", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.Student", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.StudentRecord", b =>
                {
                    b.Navigation("DeviceRecords");
                });

            modelBuilder.Entity("Access_Control_Manager.Models.User", b =>
                {
                    b.Navigation("ActivityLog");
                });
#pragma warning restore 612, 618
        }
    }
}