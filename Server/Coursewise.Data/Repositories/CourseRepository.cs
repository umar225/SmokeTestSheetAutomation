using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Repositories
{
    public class CourseRepository : GenericRepository<Course, Guid>, ICourseRepository
    {
        private readonly DbSet<Course> _coursesContext;

        public CourseRepository(ICoursewiseDbContext dbContext) : base(dbContext)
        {
            _coursesContext=dbContext.Courses;
        }

        public async Task<List<Course>> GetByType(string name)
        {
            var courses = await _coursesContext.AsQueryable().
                Include(c => c.Category).
                Where(c => !c.IsDeleted && c.IsVisible && c.Category.Name == name).ToListAsync();
            return courses;
        }

        public async Task<List<Course>> GetReadableByCategoryAndLevel(string category,string level)
        {
            var dbLevel = await DbContext.Levels.FirstOrDefaultAsync(l=>l.Name == level);
            if (dbLevel == null)
            {
                return new List<Course>();
            }
            var courses = await _coursesContext.AsQueryable().
                Include(c => c.CourseLevels.Where(l=>l.LevelId==dbLevel.Id)).
                    ThenInclude(c => c.Level).
                Include(c => c.Category).
                Where(c => !c.IsDeleted && c.IsVisible && c.Price == 0 && c.Category.Name == category && c.CourseLevels.Any()).ToListAsync();
            var selectedCourses = courses.Where(c => c.CourseLevels.Any()).ToList();
            return selectedCourses;
        }

    }
}
