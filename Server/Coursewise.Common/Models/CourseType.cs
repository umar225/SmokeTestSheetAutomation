using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Common.Models
{
    public enum CourseType
    {
        NOTYPE,
        PROJECT_MANAGEMENT_QUALIFICATIONS,
        LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS,
        COACHING_QUALIFICATIONS,
        NED_TRAINING
    }
    public static class CourseTypeModel
    {
        public static string PROJECT_MANAGEMENT_QUALIFICATIONS { get; private set; } = "Project Management Qualifications";
        public static string LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS { get; private set; } = "Leadership and Management Qualifications";
        public static string COACHING_QUALIFICATIONS { get; private set; } = "Coaching Qualifications";
        public static string NED_TRAINING { get; private set; } = "Ned Training";
        public static string GetModel(CourseType type)
        {
            string model = "";
            switch (type)
            {
                case CourseType.PROJECT_MANAGEMENT_QUALIFICATIONS:
                    model = PROJECT_MANAGEMENT_QUALIFICATIONS;
                    break;
                case CourseType.LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS:
                    model = LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS;
                    break;
                case CourseType.COACHING_QUALIFICATIONS:
                    model = COACHING_QUALIFICATIONS;
                    break;
                case CourseType.NED_TRAINING:
                    model = NED_TRAINING;
                    break;
                default:
                    break;
            }
            return model;
        }
        public static CourseType GetModel(string type)
        {
            if (type == PROJECT_MANAGEMENT_QUALIFICATIONS)
            {
                return CourseType.PROJECT_MANAGEMENT_QUALIFICATIONS;
            }
            else if (type == LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS)
            {
                return CourseType.LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS;
            }
            else if (type == COACHING_QUALIFICATIONS)
            {
                return CourseType.COACHING_QUALIFICATIONS;
            }
            else if (type == NED_TRAINING)
            {
                return CourseType.NED_TRAINING;
            }
            else
            {
                return CourseType.NOTYPE;
            }
        }

        public static object SetupCourseType()
        {
            var obj = new List<object>() {
            new { Name = "Project Management", Type = CourseTypeModel.PROJECT_MANAGEMENT_QUALIFICATIONS },
            new { Name = "Coaching", Type = CourseTypeModel.COACHING_QUALIFICATIONS },
            new { Name = "Leadership and Management", Type = CourseTypeModel.LEADERSHIP_AND_MANAGEMENT_QUALIFICATIONS },
            new { Name = "NED Training", Type = CourseTypeModel.NED_TRAINING },
            };
            return obj;
        }
        
    }
}
