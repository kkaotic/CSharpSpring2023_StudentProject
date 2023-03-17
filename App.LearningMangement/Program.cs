using System;
using App.LearningMangement.Helpers;
using Library.LearningManagement.Models;
using Library.LearningManagement.Services;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var studentService = new StudentService();
            var studentHelper = new StudentHelper(studentService);
            var courseHelper = new CourseHelper(studentService);

            bool cont = true;

            Console.WriteLine("Welcome to the Learning Management System v0.1!");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            while (cont)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("[1] Add a Student to Registry");
                Console.WriteLine("[2] Update a Student in Registry");
                Console.WriteLine("[3] List all registered Students");
                Console.WriteLine("[4] Search for a Student");
                Console.WriteLine("[5] Create a new Course");
                Console.WriteLine("[6] Update a Course");
                Console.WriteLine("[7] List all Courses");
                Console.WriteLine("[8] Search for a Course");
                Console.WriteLine("[9] Exit");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    if (result == 1)
                    {
                        studentHelper.CreateStudentRecord();
                    }else if (result == 2)
                    {
                        studentHelper.UpdateStudentRecord();
                    }else if (result == 3)
                    {
                        studentHelper.ListAndInfo();
                    }else if (result == 4)
                    {
                        studentHelper.SearchStudents();
                    }else if (result == 5)
                    {
                        courseHelper.CreateCourseRecord();
                    }else if (result == 6)
                    {
                        courseHelper.UpdateCourse();
                    }
                    else if (result == 7)
                    {
                        courseHelper.ListAndSelect();
                    }else if (result == 8)
                    {
                        courseHelper.SearchCourses();
                    }else if (result == 9)
                    {
                        Console.WriteLine("Thank you for using the Learning Management System v0.1!");
                        cont = false;
                    }

                }
            }
            
        }
    }
}