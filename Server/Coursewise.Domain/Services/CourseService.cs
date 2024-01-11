using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Coursewise.Domain.Services
{
    public class CourseService : GenericService<Data.Entities.Course, Course, Guid>, ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryService _categoryService;
        private readonly ILevelService _levelService;
        private readonly ICoursewiseLogger<CourseService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository courseRepository,
            ICategoryService categoryService,
            ILevelService levelService,
            ICoursewiseLogger<CourseService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper): base(mapper, courseRepository, unitOfWork)
        {
            _courseRepository = courseRepository;
            _categoryService = categoryService;
            _levelService = levelService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public new async Task<BaseModel> Add(Course course)
        {
            course.Price = Math.Round(course.Price, 2);
            var validData = await CommonCourseValidation(course,true);
            if (!validData.success)
                return validData;
            var url = Common.Utilities.StringHelper.UrlFriendly((string.IsNullOrEmpty(course.ProviderName)) ? course.Name : course.Name + "-by-" + course.ProviderName);
            course.Url = url;
            course.IsVisible = true;
            var existingCourseWithSameUrl = await _courseRepository.FirstOrDefaultAsync(c => !c.IsDeleted && c.Url == url);
            
            if (course.CourseLevels != null)
            {
                foreach (var item in course.CourseLevels)
                {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    item.Level = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    item.Course = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                }
            }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            course.Category = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            var dataCourse = _mapper.Map<Data.Entities.Course>(course);
            await _courseRepository.AddAsync(dataCourse);
            await _unitOfWork.SaveChangesAsync();
            if (existingCourseWithSameUrl != null)
            {
                url = url + "-" + dataCourse.Id.ToString();
                var getCourse = await _courseRepository.GetAsync(dataCourse.Id);
                getCourse.Url = url;
                await _courseRepository.UpdateAsync(getCourse);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return BaseModel.Success(dataCourse.Id);
        }
        public new async Task<BaseModel> Update(Course course)
        {
            var existingCourse = await _courseRepository.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == course.Id, "CourseLevels");
            course.Price = Math.Round(course.Price, 2);
            var validData = await CourseValidation(course, existingCourse);
            if (!validData.success)
                return validData;
            var url = Common.Utilities.StringHelper.UrlFriendly((string.IsNullOrEmpty(course.ProviderName)) ? course.Name : course.Name + "-by-" + course.ProviderName);
            var existingCourseWithSameUrl = await _courseRepository.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id!=existingCourse.Id && c.Url == url);
            if (existingCourseWithSameUrl != null)
            {
                url = url + "-" + existingCourse.Id.ToString();
            }
            if (course.CourseLevels!=null)
            {
                foreach (var item in course.CourseLevels)
                {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    item.Level = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    item.Course = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                }
            }
            
            existingCourse.Url = url;
            existingCourse.CourseLevels = _mapper.Map<List<Data.Entities.CourseLevel>>(course.CourseLevels); 
            existingCourse.Name = course.Name;
            existingCourse.Price = course.Price;
            existingCourse.Desription = course.Desription;
            existingCourse.CategoryId = course.CategoryId;
            existingCourse.ProviderPrice = course.ProviderPrice;
            existingCourse.ProviderName = course.ProviderName;
            
            await _courseRepository.UpdateAsync(existingCourse);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success(existingCourse.Id);
        }

        public async Task<BaseModel> Delete(Guid id)
        {
            var course = await _courseRepository.GetAsync(id);
            if (course == null)
            {
                return BaseModel.Error("No course found");
            }
            await _courseRepository.DeleteAsync(course);
            await _unitOfWork.SaveChangesAsync();
            return BaseModel.Success();
        }

        public async Task<BaseModel> GetCoachingCoursesByLevel(string text)
        {
            var category = "Coaching Qualifications";
            var dbCourse = await _courseRepository.GetReadableByCategoryAndLevel(category, text);
            if (dbCourse.Count==0)
            {
                var message = $"No {category} courses of level {text} available";
                _logger.Info(message);
                return BaseModel.Error(message);
            }
            var courses = _mapper.Map<List<CourseDto>>(dbCourse);
            foreach (var course in courses)
            {
                var coursesLevels = dbCourse.Find(x => x.Id == course.Id)!.CourseLevels;
                if (coursesLevels.Any() && coursesLevels.FirstOrDefault()!.Level !=null)
                {
                    course.Level = coursesLevels.FirstOrDefault()!.Level.Name;
                }
            }
            return BaseModel.Success(courses);
        }

        public async Task<BaseModel> GetLeadershipCoursesByLevel(string text)
        {
            var category = "Leadership and Management Qualifications";
            var dbCourse = await _courseRepository.GetReadableByCategoryAndLevel(category, text);
            if (dbCourse.Count == 0)
            {
                var message = $"No {category} courses of level {text} available";
                _logger.Info(message);
                return BaseModel.Error(message);
            }
            var courses = _mapper.Map<List<CourseDto>>(dbCourse);
            foreach (var course in courses)
            {
                var coursesLevels = dbCourse.Find(x => x.Id == course.Id)!.CourseLevels;
                if (coursesLevels.Any() && coursesLevels.FirstOrDefault()!.Level != null)
                {
                    course.Level = coursesLevels.FirstOrDefault()!.Level.Name;
                }
            }
            return BaseModel.Success(courses);
        }

        public async Task<List<CourseDto>> GetPmpCourses()
        {
            var coursesDto = await _courseRepository.GetByType("Project Management Qualifications");
            var courses = _mapper.Map<List<CourseDto>>(coursesDto);

            return courses;
        }
        public async Task<BaseModel> CourseById(Guid id)
        {
            var course = await _courseRepository.Get().Where(x => x.Id == id && !x.IsDeleted)
                                .Select(c=> new Course
                                {
                                    CategoryId = c.CategoryId,
                                    Name = c.Name,
                                    Desription = c.Desription,
                                    Id = c.Id,
                                    IsVisible = c.IsVisible,
                                    Price = c.Price,
                                    ProviderName = c.ProviderName,
                                    ProviderPrice = c.ProviderPrice,
                                    CourseLevels = c.CourseLevels.Select(x => new CourseLevel
                                    {
                                        LevelId = x.LevelId,
                                    })
                                }).FirstOrDefaultAsync();
            if (course == null)
            {
                _logger.Info($"No course found in library against this id {id}");
                return BaseModel.Error("No course found");
            }
            var categories = await _categoryService.Get();
            var levels = await _levelService.Get();
            var result = new { course, categories, levels };
            _logger.Info($"Returning {course.Name} course, {categories.Count} categories, {levels.Count} levels for add or edit course");
            return BaseModel.Success(result);            
        }
        public async Task<BaseModel> LibraryCoursesWithTypes()
        {
            var courses = await LibraryCourses();
            if (courses.Count==0)
            {
                return BaseModel.Error("No courses available in the library");
            }
            var couresetypes = await _categoryService.Get();
            var types = new List<object>();
            foreach (var item in couresetypes)
            {
                var name = string.IsNullOrEmpty(item.DisplayName) ? item.Name : item.DisplayName;
                types.Add(new { Name = name, Type = item.Name });
            }
            var result = new { courses = courses, courseTypes = types };
            _logger.Info($"Getting {courses.Count} library courses");
            return await Task.FromResult(BaseModel.Success(result));
        }
        public async Task<BaseModel> AllDetails()
        {
            var courses = await AllLibraryCourses(categoryId:null);
            var categories = await _categoryService.GetAll();
            var levels = await _levelService.Get();
            var result = new { courses, categories, levels };
            _logger.Info($"Returning {courses.Count} courses, {categories.Count} categories, {levels.Count} levels  for library");
            return await Task.FromResult(BaseModel.Success(result));
        }
        public async Task<BaseModel> CategoryWithCourses(Guid categoryId)
        {
            var dbcategory = await _categoryService.Get(categoryId);
            if (dbcategory == null || dbcategory.IsDeleted)
            {
                return BaseModel.Error("No category available");
            }
            var courses = await _courseRepository.Get().                
                Where(c => !c.IsDeleted && c.CategoryId == categoryId).
                Select(c => new Course
                {
                    Name = c.Name,
                    Price = c.Price,
                    ProviderName = c.ProviderName,
                    CategoryId = c.CategoryId,
                    Id = c.Id,
                    IsVisible = c.IsVisible,
                    ProviderPrice = c.ProviderPrice,
                    Url = c.Url,
                })
                .ToListAsync();
            var category = _mapper.Map<Category>(dbcategory);
            _logger.Info($"Returning {courses.Count} courses for library");
            return BaseModel.Success(new { courses, category });
        }
        public async Task<List<CourseDto>> Get(Expression<Func<Data.Entities.Course, bool>> where)
        {
            var dataCourses = await repository.Get().Where(where).ToListAsync();
            var domainCourses = _mapper.Map<List<CourseDto>>(dataCourses);
            return domainCourses;
        }
        public async Task<List<CourseDto>> LibraryCourses()
        {
            var dbCourses = await _courseRepository.Get().
                Include(c=>c.Category).
                Where(c => !c.IsDeleted && c.IsVisible && c.Price > 0).ToListAsync();
            var  courses = _mapper.Map<List<CourseDto>>(dbCourses);
            _logger.Info($"Getting {courses.Count} library courses");
            return courses;
        }

        public async Task<List<Course>> AllLibraryCourses(Guid? categoryId)
        {
            var coursesQuery = _courseRepository.Get().
                Include(c => c.Category).
                Include(c => c.CourseLevels).
                Where(c => !c.IsDeleted);
            IQueryable<Course> courseDomainQuery; 
            if (categoryId != null && categoryId != Guid.Empty)
            {
                coursesQuery = coursesQuery.Where(c=>c.CategoryId == categoryId);
            }
            courseDomainQuery = coursesQuery.Select(c => new Course
            {
                Category = new Category
                {
                    Name = c.Category.Name,
                    DisplayName = c.Category.DisplayName,
                    Id = c.Category.Id,
                    IsVisible = c.Category.IsVisible,
                },
                CourseLevels = c.CourseLevels.Select(l => new CourseLevel
                {
                    CourseId = l.CourseId,
                    LevelId = l.LevelId,
                }),
                Name = c.Name,
                Price = c.Price,
                ProviderName = c.ProviderName,
                CategoryId = c.CategoryId,
                Id = c.Id,
                IsVisible = c.IsVisible,
                ProviderPrice = c.ProviderPrice,
                Url = c.Url,
            });
            var courses = await courseDomainQuery.ToListAsync();
            _logger.Info($"Getting {courses.Count} library courses");
            return courses;
        }

        public async Task<BaseModel> CourseByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BaseModel.Error("No course found");
            }
            var course = await _courseRepository.FirstOrDefaultAsync(c => c.Url == url && !c.IsDeleted && c.IsVisible);
            if (course==null)
            {
                _logger.Info($"No course found in library against this url {url}");
                return BaseModel.Error("No course found");
            }
            var courseDto=_mapper.Map<CourseDto>(course);
            return BaseModel.Success(courseDto);
        }

        

        public async Task<List<CourseDto>> GetNedTraining()
        {
            var coursesDto = await _courseRepository.GetByType("NED Training");
            var courses = _mapper.Map<List<CourseDto>>(coursesDto);
            return courses;
        }

        private async Task<BaseModel> CourseValidation(Course course, Data.Entities.Course existingCourse)
        {
            if (existingCourse == null)
            {
                return BaseModel.Error("No course exist");
            }
            
            if (course.CategoryId != existingCourse.CategoryId)
            {
                var commonValidation = await CommonCourseValidation(course, true);
                if (!commonValidation.success)
                {
                    return commonValidation;
                }
            }
            else
            {
                var commonValidation = await CommonCourseValidation(course, null);
                if (!commonValidation.success)
                {
                    return commonValidation;
                }
            }
            

            var existingCourseWithSameName = await _courseRepository.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id != existingCourse.Id && c.Name == course.Name && c.CategoryId == course.CategoryId);
            if (existingCourseWithSameName != null && (existingCourseWithSameName.Price == course.Price || (existingCourseWithSameName.Price > 0 && course.Price > 0 &&
                    !String.IsNullOrEmpty(existingCourseWithSameName.ProviderName) && !String.IsNullOrEmpty(course.ProviderName) && existingCourseWithSameName.ProviderName == course.ProviderName)))
            {
                var eMessage = $"This course name {course.Name} is already exist under this category with price {existingCourseWithSameName.Price}";
                if (!String.IsNullOrEmpty(existingCourseWithSameName.ProviderName) && existingCourseWithSameName.ProviderName == course.ProviderName)
                {
                    eMessage = $"This course name {course.Name} with provider {course.ProviderName} is already exist under this category with price {existingCourseWithSameName.Price}";
                    _logger.Info(eMessage);
                    return BaseModel.Error(eMessage);
                }
                _logger.Info(eMessage);
                return BaseModel.Error(eMessage);
            }
            return BaseModel.Success();
        }
        private async Task<BaseModel> CommonCourseValidation(Course course,bool? checkCategory)
        {
            if (checkCategory.HasValue && checkCategory.Value)
            {
                var isCategoryCorrect = await _categoryService.IsExist(course.CategoryId);
                if (!isCategoryCorrect)
                {
                    return BaseModel.Error("Category is not correct");
                }
            }
            if (course.Price > 0 && String.IsNullOrEmpty(course.ProviderName))
            {
                return BaseModel.Error("Library course required a provider name");
            }
            if (string.IsNullOrEmpty(course.Desription))
            {
                course.Desription = "<div></div>";
            }
            if (course.CourseLevels != null && course.CourseLevels.Any())
            {
                var isLevelsCorrect = await _levelService.HasValidLevels(course.CourseLevels.Select(l => l.LevelId).ToList());
                if (!isLevelsCorrect)
                {
                    _logger.Info("Levels are not correct");
                    return BaseModel.Error("Levels are not correct");
                }
            }
            return BaseModel.Success();
        }
    }
}