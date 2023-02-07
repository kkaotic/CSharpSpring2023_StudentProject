using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.LearningMangement.Helpers
{
    public class CourseHelper
    {
        private CourseService courseService;
        private StudentService studentService;

        public CourseHelper(StudentService service)
        {
            studentService = service;
            courseService = CourseService.Current;

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

            Console.WriteLine("Add an Assignment to the course:");
            bool keepAdding = true;
            var assignments = new List<Assignment>();

            while(keepAdding)
            {
                Assignment assignment = new Assignment();
                Console.WriteLine("What is the Assignment name?");
                var ass_name = Console.ReadLine();
                Console.WriteLine("What is the Assignment Description?");
                var desc = Console.ReadLine();
                Console.WriteLine("How many points is the assignment worth?");
                var points = Console.ReadLine();
                Console.WriteLine("When is the Assignment Due?");
                DateTime date;
                bool dateFlag = false;
                while(!dateFlag)
                {
                    if (DateTime.TryParse(Console.ReadLine(), out date))
                    {
                        assignment.DueDate = date;
                        assignment.Description = desc;
                        assignment.Name = ass_name;
                        assignment.totalAvailablePoints = int.Parse(points ?? "0");
                        dateFlag = true;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Date Format, please try again:");
                    }
                }
                assignments.Add(assignment);

                Console.WriteLine("Would you like to quit adding assignments? ('Y' or 'N')");
                var choice = "Y";
                choice = Console.ReadLine() ?? string.Empty;

                if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    keepAdding = false;
                }
                else
                {
                    keepAdding = true;
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
            selectedCourse.Assignments=new List<Assignment>();
            selectedCourse.Assignments.AddRange(assignments);

            if(isCreate)
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

        public void ListAndSelect()
        {
            courseService.Courses.ForEach(Console.WriteLine);

            Console.WriteLine("Which course would you like to select?");
            var selectionStr = Console.ReadLine();

            bool IsString(object value)
            {
                return value is string;
            }

            if (IsString(selectionStr ?? string.Empty))
            {
                var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code == selectionStr);
                if (selectionStr != null)
                {
                    Console.WriteLine("Course Assignments:");
                    selectedCourse?.Assignments.ForEach(Console.WriteLine);
                }
            }
        }

        public void SearchCourses()
        {
            Console.WriteLine("Enter a course's name or description: ");
            var query = Console.ReadLine() ?? string.Empty;

            courseService.Search(query).ToList().ForEach(Console.WriteLine);
        }


    }
}
