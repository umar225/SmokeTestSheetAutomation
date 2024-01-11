using Coursewise.Common.Models;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Models;
using System.Linq.Expressions;

namespace Coursewise.Domain.Interfaces
{
    public interface ICourseService : IGenericService<Course, Guid>
    {
        new Task<BaseModel> Add(Course course);
        new Task<BaseModel> Update(Course course);
        Task<BaseModel> Delete(Guid id);
        Task<BaseModel> GetCoachingCoursesByLevel(string text);

        Task<BaseModel> GetLeadershipCoursesByLevel(string text);

        Task<List<CourseDto>> GetPmpCourses();
        Task<BaseModel> CourseById(Guid id);
        Task<BaseModel> LibraryCoursesWithTypes();
        Task<BaseModel> AllDetails();
        Task<List<CourseDto>> LibraryCourses();
        Task<List<Course>> AllLibraryCourses(Guid? categoryId);
        Task<BaseModel> CategoryWithCourses(Guid categoryId);
        Task<BaseModel> CourseByUrl(string url);
        Task<List<CourseDto>> Get(Expression<Func<Data.Entities.Course, bool>> where);
        Task<List<CourseDto>> GetNedTraining();
    }
}