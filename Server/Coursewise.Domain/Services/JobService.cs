using AutoMapper;
using Coursewise.AWS.Communication;
using Coursewise.Common.Exceptions;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Models.Dto;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class JobService : GenericService<Data.Entities.Job, Job, Guid>, IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IIndustryService _industryService;
        private readonly ISkillService _skillService;
        private readonly ILocationService _locationService;
        private readonly ITitleService _titleService;
        private readonly IArtifactService _artifactService;
        private readonly IPersistenceService _persistenceService;
        private readonly ICustomerService _customerService;
        private readonly ICoursewiseLogger<JobService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public JobService(
            IJobServiceFacade jobServiceFacade,
            IUtilityFacade utilityFacade,
            ICoursewiseLogger<JobService> logger
            ) : base(utilityFacade.Mapper, jobServiceFacade.JobRepository, utilityFacade.UnitOfWork)
        {
            _jobRepository = jobServiceFacade.JobRepository;
            _industryService = jobServiceFacade.JobSelectionFacade.IndustryService;
            _skillService = jobServiceFacade.JobSelectionFacade.SkillService;
            _locationService = jobServiceFacade.JobSelectionFacade.LocationService;
            _titleService = jobServiceFacade.JobSelectionFacade.TitleService;
            _artifactService = jobServiceFacade.ArtifactService;
            _customerService = jobServiceFacade._customerService;
            _persistenceService = jobServiceFacade.AWSServiceFacade.PersistenceService;
            _mapper = utilityFacade.Mapper;
            _logger = logger;
            _unitOfWork = utilityFacade.UnitOfWork;
        }

        public async Task<BaseModel> Add(Models.Dto.JobDto businessEntity)
        {
            Artifact? artifact = null;
            string? fileName = string.Empty;
            try
            {
                var job = _mapper.Map<Job>(businessEntity);
                var validResponse = await ValidateJob(job);
                if (!validResponse.success)
                {
                    return validResponse;
                }
                var fileResponse = await UploadArtifact(businessEntity.File);
                if (!fileResponse.success)
                {
                    return fileResponse;
                }
                if (fileResponse.success && fileResponse.data != null)
                {
                    artifact = (Artifact)fileResponse.data;
                    fileName = fileResponse.message;
                }
                job.Created = DateTime.Now.ToUniversalTime();
                job.IsClosed = false;
                job.IsVisible = true;
                if (artifact is not null)
                {
                    job.JobArtifact = new List<JobArtifact>() { new JobArtifact() { Artifact = artifact, Type = "Orignal_Logo" } };
                }
                var jobDataEntity = _mapper.Map<Data.Entities.Job>(job);
                await _jobRepository.AddAsync(jobDataEntity);
                await _unitOfWork.SaveChangesAsync();
                return BaseModel.Success(jobDataEntity.Id);
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                if (!string.IsNullOrEmpty(fileName))
                    await _persistenceService.DeleteFile(fileName, true);
                return BaseModel.Error("Some issue in adding job");
            }

        }

        public async Task<BaseModel> Update(Models.Dto.EditJobDto businessEntity)
        {
            var job = _mapper.Map<Job>(businessEntity);
            var existingJob = await _jobRepository.FirstOrDefaultAsync(j => j.Id == job.Id, "JobSkills,JobIndustry,JobLocations,JobTitles");
            if (existingJob is null)
            {
                return BaseModel.Error("Job does not exist");
            }
            var validResponse = await ValidateJob(job);
            if (!validResponse.success)
            {
                return validResponse;
            }
            existingJob.Category = job.Category;
            existingJob.Name = job.Name;
            existingJob.Description = job.Description;
            existingJob.CompanyTagLine = job.CompanyTagLine;
            existingJob.NoOfRole = job.NoOfRole;
            existingJob.Company = job.Company;
            existingJob.CompanyLink = job.CompanyLink;
            existingJob.ShortDescription = job.ShortDescription;
            existingJob.JobLocations = _mapper.Map<List<Data.Entities.JobLocation>>(job.JobLocations);
            existingJob.JobIndustry = _mapper.Map<List<Data.Entities.JobIndustry>>(job.JobIndustry);
            existingJob.JobSkills = _mapper.Map<List<Data.Entities.JobSkill>>(job.JobSkills);
            existingJob.JobTitles = _mapper.Map<List<Data.Entities.JobTitle>>(job.JobTitles);
            await _jobRepository.UpdateAsync(existingJob);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success(existingJob.Id);
        }
        public async Task<BaseModel> GetAll(Guid customerId)
        {
            var jobs = await _jobRepository.Get()
                            .Include(j => j.JobArtifact)
                            .Include(j => j.CustomerJobs)
                            .OrderByDescending(j => j.Created)
                            .Select(
                                    j => new Models.Dto.JobBasicDto
                                    {
                                        Id = j.Id,
                                        Name = j.Name,
                                        Company = j.Company,
                                        Category = j.Category,
                                        NoOfRole = j.NoOfRole,
                                        PreviewText = j.ShortDescription,
                                        IsAlreadyApplied = j.CustomerJobs.Any(c => c.CustomerId == customerId),
                                        Created = j.Created,
                                        CompanyLogo = (j.JobArtifact.Any() && j.JobArtifact.FirstOrDefault()!.Artifact != null) ? j.JobArtifact.FirstOrDefault()!.Artifact.Url : "",
                                    }
                        ).ToListAsync();
            return BaseModel.Success(jobs);
        }

        public async new Task<BaseModel> Get(Guid id)
        {
            var job = await _jobRepository.Get()
                .Include(j => j.JobLocations)
                .Include(j => j.JobSkills)
                .Include(j => j.JobIndustry)
                .Include(j => j.JobTitles)
                .Include(j => j.JobArtifact)
                .Select(j => new
                {
                    j.Id,
                    j.Name,
                    j.Company,
                    j.NoOfRole,
                    j.Category,
                    j.CompanyLink,
                    j.Created,
                    j.Description,
                    j.ShortDescription,
                    JobArtifacts = j.JobArtifact.Select(ja => new { url = ja.Artifact.Url }),
                    JobSkills = j.JobSkills.Select(js => new {
                        value = js.SkillId
                    }),
                    JobLocations = j.JobLocations.Select(jl => new { value = jl.LocationId }),
                    JobIndustry = j.JobIndustry.Select(ji => new { value = ji.IndustryId }),
                    JobTitle = j.JobTitles.Select(ji => new { value = ji.TitleId }),
                }).FirstOrDefaultAsync(j => j.Id == id);
            if (job is null)
            {
                return BaseModel.Error("No job found");
            }
            return BaseModel.Success(job);
        }

        public async Task<BaseModel> GetByCustomer(Guid id,Guid customerId, string? userId)
        {
            var isPaidUser =  await _customerService.isSubscribed(userId);
            if (!isPaidUser)
            {
                return BaseModel.Error("");
            }
            var job = await GetJobsDetails(customerId, isPaidUser).FirstOrDefaultAsync(j => j.Id == id);
            if (job is null)
            {
                return BaseModel.Error("No job found");
            }
            return BaseModel.Success(job);
        }
        public async Task<IList<JobDetaiDto>> GetLatest(Guid customerId,string? userId)
        {
            var isPaidUser = await _customerService.isSubscribed(userId);

            return await GetJobsDetails(customerId,isPaidUser).OrderByDescending(a => a.Created).Take(4).ToListAsync();

        }
        private IQueryable<JobDetaiDto> GetJobsDetails(Guid customerId,bool isPaidUser)
        {
            return _jobRepository.Get()
                            .Include(j => j.JobLocations)

                            .Include(j => j.JobSkills)

                            .Include(j => j.JobIndustry)
                            .Include(j => j.JobTitles)
                            .Include(j => j.JobArtifact)
                            .ThenInclude(j=>j.Artifact)
                            .Include(j => j.CustomerJobs)
                            .Select(
                                    j => new Models.Dto.JobDetaiDto
                                    {
                                        Id = j.Id,
                                        Name = j.Name,
                                        Company = isPaidUser ? j.Company : "",
                                        Category = j.Category,
                                        NoOfRole = j.NoOfRole,
                                        PreviewText = j.ShortDescription,
                                        Description = j.Description!,
                                        CompanyLogo = getCompanyLogo(j,isPaidUser),
                                        CompanyLink = j.CompanyLink,
                                        Created = j.Created,
                                        IsAlreadyApplied = j.CustomerJobs.Any(c => c.CustomerId == customerId),
                                        Locations = j.JobLocations.Select(x => x.Location.Name),
                                        Skills = j.JobSkills.Select(x => x.Skill.Name),
                                        Industries = j.JobIndustry.Select(x => x.Industry.Name),
                                        Titles = j.JobTitles.Select(x => x.Title.Name)

                                    });
        }
        private static string? getCompanyLogo(Data.Entities.Job j, bool isPaidUser)
        {
            if (j.JobArtifact.Any())
            {
                if (isPaidUser&&j.JobArtifact.FirstOrDefault()!.Artifact != null)
                {
                    return j.JobArtifact.FirstOrDefault()!.Artifact.Url;
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        public async Task<BaseModel> GetForCustomer(string? userId)
        {
            var isPaidUser =  await _customerService.isSubscribed(userId);
            if (!isPaidUser)
            {
                return BaseModel.Error("");
            }
            var jobs = await _jobRepository.Get()
                            .Include(j => j.JobArtifact)
                            .OrderByDescending(j => j.Created)
                            .Select(
                                    j => new Models.Dto.JobBasicDto
                                    {
                                        Id = j.Id,
                                        Name = j.Name,
                                        Company = j.Company,
                                        Category = j.Category,
                                        NoOfRole = j.NoOfRole,
                                        CompanyLogo = (j.JobArtifact.Any() && j.JobArtifact.FirstOrDefault()!.Artifact != null) ? j.JobArtifact.FirstOrDefault()!.Artifact.Url : "",
                                    }
                        ).ToListAsync();
            return BaseModel.Success(jobs);
        }

        public async Task<BaseModel> UploadPicture(Models.Dto.JobPictureUploadDto model)
        {
            Data.Entities.Artifact? artifact = null;
            string? fileName = string.Empty;
            try
            {
                var job = await _jobRepository.Get().AsNoTracking()
                    .Include(j => j.JobArtifact)
                    .ThenInclude(j => j.Artifact)
                    .FirstOrDefaultAsync(j => j.Id == model.Id);
                if (job == null)
                {
                    return BaseModel.Error("Job does not exist");
                }
                var fileResponse = await UploadArtifact(model.File);
                if (!fileResponse.success)
                {
                    return fileResponse;
                }
                if (fileResponse.success && fileResponse.data != null)
                {
                    artifact = _mapper.Map<Data.Entities.Artifact>(fileResponse.data);
                    fileName = fileResponse.message;
                }
                var existingArtifact = job.JobArtifact.FirstOrDefault(ja => ja.Type == "Orignal_Logo")!.Artifact;
                await _persistenceService.DeleteFile(existingArtifact.StorageLocation);
                await _artifactService.Delete(_mapper.Map<Artifact>(existingArtifact));
                job = await _jobRepository
                    .FirstOrDefaultAsync(j => j.Id == model.Id);
                job.JobArtifact = new List<Data.Entities.JobArtifact>() { new Data.Entities.JobArtifact() { Artifact = artifact!, Type = "Orignal_Logo" } };
                await _jobRepository.UpdateAsync(job);
                await _unitOfWork.SaveChangesAsync();
                return BaseModel.Success(new { jobId = model.Id, url = artifact!.Url });
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                if (!string.IsNullOrEmpty(fileName))
                    await _persistenceService.DeleteFile(fileName, true);
                return BaseModel.Error("Some issue in uploading picture");
            }

        }

        public async Task<BaseModel> Delete(Guid id)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == id, "JobSkills,JobLocations,JobIndustry,JobTitles");
            if (job == null)
            {
                return BaseModel.Error("No job found");
            }
            await _jobRepository.DeleteAsync(job);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }

        public async Task<BaseModel> GetJobsCounts()
        {
            var industries = await _industryService.Get();
            var skills = await _skillService.Get();
            var locations = await _locationService.Get();
            var titles = await _titleService.Get();
            var response = new
            {
                sort = new string[] { "External", "Exclusive", "All" },
                industries = industries.OrderByDescending(i => i.NoOfJob),
                skills = skills.OrderByDescending(i => i.NoOfJob),
                locations = locations.OrderByDescending(i => i.NoOfJob),
                titles = titles.OrderByDescending(i => i.NoOfJob)
            };
            return BaseModel.Success(response);
        }

        public async Task<BaseModel> CalculateJobsCounts()
        {
            var industries = await _industryService.GetCounts();
            var skills = await _skillService.GetCounts();
            var locations = await _locationService.GetCounts();
            var titles = await _titleService.GetCounts();
            var response = new
            {
                sort = new string[] { "External", "Exclusive", "All" },
                industries = industries,
                skills = skills,
                locations = locations,
                titles = titles
            };
            return BaseModel.Success(response);
        }
        public async Task<BaseModel> UpdateJobsCounts()
        {
            var skills = await _skillService.UpdateCounts();
            var locations = await _locationService.UpdateCounts();
            var industries = await _industryService.UpdateCounts();
            var titles = await _titleService.UpdateCounts();
            return (skills.success && locations.success && industries.success && titles.success) ? BaseModel.Success() : BaseModel.Error("Some issue in update counts");
        }
        private static IQueryable<Data.Entities.Job> filterJobQuery(Models.JobFilters filters, IQueryable<Data.Entities.Job> jobQuery)
        {
            filters.SearchString = filters.SearchString.ToLower();
            if (filters.Locations.Any())
            {
                jobQuery = jobQuery.Where(j => j.JobLocations.Any(x => filters.Locations.Contains(x.LocationId)));
            }
            if (filters.Industries.Any())
            {
                jobQuery = jobQuery.Where(j => j.JobIndustry.Any(x => filters.Industries.Contains(x.IndustryId)));

            }
            if (filters.Skills.Any())
            {
                jobQuery = jobQuery.Where(j => j.JobSkills.Any(x => filters.Skills.Contains(x.SkillId)));

            }
            if (filters.Titles is not null && filters.Titles.Any())
            {
                jobQuery = jobQuery.Where(j => j.JobTitles.Any(x => filters.Titles.Contains(x.TitleId)));

            }
            if (!string.IsNullOrEmpty(filters.SearchString))
            {
                var searchQuery=filters.SearchString.Trim().ToLower();
                jobQuery = jobQuery.Where(j =>(j.Company!=null&&j.Company.ToLower().Contains(searchQuery))
                ||(j.ShortDescription!=null &&j.ShortDescription.ToLower().Contains(searchQuery))
                ||j.JobTitles.Any(x => x.Title.Name.ToLower().Contains(searchQuery))
                || j.JobIndustry.Any(x => x.Industry.Name.ToLower().Contains(searchQuery))
                || j.JobSkills.Any(x => x.Skill.Name.ToLower().Contains(searchQuery))
                || j.JobLocations.Any(x => x.Location.Name.ToLower().Contains(searchQuery)));

            }
            return jobQuery;
        }
        public async Task<BaseModel> Filter(Models.JobFilters filters,Guid customerId,string? userId)
        {
            var isPaidUser =  await _customerService.isSubscribed(userId);
           
            var isFirstTime = false;
            DateTime lastDate;
            if (filters.LastDate is null)
            {
                lastDate = DateTime.Now.ToUniversalTime();
                isFirstTime = true;
            }
            else
            {
                lastDate = filters.LastDate.Value;
            }
            filters.Sort = (String.IsNullOrEmpty(filters.Sort)) ? "Exclusive" : filters.Sort;
            System.Linq.Expressions.Expression<Func<Data.Entities.Job, bool>> query = j => j.Created < lastDate && j.Category == filters.Sort;
            if (filters.Sort == "All")
            {
                query = j => j.Created < lastDate;
            }
            var jobQuery = _jobRepository.Get().Where(query)
                .Include(j => j.JobArtifact)
                .Include(j=>j.CustomerJobs)
                .Include(j => j.JobLocations)
                    .ThenInclude(j => j.Location)
                .Include(j => j.JobSkills)
                    .ThenInclude(j => j.Skill)
                .Include(j => j.JobIndustry)
                    .ThenInclude(j => j.Industry)
                .Include(j => j.JobTitles)
                    .ThenInclude(j => j.Title)
                .Include(j => j.JobArtifact).OrderByDescending(b => b.Created)
            .ThenBy(b => b.Id).AsQueryable();
            jobQuery= filterJobQuery(filters,jobQuery);


            var jobs = await jobQuery
                .Select(
                                    j => new Models.Dto.JobCustomerDto
                                    {
                                        Id = j.Id,
                                        Name = j.Name,
                                        Company =isPaidUser? j.Company:"",
                                        Category = j.Category,
                                        NoOfRole = j.NoOfRole,
                                        CreatedAt = j.Created,
                                        PreviewText = j.ShortDescription,
                                        CompanyLogo =(isPaidUser&&j.JobArtifact.Any() && j.JobArtifact.FirstOrDefault()!.Artifact != null) ? j.JobArtifact.FirstOrDefault()!.Artifact.Url : "",
                                        Skills = j.JobSkills.Select(s => s.Skill.Name),
                                        IsAlreadyApplied = j.CustomerJobs.Any(c => c.CustomerId == customerId),
                                        Industries = j.JobIndustry.Select(s => s.Industry.Name),
                                        Locations = j.JobLocations.Select(s => s.Location.Name),
                                        Titles = j.JobTitles.Select(s => s.Title.Name)
                                    }).Take(30)
                .ToListAsync();
            int? totalRolesCount = null;
            if (isFirstTime)
            {
                totalRolesCount = await jobQuery.SumAsync(x=>x.NoOfRole);
            }
            var lastJob = jobs.LastOrDefault();
            DateTime? dateTime = null;
            dateTime = (lastJob is null)? dateTime : lastJob.CreatedAt;
            
            return BaseModel.Success(new { noOfJobs = jobs.Count, totalRolesCount = totalRolesCount, dateTime = dateTime, jobs  }) ;
        }

        public async Task<bool> IsJobExist(Guid jobId)
        {
            var result = await _jobRepository.IsJobExist(jobId);
            return result;
        }
        public async Task<Job> GetJobSpecifDetail(Guid id)
        {
            var dataJob = await _jobRepository.Get().Where(x => x.Id == id).
                Include(x=> x.JobTitles).
                    ThenInclude(x=>x.Title).
                Select(x => new Data.Entities.Job
                {
                    Company = x.Company,
                    JobTitles = x.JobTitles.Select(j=>new Data.Entities.JobTitle { Title = j.Title}).ToList(),
                }).FirstOrDefaultAsync();
            var job = _mapper.Map<Job>(dataJob);
            if (job == null)
            {
                job = new Job();
            }
            return job;
        }
        public async Task<BaseModel> ValidateJob(Job job)
        {
            var validIndustries = await _industryService.ValidIndustries(job.JobIndustry.Select(x => x.IndustryId).ToList());
            if (!validIndustries)
            {
                return BaseModel.Error("Some of the industries are not valid");
            }
            var validLocations = await _locationService.ValidLocations(job.JobLocations.Select(x => x.LocationId).ToList());
            if (!validLocations)
            {
                return BaseModel.Error("Some of the locations are not valid");
            }
            var validSkills = await _skillService.ValidSkills(job.JobSkills.Select(x => x.SkillId).ToList());
            if (!validSkills)
            {
                return BaseModel.Error("Some of the skills are not valid");
            }
            var validTitles = await _titleService.ValidSkills(job.JobTitles.Select(x => x.TitleId).ToList());
            if (!validTitles)
            {
                return BaseModel.Error("Some of the titles are not valid");
            }
            return BaseModel.Success();
        }

        public async Task<BaseModel> UploadArtifact(IFormFile? file)
        {
            if (file is null)

            {
                return BaseModel.Success();
            }
            Artifact uploadedFile;
            try
            {
                var fileName = $"company/logo/{DateTime.Now.ToUniversalTime().Ticks.ToString()}{file.FileName.Substring(file.FileName.LastIndexOf('.'))}";
                var fileArtifact = (AWS.Communication.Models.Artifact)await _persistenceService.WriteFile(fileName, file.OpenReadStream());
                uploadedFile = _mapper.Map<Artifact>(fileArtifact);
                if (string.IsNullOrWhiteSpace(fileArtifact.Url))
                    throw new AwsFileUploadException();

               return BaseModel.Success(uploadedFile,0,fileName);

            }
            catch (AwsFileUploadException ex)
            {
                _logger.Exception(ex);
                return BaseModel.Error("Failed to upload file, Please try again.");
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return BaseModel.Error("Failed to upload file, Please try again.");
            }
        }
    }
}
