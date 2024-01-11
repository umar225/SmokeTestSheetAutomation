using Coursewise.Common.Models;
using Coursewise.Data.Entities;
using Coursewise.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data
{
    public class CoursewiseDbContext : IdentityDbContext<CoursewiseUser, IdentityRole, string>, ICoursewiseDbContext
    {
        public CoursewiseDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobIndustry> JobIndustries { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<JobArtifact> JobArtifacts { get; set; }
        public DbSet<CustomerJob> CustomerJobs { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceArtifact> ResourceArtifacts { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<ServiceStatus> ServiceStatuses { get; set; }

        
        public DatabaseFacade DatabaseObject
        {
            get
            {
                return Database;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CoursewiseUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(127));
            builder.Entity<CoursewiseUser>().ToTable("coursewise_users");
            builder.Entity<CoursewiseUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(127));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(127));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(127));
            builder.Entity<IdentityRole>(entity => entity.Property(x => x.Name).HasMaxLength(127));
            builder.Entity<CoursewiseUser>(entity => entity.Property(e => e.Email).HasMaxLength(127));
            builder.Entity<CoursewiseUser>(entity => entity.Property(e => e.UserName).HasMaxLength(127));
            builder.Entity<CoursewiseUser>(entity => entity.Property(e => e.NormalizedUserName).HasMaxLength(127));

        }
    }
}
