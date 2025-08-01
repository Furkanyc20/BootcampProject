﻿// <auto-generated />
using System;
using BootcampProject.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BootcampProject.Repositories.Migrations
{
    [DbContext(typeof(BootcampDbContext))]
    [Migration("20250725213908_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ApplicationState")
                        .HasColumnType("int");

                    b.Property<Guid>("BootcampId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.HasIndex("BootcampId");

                    b.ToTable("Applications", (string)null);
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Blacklist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.ToTable("Blacklists", (string)null);
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Bootcamp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BootcampState")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InstructorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InstructorId");

                    b.ToTable("Bootcamps", (string)null);
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NationalityIdentity")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Applicant", b =>
                {
                    b.HasBaseType("BootcampProject.Entities.Concrete.User");

                    b.Property<string>("About")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.ToTable("Applicants", (string)null);
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Employee", b =>
                {
                    b.HasBaseType("BootcampProject.Entities.Concrete.User");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Instructor", b =>
                {
                    b.HasBaseType("BootcampProject.Entities.Concrete.User");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.ToTable("Instructors", (string)null);
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Application", b =>
                {
                    b.HasOne("BootcampProject.Entities.Concrete.Applicant", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BootcampProject.Entities.Concrete.Bootcamp", "Bootcamp")
                        .WithMany("Applications")
                        .HasForeignKey("BootcampId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Applicant");

                    b.Navigation("Bootcamp");
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Blacklist", b =>
                {
                    b.HasOne("BootcampProject.Entities.Concrete.Applicant", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Applicant");
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Bootcamp", b =>
                {
                    b.HasOne("BootcampProject.Entities.Concrete.Instructor", "Instructor")
                        .WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Applicant", b =>
                {
                    b.HasOne("BootcampProject.Entities.Concrete.User", null)
                        .WithOne()
                        .HasForeignKey("BootcampProject.Entities.Concrete.Applicant", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Employee", b =>
                {
                    b.HasOne("BootcampProject.Entities.Concrete.User", null)
                        .WithOne()
                        .HasForeignKey("BootcampProject.Entities.Concrete.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Instructor", b =>
                {
                    b.HasOne("BootcampProject.Entities.Concrete.User", null)
                        .WithOne()
                        .HasForeignKey("BootcampProject.Entities.Concrete.Instructor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BootcampProject.Entities.Concrete.Bootcamp", b =>
                {
                    b.Navigation("Applications");
                });
#pragma warning restore 612, 618
        }
    }
}
