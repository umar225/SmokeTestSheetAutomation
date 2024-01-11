using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class JobDto : IValidatableObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }
        public string CompanyTagLine { get; set; }
        [Required(ErrorMessage = "Picture is required.")]
        public IFormFile File { get; set; }
        [Required(ErrorMessage = "Company url is required.")]
        public string CompanyLink { get; set; }
        public int NoOfRole { get; set; }
        [Required(ErrorMessage = "Preview description is required.")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }
        public DateTime? Expire { get; set; }
        public IEnumerable<JobLocationDto> JobLocations { get; set; }
        public IEnumerable<JobSkillDto> JobSkills { get; set; }
        public IEnumerable<JobIndustryDto> JobIndustry { get; set; }
        public IEnumerable<JobTitleDto> JobTitles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = (JobDto)validationContext.ObjectInstance;
            var categories = new List<string>() { "External", "Exclusive" };
            if (!categories.Contains(model.Category))
            {
                yield return new ValidationResult($"Only this categoris {String.Join(", ", categories)} allow.");
            }
            if (!model.JobIndustry.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 industry.");
            }
            if (!model.JobLocations.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 location.");
            }
            if (!model.JobSkills.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 skill.");
            }
            if (!model.JobTitles.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 title.");
            }
            var picValidation = model.File.PictureValidation();
            foreach (var item in picValidation)
            {
                yield return item;
            }
        }
    }

    public class JobIndustryDto
    {
        public int IndustryId { get; set; }
    }
    public class JobLocationDto
    {
        public int LocationId { get; set; }
    }
    public class JobSkillDto
    {
        public int SkillId { get; set; }
    }
    public class JobTitleDto
    {
        public int TitleId { get; set; }
    }


    public class EditJobDto : IValidatableObject
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }
        public string CompanyTagLine { get; set; }
        [Required(ErrorMessage = "Company url is required.")]
        public string CompanyLink { get; set; }
        public int NoOfRole { get; set; }
        [Required(ErrorMessage = "Preview description is required.")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }
        public DateTime? Expire { get; set; }
        public IEnumerable<JobLocationDto> JobLocations { get; set; }
        public IEnumerable<JobSkillDto> JobSkills { get; set; }
        public IEnumerable<JobIndustryDto> JobIndustry { get; set; }
        public IEnumerable<JobTitleDto> JobTitles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = (EditJobDto)validationContext.ObjectInstance;
            var categories = new List<string>() { "External", "Exclusive" };
            if (!categories.Contains(model.Category))
            {
                yield return new ValidationResult($"Only this categoris {String.Join(", ", categories)} allow.");
            }
            if (!model.JobIndustry.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 industry.");
            }
            if (!model.JobLocations.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 location.");
            }
            if (!model.JobSkills.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 skill.");
            }
            if (!model.JobTitles.Any())
            {
                yield return new ValidationResult($"Provide atleast 1 title.");
            }
        }
    }

    public class JobPictureUploadDto : IValidatableObject
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Picture is required.")]
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = (JobPictureUploadDto)validationContext.ObjectInstance;

            return model.File.PictureValidation();
        }
    }

    public static class JobCommonValidation
    {
        public static IEnumerable<ValidationResult> PictureValidation(this IFormFile file)
        {
            var fileTypes = new List<string>() { ".png", ".jpeg", ".jpg" };
            if (file is not null)
            {
                if (!fileTypes.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    yield return new ValidationResult($"Only this image types {String.Join(", ", fileTypes)} allow.");
                }
                if (file.Length > 5242880)
                {
                    yield return new ValidationResult($"Upto 5 mb images allowed");
                }
            }
        }
    }
}
