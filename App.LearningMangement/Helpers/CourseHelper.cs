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
            bool isCreate = false;
            if (selectedCourse == null)
            {
                isCreate = true;
                selectedCourse = new Course();
            }
            var choice = "Y";
            if (!isCreate)
            {
                Console.WriteLine("Do you want to update the course code?");
                choice = Console.ReadLine() ?? "N";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) 
            {
                Console.WriteLine("What is the code of the course?");
                selectedCourse.Code = Console.ReadLine() ?? string.Empty;
            }

            if (!isCreate)
            {
                Console.WriteLine("Do you want to update the course name?");
                choice = Console.ReadLine() ?? "N";
            } else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the name of the course?");
                selectedCourse.Name = Console.ReadLine() ?? string.Empty;
            }

            if (!isCreate)
            {
                Console.WriteLine("Do you want to update the course description?");
                choice = Console.ReadLine() ?? "N";
            } else
            {
                choice = "Y";
            }
            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What is the description of the course?");
                selectedCourse.Description = Console.ReadLine() ?? string.Empty;
            }


            if (isCreate)
            {
                SetupRoster(selectedCourse);
                SetupAssignments(selectedCourse);
            }


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
                    Console.WriteLine("Course Roster:");
                    selectedCourse?.Roster.ForEach(Console.WriteLine);
                    Console.WriteLine("");
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

        public void AddStudent()
        {
            Console.WriteLine("Select a course's code to update it's roster");
            ListCourses();

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
                    studentService.Students.Where(s => !selectedCourse.Roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);
                    if (studentService.Students.Any(s => !selectedCourse.Roster.Any(s2 => s2.Id == s.Id)))
                    {
                        selectionStr = Console.ReadLine() ?? string.Empty;
                    }
                    if(selectionStr != null)
                    {
                        var selectedId = int.Parse(selectionStr);
                        var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);
                        if(selectedStudent != null) 
                        {
                            selectedCourse.Roster.Add(selectedStudent);

                        }
                    }

                }
            }
        }

        public void RemoveStudent()
        {
            Console.WriteLine("Select a course's code to update it's roster");
            ListCourses();

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
                    selectedCourse.Roster.ForEach(Console.WriteLine);
                    if (selectedCourse.Roster.Any())
                    {
                        selectionStr = Console.ReadLine() ?? string.Empty;
                    }
                    else
                    {
                        selectionStr = null;
                    }
                    if (selectionStr != null)
                    {
                        var selectedId = int.Parse(selectionStr);
                        var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);
                        if (selectedStudent != null)
                        {
                            selectedCourse.Roster.Remove(selectedStudent);

                        }
                    }

                }
            }
        }

        private void SetupRoster(Course c)
        {
            Console.WriteLine("Which students should be enrolled in this course? ('Q' to Quit)");
            bool continueAdding = true;

            while (continueAdding)
            {
                studentService.Students.Where(s => !c.Roster.Any(s2 => s2.Id == s.Id)).ToList().ForEach(Console.WriteLine);

                var selection = "Q";
                if (studentService.Students.Any(s => !c.Roster.Any(s2 => s2.Id == s.Id)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase) || !studentService.Students.Any(s => !c.Roster.Any(s2 => s2.Id == s.Id)))
                {
                    continueAdding = false;
                }
                else
                {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.Id == selectedId);

                    if (selectedStudent != null)
                    {
                        c.Roster.Add(selectedStudent);
                    }
                }
            }
        }

        private void SetupAssignments(Course c)
        {
            Console.WriteLine("Add an Assignment to the course:");
            bool keepAdding = true;
            while (keepAdding)
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
                while (!dateFlag)
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
                c.Assignments.Add(assignment);

                Console.WriteLine("Would you like to quit adding assignments? ('Y' or 'N')");
                var opt = "Y";
                opt = Console.ReadLine() ?? string.Empty;

                if (opt.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    keepAdding = false;
                }
                else
                {
                    keepAdding = true;
                }

            }
        }

    }
}
