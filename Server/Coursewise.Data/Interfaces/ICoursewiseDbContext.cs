using Coursewise.Common.Models;
using Coursewise.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Interfaces
{
    public  interface ICoursewiseDbContext
    {
        DbSet<CoursewiseUser> Users { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Level> Levels { get; set; }
        DbSet<CourseLevel> CourseLevels { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Artifact> Artifacts { get; set; }
        DbSet<Industry> Industries { get; set; }
        DbSet<Skill> Skills { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<JobIndustry> JobIndustries { get; set; }
        DbSet<JobSkill> JobSkills { get; set; }
        DbSet<JobLocation> JobLocations { get; set; }
        DbSet<JobArtifact> JobArtifacts { get; set; }
        DbSet<CustomerJob> CustomerJobs { get; set; }
        DbSet<EmailTemplate> EmailTemplates { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<ResourceArtifact> ResourceArtifacts { get; set; }
        DbSet<Title> Titles { get; set; }
        DbSet<JobTitle> JobTitles { get; set; }
        DbSet<ServiceStatus> ServiceStatuses { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DatabaseFacade DatabaseObject { get; }
    }
}
