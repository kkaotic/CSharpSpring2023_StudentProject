using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.LearningMangement.Helpers
{
    public class CourseHelper
    {
        private CourseService courseService = new CourseService();

        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            Console.WriteLine("What is the code of the course?");
            var code = Console.ReadLine() ?? string.Empty; ;
            Console.WriteLine("What is the name of the course?");
            var name = Console.ReadLine() ?? string.Empty; ;
            Console.WriteLine("What is the description of the course?");
            var description = Console.ReadLine() ?? string.Empty;

            bool isCreate = false;
            if (selectedCourse == null)
            {
                isCreate = true;
                selectedCourse = new Course();
            }



            selectedCourse.Code = code ?? string.Empty;
            selectedCourse.Name = name ?? string.Empty;
            selectedCourse.Description = description;

            if (isCreate)
            {
                courseService.Add(selectedCourse);
            }

            /*var course = new Course
            {
                Code = code,
                Name = name,
                Description = description

            };

            courseService.Add(course);
            */
        }

        public void UpdateCourse()
        {
            Console.WriteLine("Select a course to update");
            ListCourses();

            var selectionStr = Console.ReadLine();

            bool IsString(object value)
            {
                return value is string;
            }

            if (IsString(selectionStr??string.Empty))
            {
                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code == selectionStr);
                if (selectionStr != null)
                {
                    CreateCourseRecord(selectedCourse);
                }
            }
        }

        public void ListCourses()
        {
            courseService.Courses.ForEach(Console.WriteLine);

        }



    }
}
