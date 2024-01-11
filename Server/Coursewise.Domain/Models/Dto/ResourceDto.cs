using Coursewise.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class ResourceDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } 
        [Required(ErrorMessage = "Preview text is required.")]
        [MaxLength(200,ErrorMessage = "Preview text has exceeded the maximum length.")]
        public string Preview { get; set; } 
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } 
        [Required(ErrorMessage = "Provide numbers of minutes.")]
        [Range(1,120,ErrorMessage = "Please enter a number, which should be the estimated length in minutes.")]
        public int Length { get; set; }
        public bool IsFeatured { get; set; }
        public int ResourceType { get; set; } = (int)Common.Models.ResourceType.IMAGE_RESOURCE;
        public string? VideoLink { get; set; }
        public bool SendNotification { get; set; } = false;

    }
    public class AddResourceDto: ResourceDto, IValidatableObject
    {
        public IEnumerable<IFormFile> Media { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = (AddResourceDto)validationContext.ObjectInstance;
            if (model.ResourceType==(int)Common.Models.ResourceType.IMAGE_RESOURCE && model.Media is null)
            {
                yield return new ValidationResult($"Image is required.");
            }
            else
            {
                var mediaValidation = model.Media.ResourceValidation();
                foreach (var item in mediaValidation)
                {
                    yield return item;
                }
            }            
        }
    }
    public class EditResourceDto : ResourceDto
    {
        public Guid Id { get; set; }
    }

    public class ResourceBasicDto
    {
        public string Title { get; set; } 
        public string Preview { get; set; }
        public int Length { get; set; }
        public bool IsFeatured { get; set; }
        public string Media { get; set; }
        public string Url { get; set; }
        public int NoOfClaps { get; set; }
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public int ResourceType { get; set; } 
        public string? VideoLink { get; set; }

    }
    public class ResourceDetailDto: ResourceBasicDto
    {
        public string Description { get; set; }
        public bool IsReady { get; set; }
        public new IEnumerable<ResourceMediaDto> Media { get; set; }
    }
    public class ResourceMediaDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ArtifactType { get; set; }
    }
    public class ResourceMediaUploadDto: IValidatableObject
    {
        [Required(ErrorMessage = "Resource id is required.")]
        public Guid ResourceId { get; set; }

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = (ResourceMediaUploadDto)validationContext.ObjectInstance;
            if (model.ResourceId == Guid.Empty)
            {
                yield return new ValidationResult($"Resource id is required.");
            }
            var media = new List<IFormFile>();
            media.Add(model.File);
            var validationResult = media.ResourceValidation();
            foreach (var item in validationResult)
            {
                yield return item;
            }
        }
    }
}
