using Coursewise.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class ApplyJobDto : IValidatableObject
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "CV is required.")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Job id is required.")]
        public Guid JobId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = (ApplyJobDto)validationContext.ObjectInstance;
            
            var validation = model.File.CvValidation();
            foreach (var item in validation)
            {
                yield return item;
            }
        }
    }
}
