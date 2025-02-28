using Microsoft.Playwright;
using NUnit.Framework;
using SchoolManagement.Playwright.Base;
using SchoolManagement.Playwright.Helpers;
using SchoolManagement.Playwright.PageObjects;
using System;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.Tests
{
    [TestFixture]
    public class SchoolManagementEndToEndTests : BaseTest
    {
        private StudentsPage _studentsPage = null!;
        private TeachersPage _teachersPage = null!;
        private ClassesPage _classesPage = null!;

        [SetUp]
        public async Task TestSetUp()
        {
            // Navigate to the application
            await Page!.GotoAsync(BaseUrl);
            
            // Wait for the page to load
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Print the page title for debugging
            System.Console.WriteLine($"Page title: {await Page.TitleAsync()}");
            
            // Take a screenshot for debugging
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "homepage.png" });
            System.Console.WriteLine("Screenshot saved to homepage.png");
            
            // Initialize page objects
            _studentsPage = new StudentsPage(Page);
            _teachersPage = new TeachersPage(Page);
            _classesPage = new ClassesPage(Page);
            
            // Verify the page has loaded by checking for the presence of navigation elements
            var studentsNavLink = Page.Locator("nav a.nav-link", new() { HasText = "Students" });
            
            // Wait for the navigation link to be visible
            await studentsNavLink.WaitForAsync(new() { Timeout = 10000 });
            Assert.That(await studentsNavLink.IsVisibleAsync(), Is.True, "Students navigation link is not visible");
        }

        [Test]
        public async Task SchoolManagementEndToEndTest()
        {
            try
            {
                // Step 1: Add 3 students
                for (int i = 0; i < TestData.Students.Length; i++)
                {
                    var student = TestData.Students[i];
                    System.Console.WriteLine($"Adding student {i+1}/{TestData.Students.Length}: {student.Name} ({student.Id})");
                    
                    // Reload the page to ensure we have a clean state
                    if (i > 0)
                    {
                        await Page!.ReloadAsync();
                        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    }
                    
                    await _studentsPage.AddStudent(
                        name: student.Name,
                        studentId: student.Id,
                        email: student.Email,
                        phone: student.Phone
                    );
                    
                    // Verify student was added
                    var isStudentAdded = await _studentsPage.VerifyStudentAdded(student.Name, student.Id);
                    Assert.That(isStudentAdded, Is.True, $"Student {student.Name} with ID {student.Id} was not added successfully");
                }
                
                // Step 2: Add 3 teachers
                for (int i = 0; i < TestData.Teachers.Length; i++)
                {
                    var teacher = TestData.Teachers[i];
                    System.Console.WriteLine($"Adding teacher {i+1}/{TestData.Teachers.Length}: {teacher.Name} ({teacher.Id})");
                    
                    // Reload the page to ensure we have a clean state
                    await Page!.ReloadAsync();
                    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    
                    await _teachersPage.AddTeacher(
                        name: teacher.Name,
                        teacherId: teacher.Id,
                        email: teacher.Email,
                        phone: teacher.Phone,
                        subject: teacher.Subject,
                        address: teacher.Address
                    );
                    
                    // Verify teacher was added
                    var isTeacherAdded = await _teachersPage.VerifyTeacherAdded(teacher.Name, teacher.Id);
                    Assert.That(isTeacherAdded, Is.True, $"Teacher {teacher.Name} with ID {teacher.Id} was not added successfully");
                }
                
                // Step 3: Add 2 classes
                for (int i = 0; i < TestData.Classes.Length; i++)
                {
                    var classData = TestData.Classes[i];
                    System.Console.WriteLine($"Adding class {i+1}/{TestData.Classes.Length}: {classData.Name} ({classData.Code})");
                    
                    // Reload the page to ensure we have a clean state
                    await Page!.ReloadAsync();
                    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    
                    await _classesPage.AddClass(
                        name: classData.Name,
                        classCode: classData.Code,
                        description: classData.Description
                    );
                    
                    // Verify class was added
                    var isClassAdded = await _classesPage.VerifyClassAdded(classData.Name, classData.Code);
                    Assert.That(isClassAdded, Is.True, $"Class {classData.Name} with code {classData.Code} was not added successfully");
                }
                
                // Step 4: Assign teachers to classes
                for (int i = 0; i < TestData.Classes.Length; i++)
                {
                    var classData = TestData.Classes[i];
                    var teacher = TestData.Teachers[i]; // Assign each teacher to a different class
                    System.Console.WriteLine($"Assigning teacher {teacher.Name} to class {classData.Name}");
                    
                    // Reload the page to ensure we have a clean state
                    await Page!.ReloadAsync();
                    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    
                    await _classesPage.AssignTeacherToClass(
                        teacherName: teacher.Name,
                        className: classData.Name
                    );
                }
                
                // Step 5: Assign students to classes
                // For each class, assign 2 students
                for (int i = 0; i < TestData.Classes.Length; i++)
                {
                    var classData = TestData.Classes[i];
                    
                    // Assign 2 students to each class
                    for (int j = 0; j < 2; j++)
                    {
                        var studentIndex = j + i;
                        if (studentIndex >= TestData.Students.Length)
                        {
                            studentIndex = j; // Wrap around if we run out of students
                        }
                        
                        var student = TestData.Students[studentIndex];
                        System.Console.WriteLine($"Assigning student {student.Name} to class {classData.Name}");
                        
                        // Reload the page to ensure we have a clean state
                        await Page!.ReloadAsync();
                        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                        
                        await _classesPage.AssignStudentToClass(
                            studentName: student.Name,
                            className: classData.Name
                        );
                    }
                }
                
                // Take a final screenshot
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "final-state.png" });
                System.Console.WriteLine("Final screenshot saved to final-state.png");
            }
            catch (System.Exception ex)
            {
                // Take a screenshot on error
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "error.png" });
                System.Console.WriteLine($"Error screenshot saved to error.png");
                System.Console.WriteLine($"Error: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
