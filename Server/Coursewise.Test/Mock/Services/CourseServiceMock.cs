using AutoMapper;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Services;
using Coursewise.Logging;
using Coursewise.Test.Models;
using GenFu;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock.Services
{
    public interface ICourseServiceMock 
    {
        CourseService SetupSuccessfullCourseServiceMock(CourseModel courseModel);
    }
    public class CourseServiceMock: ICourseServiceMock
    {
        private readonly IAllMocks _allMocks;
        private readonly Mock<ICourseService> _courseServiceMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        //private readonly Mock<IMapper> _mapperMock;
        private readonly IMapper _mapper;
        private readonly Mock<ICoursewiseLogger<CourseService>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<ILevelService> _levelServiceMock;

        public CourseServiceMock()
        {
            _allMocks = AllMocksGetter.AllMocks;
            _courseServiceMock = _allMocks.MockCourseService;
            _categoryServiceMock = _allMocks.MockCategoryService;
            _levelServiceMock = _allMocks.MockLevelService;
            _courseRepositoryMock = _allMocks.MockCourseRepository;
            _mapper = MockObjects.GetMapper(_allMocks.MockMapper.Object);
            _loggerMock = _allMocks.MockCourseServiceLogger;
            _unitOfWorkMock= _allMocks.MockUnitOfWork;
        }

        public CourseService SetupSuccessfullCourseServiceMock(CourseModel courseModel)
        {
            SetupGenFuObjects(courseModel.Type, courseModel.Level);
            _courseRepositoryMock.Setup(e => e.GetReadableByCategoryAndLevel(It.IsAny<string>(), It.IsAny<string>())).Returns(
                async () => { return await Task.FromResult(A.ListOf<Data.Entities.Course>(10)); }
                );
            _courseRepositoryMock.Setup(e => e.GetByType(It.IsAny<string>())).Returns(
                async () => { return await Task.FromResult(A.ListOf<Data.Entities.Course>(10)); }
                );
            return GetCourseService();
        }

        private CourseService GetCourseService()
        {  
            return new CourseService(_courseRepositoryMock.Object,_categoryServiceMock.Object, _levelServiceMock.Object, _loggerMock.Object,_unitOfWorkMock.Object, _mapper);
        }

        private static void SetupGenFuObjects(string type, string level)
        {
            A.Configure<Data.Entities.Level>()
                .Fill(op => op.Name, () => { return level; });
            A.Configure<Data.Entities.Category>()
                .Fill(op => op.Name, () => { return type; });
            A.Configure<Data.Entities.CourseLevel>()
                .Fill(op => op.Level, () => { return A.New<Data.Entities.Level>(); });
            A.Configure<Data.Entities.Course>()
                .Fill(op => op.CourseLevels, () => { return A.ListOf<Data.Entities.CourseLevel>(5); })
                .Fill(op => op.Category, () => { return A.New<Data.Entities.Category>(); });
            A.Configure<Domain.Models.CourseDto>()
                .Fill(op => op.Type, () => { return type; })
                .Fill(op => op.Type, () => { return level; });
                

        }
    }
}
