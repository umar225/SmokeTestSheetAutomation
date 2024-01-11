﻿// <auto-generated />
using System;
using Coursewise.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Coursewise.Data.Migrations
{
    [DbContext(typeof(CoursewiseDbContext))]
    [Migration("20220707165702_Industry_Skill_Location_NoOfJobs")]
    partial class Industry_Skill_Location_NoOfJobs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Coursewise.Common.Models.CoursewiseUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("coursewise_users", (string)null);
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Artifact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("StorageLocation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.HasKey("Id");

                    b.ToTable("Artifacts");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Desription")
                        .HasColumnType("longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("ProviderName")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<double?>("ProviderPrice")
                        .HasColumnType("double");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.CourseLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("LevelId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LevelId");

                    b.ToTable("CourseLevels");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("LastName")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("WordpressBoardwiseUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Industry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("NoOfJob")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Industries");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Category")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Company")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("CompanyLink")
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("CompanyTagLine")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");

                    b.Property<DateTime?>("Expire")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("NoOfRole")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Url")
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobArtifact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ArtifactId")
                        .HasColumnType("int");

                    b.Property<Guid>("JobId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ArtifactId");

                    b.HasIndex("JobId");

                    b.ToTable("JobArtifacts");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobIndustry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IndustryId")
                        .HasColumnType("int");

                    b.Property<Guid>("JobId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Other")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("IndustryId");

                    b.HasIndex("JobId");

                    b.ToTable("JobIndustries");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("JobId")
                        .HasColumnType("char(36)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Other")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("LocationId");

                    b.ToTable("JobLocations");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("JobId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Other")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("SkillId");

                    b.ToTable("JobSkills");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Level", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("NoOfJob")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("NoOfJob")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("RoleId")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Course", b =>
                {
                    b.HasOne("Coursewise.Data.Entities.Category", "Category")
                        .WithMany("Courses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.CourseLevel", b =>
                {
                    b.HasOne("Coursewise.Data.Entities.Course", "Course")
                        .WithMany("CourseLevels")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Coursewise.Data.Entities.Level", "Level")
                        .WithMany("CourseLevels")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Level");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Customer", b =>
                {
                    b.HasOne("Coursewise.Common.Models.CoursewiseUser", "CoursewiseUser")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("CoursewiseUser");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobArtifact", b =>
                {
                    b.HasOne("Coursewise.Data.Entities.Artifact", "Artifact")
                        .WithMany()
                        .HasForeignKey("ArtifactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Coursewise.Data.Entities.Job", "Job")
                        .WithMany("JobArtifact")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artifact");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobIndustry", b =>
                {
                    b.HasOne("Coursewise.Data.Entities.Industry", "Industry")
                        .WithMany("JobIndustry")
                        .HasForeignKey("IndustryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Coursewise.Data.Entities.Job", "Job")
                        .WithMany("JobIndustry")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Industry");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobLocation", b =>
                {
                    b.HasOne("Coursewise.Data.Entities.Job", "Job")
                        .WithMany("JobLocations")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Coursewise.Data.Entities.Location", "Location")
                        .WithMany("JobLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.JobSkill", b =>
                {
                    b.HasOne("Coursewise.Data.Entities.Job", "Job")
                        .WithMany("JobSkills")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Coursewise.Data.Entities.Skill", "Skill")
                        .WithMany("JobSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Coursewise.Common.Models.CoursewiseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Coursewise.Common.Models.CoursewiseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Coursewise.Common.Models.CoursewiseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Coursewise.Common.Models.CoursewiseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Category", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Course", b =>
                {
                    b.Navigation("CourseLevels");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Industry", b =>
                {
                    b.Navigation("JobIndustry");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Job", b =>
                {
                    b.Navigation("JobArtifact");

                    b.Navigation("JobIndustry");

                    b.Navigation("JobLocations");

                    b.Navigation("JobSkills");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Level", b =>
                {
                    b.Navigation("CourseLevels");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Location", b =>
                {
                    b.Navigation("JobLocations");
                });

            modelBuilder.Entity("Coursewise.Data.Entities.Skill", b =>
                {
                    b.Navigation("JobSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
