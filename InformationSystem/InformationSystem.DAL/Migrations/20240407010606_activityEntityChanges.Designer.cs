﻿// <auto-generated />
using System;
using InformationSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InformationSystem.DAL.Migrations
{
    [DbContext(typeof(InformationSystemDbContext))]
    [Migration("20240407010606_activityEntityChanges")]
    partial class activityEntityChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("CourseEntityStudentEntity", b =>
                {
                    b.Property<Guid>("CoursesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentsId")
                        .HasColumnType("TEXT");

                    b.HasKey("CoursesId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("StudentCourse", (string)null);
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.ActivityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ActivityType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EvaluationId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoomType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("EvaluationId")
                        .IsUnique();

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.CourseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT");

                    b.Property<int>("Credits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxStudents")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.EvaluationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Evaluations");
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.StudentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("StartYear")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CourseEntityStudentEntity", b =>
                {
                    b.HasOne("InformationSystem.DAL.Entities.CourseEntity", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InformationSystem.DAL.Entities.StudentEntity", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.ActivityEntity", b =>
                {
                    b.HasOne("InformationSystem.DAL.Entities.CourseEntity", "Course")
                        .WithMany("Activities")
                        .HasForeignKey("CourseId");

                    b.HasOne("InformationSystem.DAL.Entities.EvaluationEntity", "Evaluation")
                        .WithOne("Activity")
                        .HasForeignKey("InformationSystem.DAL.Entities.ActivityEntity", "EvaluationId");

                    b.Navigation("Course");

                    b.Navigation("Evaluation");
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.EvaluationEntity", b =>
                {
                    b.HasOne("InformationSystem.DAL.Entities.StudentEntity", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.CourseEntity", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("InformationSystem.DAL.Entities.EvaluationEntity", b =>
                {
                    b.Navigation("Activity")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
