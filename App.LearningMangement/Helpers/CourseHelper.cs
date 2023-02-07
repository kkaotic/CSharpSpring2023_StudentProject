using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.LearningMangement.Helpers
{
    public class CourseHelper
    {
        private CourseService courseService = new CourseService();
        private StudentService studentService;

        public CourseHelper(StudentService service)
        {
            studentService = service;

        }

        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            Console.WriteLine("What is the code of the course?");
            var code = Console.ReadLine() ?? string.Empty; ;
            Console.WriteLine("What is the name of the course?");
            var name = Console.ReadLine() ?? string.Empty; ;
            Console.WriteLine("What is the description of the course?");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Which students should be enrolled in this course? ('Q' to Quit)");
            bool continueAdding = true;
            var roster = new List<Person>();

            while (continueAdding)
            {
                studentService.Students.Where(s => !roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);

                var selection = "Q";
                if(studentService.Students.Any(s => !roster.Any(s2 => s2.Id == s.Id)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase) || !studentService.Students.Any(s => !roster.Any(s2 => s2.Id == s.Id)))
                {
                    continueAdding = false;
                }
                else
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);

                    if (selectedStudent != null) 
                    {
                        roster.Add(selectedStudent);
                    }
                }
            }

            bool isCreate = false;
            if (selectedCourse == null)
            {
                isCreate = true;
                selectedCourse = new Course();
            }



            selectedCourse.Code = code ?? string.Empty;
            selectedCourse.Name = name ?? string.Empty;
            selectedCourse.Description = description;
            selectedCourse.Roster = new List<Person>();
            selectedCourse.Roster.AddRange(roster);

            if (isCreate)
            {
                courseService.Add(selectedCourse);
            }

        }

        public void UpdateCourse()
        {
            Console.WriteLine("Select a course's code to update");
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
        public void SearchCourses()
        {
            Console.WriteLine("Enter a course's name or description: ");
            var query = Console.ReadLine() ?? string.Empty;

            courseService.Search(query).ToList().ForEach(Console.WriteLine);
        }


    }
}
