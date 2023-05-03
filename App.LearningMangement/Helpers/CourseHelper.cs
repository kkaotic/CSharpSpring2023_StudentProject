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
                SetUpModules(selectedCourse);
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
                    Console.WriteLine(selectedCourse.DetailDisplay);
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

        private Assignment CreateAssignment()
        {
            Console.Write("Name: ");
            var assignmentName = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Description: ");
            var assignmentDesc = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Total Points: ");
            var totalPoints = decimal.Parse(Console.ReadLine() ?? "100");
            Console.WriteLine("Due Date: ");
            var dueDate = DateTime.Parse(Console.ReadLine() ?? "01/01/1900");

            return new Assignment
            {
                Name = assignmentName,
                Description = assignmentDesc,
                totalAvailablePoints = totalPoints,
                DueDate = dueDate
            };
        }

        public void AddAssignment()
        {
            Console.WriteLine("Enter the code for the course to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection));
            if (selection != null) 
            {
                selectedCourse.Assignments.Add(CreateAssignment());
            }
        }

        public void AddModule()
        {
            Console.WriteLine("Enter the code for the course to add the Module to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection));
            if (selectedCourse != null) 
            {
                selectedCourse.Modules.Add(CreateModule(selectedCourse));
            }
        }

        public void RemoveModule()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection));
            if (selection != null)
            {
                Console.WriteLine("Choose an Module to remove:");

                selectedCourse.Modules.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedModule = selectedCourse.Modules.FirstOrDefault(m => m.Id == selectionInt);
                if (selectedModule != null)
                {
                    selectedCourse.Modules.Remove(selectedModule);
                }
            }
        }

        public void UpdateModule()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            Console.WriteLine("Enter the ID of the Module to update:");
            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection));

            if(selectedCourse != null && selectedCourse.Modules.Any())
            {
                selectedCourse.Modules.ForEach(Console.WriteLine);

                selection = Console.ReadLine();
                var selectedModule = selectedCourse.Modules.FirstOrDefault(m => m.Id.ToString().Equals(selection, StringComparison.InvariantCultureIgnoreCase));

                if(selectedModule != null)
                {
                    Console.WriteLine("Would you like to change the Module Name? (Y/N)");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Name:");
                        selectedModule.Name = Console.ReadLine();
                    }
                    Console.WriteLine("Would you like to change the Module Description? (Y/N)");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        Console.WriteLine("Description:");
                        selectedModule.Description = Console.ReadLine();
                    }
                    Console.WriteLine("Would you like to delete content from this module?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        var keepRemoving = true;
                        while(keepRemoving)
                        {
                            selectedModule.Content.ForEach(Console.WriteLine);
                            selection = Console.ReadLine();


                            var contentToRemove = selectedModule.Content.FirstOrDefault(c => c.Id.ToString().Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                            if (contentToRemove != null)
                            {
                                selectedModule.Content.Remove(contentToRemove);
                            }

                            Console.WriteLine("Would you like to keep removing content? (Y/N)");
                            selection = Console.ReadLine();
                            if(selection?.Equals("N", StringComparison.InvariantCultureIgnoreCase) ?? false)
                            {
                                keepRemoving = false;
                            }
                        }

                    }
                    Console.WriteLine("Would you like to add Content? (Y/N)");
                    var choice = Console.ReadLine() ?? "N";
                    while (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("What type of Content would you like to add?");
                        Console.WriteLine("[1] Assignment");
                        Console.WriteLine("[2] File");
                        Console.WriteLine("[3] Page");
                        var contentChoice = int.Parse(Console.ReadLine() ?? "0");

                        switch (contentChoice)
                        {
                            case 1:
                                var newAssignmentContent = CreateAssignmentItem(selectedCourse);
                                if (newAssignmentContent != null)
                                {
                                    selectedModule.Content.Add(newAssignmentContent);
                                }
                                break;
                            case 2:
                                var newFileContent = CreateFileItem(selectedCourse);
                                if (newFileContent != null)
                                {
                                    selectedModule.Content.Add(newFileContent);
                                }
                                break;
                            case 3:
                                var newPageContent = CreatePageItem(selectedCourse);
                                if (newPageContent != null)
                                {
                                    selectedModule.Content.Add(newPageContent);
                                }
                                break;
                            default:
                                break;
                        }

                        Console.WriteLine("Would you like to add more Content? (Y/N)");
                        choice = Console.ReadLine() ?? "N";

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
            Console.WriteLine("Would you like to add assignments? (Y/N)");
            bool continueAdding = true;
            var assignResponse = Console.ReadLine() ?? "N";

            if (assignResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) 
            {
                Console.WriteLine("Add an Assignment to the course:");
                while (continueAdding)
                {
                    c.Assignments.Add(CreateAssignment());

                    Console.WriteLine("Would you like to keep adding assignments? ('Y' or 'N')");
                    var opt = Console.ReadLine() ?? "N";
                    if (opt.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continueAdding = true;
                    }
                    else
                    {
                        continueAdding = false;
                    }

                }
            }
        }

        public void UpdateAssignment()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection));
            if (selection != null)
            {
                Console.WriteLine("Choose an assignment to update:");

                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if (selectedAssignment != null)
                {
                    var index = selectedCourse.Assignments.IndexOf(selectedAssignment);
                    selectedCourse.Assignments.RemoveAt(index);
                    selectedCourse.Assignments.Insert(index, CreateAssignment());
                }
            }
        }

        public void RemoveAssignment()
        {
            Console.WriteLine("Enter the code for the course:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection));
            if (selection != null)
            {
                Console.WriteLine("Choose an assignment to remove:");

                selectedCourse.Assignments.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.Id == selectionInt);
                if (selectedAssignment != null)
                {
                    selectedCourse.Assignments.Remove(selectedAssignment);
                }
            }
        }

        private void SetUpModules(Course c)
        {
            Console.WriteLine("Would you like to add a Module? (Y/N)");
            var modResponse = Console.ReadLine() ?? "N";
            bool continueAdding;
            if (modResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                continueAdding = true;
                while (continueAdding)
                {
                    c.Modules.Add(CreateModule(c));
                    Console.WriteLine("Add more Modules? (Y/N)");
                    modResponse = Console.ReadLine() ?? "N";
                    if (modResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continueAdding = false;
                    }
                }
            }
        }

        private Module CreateModule(Course c)
        {
            Console.WriteLine("Name:");
            var modName = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Description:");
            var modDescription = Console.ReadLine() ?? string.Empty;

            var module = new Module
            {
                Name = modName,
                Description = modDescription
            };
            Console.WriteLine("Would you like to add Content? (Y/N)");
            var choice = Console.ReadLine() ?? "N";
            while(choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("What type of Content would you like to add?");
                Console.WriteLine("[1] Assignment");
                Console.WriteLine("[2] File");
                Console.WriteLine("[3] Page");
                var contentChoice = int.Parse(Console.ReadLine() ?? "0");

                switch (contentChoice)
                {
                    case 1:
                        var newAssignmentContent = CreateAssignmentItem(c);
                        if(newAssignmentContent != null) 
                        {
                            module.Content.Add(newAssignmentContent);
                        }
                        //module.Content.Add(CreateAssignmentItem(c));
                        break;
                    case 2:
                        var newFileContent = CreateFileItem(c);
                        if (newFileContent != null)
                        {
                            module.Content.Add(newFileContent);
                        }
                        //module.Content.Add(CreateFileItem(c));
                        break;
                    case 3:
                        var newPageContent = CreatePageItem(c);
                        if (newPageContent != null)
                        {
                            module.Content.Add(newPageContent);
                        }
                       // module.Content.Add(CreatePageItem(c));
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Would you like to add more Content? (Y/N)");
                choice = Console.ReadLine() ?? "N";

            }

            return module;
        }

        private AssignmentItem? CreateAssignmentItem(Course c)
        {
            Console.WriteLine("Which Assignment should be added?");
            c.Assignments.ForEach(Console.WriteLine);
            var choice = int.Parse(Console.ReadLine() ?? "-1");

            if (choice >= 0)
            {
                var assignment = c.Assignments.FirstOrDefault(a => a.Id == choice);
                return new AssignmentItem { Assignment = assignment };
            }
            return null;
        }

        private FileItem? CreateFileItem(Course c)
        {
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter a path to the file:");
            var filePath = Console.ReadLine();

            return new FileItem 
            { Path = filePath,
              Name = name,
              Description = description
            };

        }

        private PageItem? CreatePageItem(Course c)
        {
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter page content:");
            var body = Console.ReadLine();

            return new PageItem
            {
                HtmlBody = body,
                Name = name,
                Description = description
            };

        }

    }
}
