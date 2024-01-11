using AutoMapper;
using Coursewise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Mapping
{
    public static class ModelMapper
    {
        public static MapperConfiguration Configure(MapperConfigurationExpression? cfg = null)
        {
            if (cfg == null)
                cfg = new MapperConfigurationExpression();

            cfg.CreateMap<Category, Data.Entities.Category>();
            cfg.CreateMap<Data.Entities.Category, Category>();
            cfg.CreateMap<Course, Data.Entities.Course>();
            cfg.CreateMap<Data.Entities.Course, Course>();
            cfg.CreateMap<Level, Data.Entities.Level>();
            cfg.CreateMap<Data.Entities.Level, Level>();
            cfg.CreateMap<CourseLevel, Data.Entities.CourseLevel>();
            cfg.CreateMap<Data.Entities.CourseLevel, CourseLevel>();

            cfg.CreateMap<Customer, Data.Entities.Customer>();
            cfg.CreateMap<Data.Entities.Customer, Customer>();

            cfg.CreateMap<Artifact, Data.Entities.Artifact>();
            cfg.CreateMap<Data.Entities.Artifact, Artifact>();
            cfg.CreateMap<Industry, Data.Entities.Industry>();
            cfg.CreateMap<Data.Entities.Industry, Industry>();
            cfg.CreateMap<Location, Data.Entities.Location>();
            cfg.CreateMap<Data.Entities.Location, Location>();
            cfg.CreateMap<Skill, Data.Entities.Skill>();
            cfg.CreateMap<Data.Entities.Skill, Skill>();
            cfg.CreateMap<Title, Data.Entities.Title>();
            cfg.CreateMap<Data.Entities.Title, Title>();
            cfg.CreateMap<JobIndustry, Data.Entities.JobIndustry>();
            cfg.CreateMap<Data.Entities.JobIndustry, JobIndustry>();
            cfg.CreateMap<JobLocation, Data.Entities.JobLocation>();
            cfg.CreateMap<Data.Entities.JobLocation, JobLocation>();
            cfg.CreateMap<JobSkill, Data.Entities.JobSkill>();
            cfg.CreateMap<Data.Entities.JobSkill, JobSkill>();
            cfg.CreateMap<JobTitle, Data.Entities.JobTitle>();
            cfg.CreateMap<Data.Entities.JobTitle, JobTitle>();
            cfg.CreateMap<JobArtifact, Data.Entities.JobArtifact>();
            cfg.CreateMap<Data.Entities.JobArtifact, JobArtifact>();
            cfg.CreateMap<Job, Data.Entities.Job>();
            cfg.CreateMap<Data.Entities.Job, Job>();

            cfg.CreateMap<Data.Entities.Course, Models.CourseDto>().
                ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Category.Name)).
                ForMember(dest => dest.Urls, opt => opt.MapFrom(src => src.Url));
            cfg.CreateMap<Models.CourseDto, Data.Entities.Course>();

            cfg.CreateMap<Models.Dto.JobIndustryDto, JobIndustry>();
            cfg.CreateMap<Models.Dto.JobLocationDto, JobLocation>();
            cfg.CreateMap<Models.Dto.JobSkillDto, JobSkill>();
            cfg.CreateMap<Models.Dto.JobTitleDto, JobTitle>();
            cfg.CreateMap<Models.Dto.JobDto, Job>();
            cfg.CreateMap<Models.Dto.EditJobDto, Job>();
            cfg.CreateMap<AWS.Communication.Models.Artifact, Artifact>();
            cfg.CreateMap<CustomerJob, Data.Entities.CustomerJob>();
            cfg.CreateMap<Data.Entities.CustomerJob, CustomerJob>();
            cfg.CreateMap<EmailTemplate, Data.Entities.EmailTemplate>();
            cfg.CreateMap<Data.Entities.EmailTemplate, EmailTemplate>();
            cfg.CreateMap<Resource, Data.Entities.Resource>();
            cfg.CreateMap<Data.Entities.Resource, Resource>();
            cfg.CreateMap<ResourceArtifact, Data.Entities.ResourceArtifact>();
            cfg.CreateMap<Data.Entities.ResourceArtifact, ResourceArtifact>();

            #region DTO
            cfg.CreateMap<Skill, Models.Dto.EntityDropdown>().
                ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<Location, Models.Dto.EntityDropdown>().
                ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<Industry, Models.Dto.EntityDropdown>().
                ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<Title, Models.Dto.EntityDropdown>().
                ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.TitleId));
            cfg.CreateMap<Models.Dto.ApplyJobDto, CustomerJob>();
            cfg.CreateMap<Models.Dto.AddResourceDto, Resource>();
            #endregion
            var config = new MapperConfiguration(cfg);
            return config;
        }
    }
}
