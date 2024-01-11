using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Coursewise.Domain.Extensions
{
    public static class FormFileExtensions
    {
        public static IEnumerable<ValidationResult> CvValidation(this IFormFile file)
        {
            var fileTypes = new List<string>() { ".pdf", ".DOCX", ".docx", ".DOC", ".doc" };
            if (file is not null)
            {
                if (!fileTypes.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    yield return new ValidationResult($"Invalid file format. Only {String.Join(", ", fileTypes)} allowed");
                }
                if (file.Length > 5242880)
                {
                    yield return new ValidationResult($"File size is exceeding the limit");
                }
            }
        }
        public static IEnumerable<ValidationResult> ResourceValidation(this IEnumerable<IFormFile> files)
        {
            var fileTypes = new List<string>() { ".png", ".jpeg", ".jpg", ".PNG", ".JPEG", ".JPG" };
            foreach (var file in files)
            {
                if (!fileTypes.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    yield return new ValidationResult($"Invalid {file.FileName} file format. Only {String.Join(", ", fileTypes)} allowed");
                }
                if (file.Length > 5242880)
                {
                    yield return new ValidationResult($"{file.FileName} size is exceeding the limit");
                }
            }
        }
    }
}
