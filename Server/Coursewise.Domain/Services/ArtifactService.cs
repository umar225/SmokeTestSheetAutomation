using AutoMapper;
using Coursewise.AWS.Communication;
using Coursewise.Common.Exceptions;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services
{
    public class ArtifactService: GenericService<Data.Entities.Artifact, Artifact, int>, IArtifactService
    {
        private readonly IPersistenceService _persistenceService;
        private readonly IArtifactRepository _artifactRepository;
        private readonly IMapper _mapper;
        private readonly ICoursewiseLogger<ArtifactService> _logger;

        public ArtifactService(
            IArtifactRepository artifactRepository,
            IPersistenceService persistenceService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICoursewiseLogger<ArtifactService> logger) : base(mapper, artifactRepository, unitOfWork)
        {
            _persistenceService = persistenceService;
            _artifactRepository = artifactRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseModel> UploadArtifact(IFormFile file,string fileLoaction, bool? isFilePermissionPrivate=null)
        {
            var parameters = new Dictionary<string, object>();
            if (isFilePermissionPrivate.HasValue && isFilePermissionPrivate.Value)
            {
                parameters.Add("filePermission",Amazon.S3.S3CannedACL.Private);
            }
            fileLoaction = string.IsNullOrEmpty(fileLoaction) ? "" : $"{fileLoaction}/";
            Artifact uploadedFile;
            try
            {
                var fileName = $"{fileLoaction}{DateTime.Now.ToUniversalTime().Ticks}{file.FileName.Substring(file.FileName.LastIndexOf('.'))}";
                var fileArtifact = (AWS.Communication.Models.Artifact)await _persistenceService.WriteFile(fileName, file.OpenReadStream(), parameters);
                uploadedFile = _mapper.Map<Artifact>(fileArtifact);
                if (string.IsNullOrWhiteSpace(fileArtifact.Url))
                    throw new AwsFileUploadException();

                return BaseModel.Success(uploadedFile, 0, fileName);

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

        public async Task<BaseModel> DeleteArtifact(string fileName)
        {
            try
            {
                await _persistenceService.DeleteFile(fileName);
                return BaseModel.Success();

            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return BaseModel.Error($"Failed to delete the file {fileName}");
            }
        }
        public async Task<BaseModel> DeleteArtifacts(IEnumerable<string> fileNames)
        {
            List<BaseModel> result = new List<BaseModel>();
            foreach (var file in fileNames)
            {
                var response =await DeleteArtifact(file);
                result.Add(response);
            }
            return result.TrueForAll(r => r.success)?BaseModel.Success():BaseModel.Error("Issue in delete some files");
        }

        public string GetFileUrl(string fileName)
        {
            try
            {
                var url = _persistenceService.GetUploadPreSignedUrl(fileName);
                return url;
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return "";
            }
        }
        public async Task DeleteArtifactEntry(Data.Entities.Artifact artifact)
        {
            await _artifactRepository.DeleteAsync(artifact);
        }
    }
}
