using Microsoft.Playwright;
using NUnit.Framework;
using SchoolManagement.Playwright.Base;
using SchoolManagement.Playwright.Helpers;
using SchoolManagement.Playwright.PageObjects;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.Tests
{
    [TestFixture]
    public class DuplicateCheckTests : BaseTest
    {
        private StudentsPage _studentsPage = null!;
        private TeachersPage _teachersPage = null!;

        [SetUp]
        public async Task TestSetUp()
        {
            System.Console.WriteLine("=== TEST SETUP PERFORMANCE DEBUGGING ===");
            var totalSetupSw = Stopwatch.StartNew();
            
            // Navigate to the application
            System.Console.WriteLine("Navigating to application URL...");
            var navigationSw = Stopwatch.StartNew();
            await Page!.GotoAsync(BaseUrl);
            navigationSw.Stop();
            System.Console.WriteLine($"Initial navigation completed in {navigationSw.ElapsedMilliseconds}ms");
            
            // Wait for the page to load
            System.Console.WriteLine("Waiting for network idle...");
            var networkSw = Stopwatch.StartNew();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            networkSw.Stop();
            System.Console.WriteLine($"Network idle reached in {networkSw.ElapsedMilliseconds}ms");
            
            // Initialize page objects
            System.Console.WriteLine("Initializing page objects...");
            var initSw = Stopwatch.StartNew();
            _studentsPage = new StudentsPage(Page);
            _teachersPage = new TeachersPage(Page);
            initSw.Stop();
            System.Console.WriteLine($"Page objects initialized in {initSw.ElapsedMilliseconds}ms");
            
            // Verify the page has loaded by checking for the presence of navigation elements
            System.Console.WriteLine("Verifying page has loaded...");
            var verifySw = Stopwatch.StartNew();
            var studentsNavLink = Page.Locator("nav a.nav-link", new() { HasText = "Students" });
            System.Console.WriteLine($"Students nav link located in {verifySw.ElapsedMilliseconds}ms");
            
            // Wait for the navigation link to be visible
            var waitSw = Stopwatch.StartNew();
            await studentsNavLink.WaitForAsync(new() { Timeout = 10000 });
            waitSw.Stop();
            System.Console.WriteLine($"Waited for Students nav link to be visible in {waitSw.ElapsedMilliseconds}ms");
            
            var visibleSw = Stopwatch.StartNew();
            var isVisible = await studentsNavLink.IsVisibleAsync();
            visibleSw.Stop();
            System.Console.WriteLine($"Checked if Students nav link is visible in {visibleSw.ElapsedMilliseconds}ms");
            
            Assert.That(isVisible, Is.True, "Students navigation link is not visible");
            
            verifySw.Stop();
            System.Console.WriteLine($"Page verification completed in {verifySw.ElapsedMilliseconds}ms");
            
            totalSetupSw.Stop();
            System.Console.WriteLine($"Total test setup time: {totalSetupSw.ElapsedMilliseconds}ms");
        }

        [Test]
        public async Task StudentDuplicateIdCheckTest()
        {
            // Create a stopwatch for timing operations
            var totalStopwatch = Stopwatch.StartNew();
            var operationStopwatch = new Stopwatch();
            
            try
            {
                System.Console.WriteLine("=== PERFORMANCE DEBUGGING ===");
                System.Console.WriteLine("Starting duplicate student ID check test");
                
                // Step 1: Navigate to Students page
                System.Console.WriteLine("\n=== NAVIGATING TO STUDENTS PAGE ===");
                var navStopwatch = Stopwatch.StartNew();
                await _studentsPage.NavigateToStudentsPage();
                navStopwatch.Stop();
                System.Console.WriteLine($"Navigation completed in {navStopwatch.ElapsedMilliseconds}ms");

                // Step 2: Add a student
                System.Console.WriteLine("\n=== ADDING FIRST STUDENT ===");
                var student = (Name: "Duplicate Test Student", Id: "DUP123", Email: "duplicate@example.com", Phone: "555-123-4567");
                
                // Time the entire student addition process
                operationStopwatch.Restart();
                
                // Click Add New Student button
                System.Console.WriteLine($"Clicking Add New Student button...");
                var buttonStopwatch = Stopwatch.StartNew();
                await _studentsPage.ClickAddNewStudentButton();
                buttonStopwatch.Stop();
                System.Console.WriteLine($"Button click completed in {buttonStopwatch.ElapsedMilliseconds}ms");
                
                // Fill student form
                System.Console.WriteLine($"Filling student form...");
                var formStopwatch = Stopwatch.StartNew();
                await _studentsPage.FillStudentForm(
                    name: student.Name,
                    studentId: student.Id,
                    email: student.Email,
                    phone: student.Phone
                );
                formStopwatch.Stop();
                System.Console.WriteLine($"Form filling completed in {formStopwatch.ElapsedMilliseconds}ms");
                
                // Submit student form
                System.Console.WriteLine($"Submitting student form...");
                var submitStopwatch = Stopwatch.StartNew();
                await _studentsPage.SubmitStudentForm();
                submitStopwatch.Stop();
                System.Console.WriteLine($"Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                
                // Verify student was added
                System.Console.WriteLine($"Verifying student was added...");
                var verifyStopwatch = Stopwatch.StartNew();
                var isStudentAdded = await _studentsPage.VerifyStudentAdded(student.Name, student.Id);
                verifyStopwatch.Stop();
                System.Console.WriteLine($"Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                
                operationStopwatch.Stop();
                System.Console.WriteLine($"Total time to add student: {operationStopwatch.ElapsedMilliseconds}ms");
                
                Assert.That(isStudentAdded, Is.True, $"Student {student.Name} with ID {student.Id} was not added successfully");

                // Step 3: Try to add another student with the same ID
                System.Console.WriteLine("\n=== ATTEMPTING TO ADD DUPLICATE STUDENT ===");
                var duplicateStudent = (Name: "Another Student", Id: "DUP123", Email: "another@example.com", Phone: "555-987-6543");
                
                // Time the duplicate student addition process
                operationStopwatch.Restart();
                
                // Click Add New Student button
                System.Console.WriteLine($"Clicking Add New Student button...");
                buttonStopwatch = Stopwatch.StartNew();
                await _studentsPage.ClickAddNewStudentButton();
                buttonStopwatch.Stop();
                System.Console.WriteLine($"Button click completed in {buttonStopwatch.ElapsedMilliseconds}ms");
                
                // Fill student form
                System.Console.WriteLine($"Filling student form with duplicate ID...");
                formStopwatch = Stopwatch.StartNew();
                await _studentsPage.FillStudentForm(
                    name: duplicateStudent.Name,
                    studentId: duplicateStudent.Id,
                    email: duplicateStudent.Email,
                    phone: duplicateStudent.Phone
                );
                formStopwatch.Stop();
                System.Console.WriteLine($"Form filling completed in {formStopwatch.ElapsedMilliseconds}ms");
                
                // Submit student form
                System.Console.WriteLine($"Submitting student form with duplicate ID...");
                submitStopwatch = Stopwatch.StartNew();
                await _studentsPage.SubmitStudentForm();
                submitStopwatch.Stop();
                System.Console.WriteLine($"Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                
                // Wait for error message or check that the student was not added
                await Task.Delay(1000); // Wait for any error messages to appear
                
                // Verify the duplicate student was not added
                System.Console.WriteLine($"Verifying duplicate student was not added...");
                verifyStopwatch = Stopwatch.StartNew();
                
                // Count the number of rows with the duplicate ID
                var duplicateRows = Page!.Locator("tr", new() { HasText = duplicateStudent.Id });
                var duplicateRowCount = await duplicateRows.CountAsync();
                
                // Count the number of rows with the duplicate student name
                var duplicateNameRows = Page!.Locator("tr", new() { HasText = duplicateStudent.Name });
                var duplicateNameRowCount = await duplicateNameRows.CountAsync();
                
                verifyStopwatch.Stop();
                System.Console.WriteLine($"Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                
                operationStopwatch.Stop();
                System.Console.WriteLine($"Total time for duplicate check: {operationStopwatch.ElapsedMilliseconds}ms");
                
                // There should be only one row with the duplicate ID (the original student)
                Assert.That(duplicateRowCount, Is.EqualTo(1), "There should be exactly one student with the duplicate ID");
                
                // There should be no rows with the duplicate student name
                Assert.That(duplicateNameRowCount, Is.EqualTo(0), "The duplicate student should not have been added");
                
                // Take a final screenshot
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "duplicate-check-test.png" });
                System.Console.WriteLine("Final screenshot saved to duplicate-check-test.png");
                
                // Report total test time
                totalStopwatch.Stop();
                System.Console.WriteLine($"\n=== TEST COMPLETED ===");
                System.Console.WriteLine($"Total test execution time: {totalStopwatch.ElapsedMilliseconds}ms");
            }
            catch (System.Exception ex)
            {
                // Take a screenshot on error
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "duplicate-check-error.png" });
                System.Console.WriteLine($"Error screenshot saved to duplicate-check-error.png");
                System.Console.WriteLine($"Error: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        [Test]
        public async Task TeacherDuplicateIdCheckTest()
        {
            // Create a stopwatch for timing operations
            var totalStopwatch = Stopwatch.StartNew();
            var operationStopwatch = new Stopwatch();
            
            try
            {
                System.Console.WriteLine("=== PERFORMANCE DEBUGGING ===");
                System.Console.WriteLine("Starting duplicate teacher ID check test");
                
                // Step 1: Navigate to Teachers page
                System.Console.WriteLine("\n=== NAVIGATING TO TEACHERS PAGE ===");
                var navStopwatch = Stopwatch.StartNew();
                await _teachersPage.NavigateToTeachersPage();
                navStopwatch.Stop();
                System.Console.WriteLine($"Navigation completed in {navStopwatch.ElapsedMilliseconds}ms");

                // Step 2: Add a teacher
                System.Console.WriteLine("\n=== ADDING FIRST TEACHER ===");
                var teacher = (Name: "Duplicate Test Teacher", Id: "TDUP123", Email: "teacher.duplicate@example.com", Phone: "555-123-4567", Subject: "Mathematics", Address: "123 School St, Anytown, CA 12345");
                
                // Time the entire teacher addition process
                operationStopwatch.Restart();
                
                // Click Add New Teacher button
                System.Console.WriteLine($"Clicking Add New Teacher button...");
                var buttonStopwatch = Stopwatch.StartNew();
                await _teachersPage.ClickAddNewTeacherButton();
                buttonStopwatch.Stop();
                System.Console.WriteLine($"Button click completed in {buttonStopwatch.ElapsedMilliseconds}ms");
                
                // Fill teacher form
                System.Console.WriteLine($"Filling teacher form...");
                var formStopwatch = Stopwatch.StartNew();
                await _teachersPage.FillTeacherForm(
                    name: teacher.Name,
                    teacherId: teacher.Id,
                    email: teacher.Email,
                    phone: teacher.Phone,
                    subject: teacher.Subject,
                    address: teacher.Address
                );
                formStopwatch.Stop();
                System.Console.WriteLine($"Form filling completed in {formStopwatch.ElapsedMilliseconds}ms");
                
                // Submit teacher form
                System.Console.WriteLine($"Submitting teacher form...");
                var submitStopwatch = Stopwatch.StartNew();
                await _teachersPage.SubmitTeacherForm();
                submitStopwatch.Stop();
                System.Console.WriteLine($"Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                
                // Verify teacher was added
                System.Console.WriteLine($"Verifying teacher was added...");
                var verifyStopwatch = Stopwatch.StartNew();
                var isTeacherAdded = await _teachersPage.VerifyTeacherAdded(teacher.Name, teacher.Id);
                verifyStopwatch.Stop();
                System.Console.WriteLine($"Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                
                operationStopwatch.Stop();
                System.Console.WriteLine($"Total time to add teacher: {operationStopwatch.ElapsedMilliseconds}ms");
                
                Assert.That(isTeacherAdded, Is.True, $"Teacher {teacher.Name} with ID {teacher.Id} was not added successfully");

                // Step 3: Try to add another teacher with the same ID
                System.Console.WriteLine("\n=== ATTEMPTING TO ADD DUPLICATE TEACHER ===");
                var duplicateTeacher = (Name: "Another Teacher", Id: "TDUP123", Email: "another.teacher@example.com", Phone: "555-987-6543", Subject: "Science", Address: "456 School St, Anytown, CA 12345");
                
                // Time the duplicate teacher addition process
                operationStopwatch.Restart();
                
                // Click Add New Teacher button
                System.Console.WriteLine($"Clicking Add New Teacher button...");
                buttonStopwatch = Stopwatch.StartNew();
                await _teachersPage.ClickAddNewTeacherButton();
                buttonStopwatch.Stop();
                System.Console.WriteLine($"Button click completed in {buttonStopwatch.ElapsedMilliseconds}ms");
                
                // Fill teacher form
                System.Console.WriteLine($"Filling teacher form with duplicate ID...");
                formStopwatch = Stopwatch.StartNew();
                await _teachersPage.FillTeacherForm(
                    name: duplicateTeacher.Name,
                    teacherId: duplicateTeacher.Id,
                    email: duplicateTeacher.Email,
                    phone: duplicateTeacher.Phone,
                    subject: duplicateTeacher.Subject,
                    address: duplicateTeacher.Address
                );
                formStopwatch.Stop();
                System.Console.WriteLine($"Form filling completed in {formStopwatch.ElapsedMilliseconds}ms");
                
                // Submit teacher form
                System.Console.WriteLine($"Submitting teacher form with duplicate ID...");
                submitStopwatch = Stopwatch.StartNew();
                await _teachersPage.SubmitTeacherForm();
                submitStopwatch.Stop();
                System.Console.WriteLine($"Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                
                // Wait for error message or check that the teacher was not added
                await Task.Delay(1000); // Wait for any error messages to appear
                
                // Verify the duplicate teacher was not added
                System.Console.WriteLine($"Verifying duplicate teacher was not added...");
                verifyStopwatch = Stopwatch.StartNew();
                
                // Count the number of rows with the duplicate ID
                var duplicateRows = Page!.Locator("tr", new() { HasText = duplicateTeacher.Id });
                var duplicateRowCount = await duplicateRows.CountAsync();
                
                // Count the number of rows with the duplicate teacher name
                var duplicateNameRows = Page!.Locator("tr", new() { HasText = duplicateTeacher.Name });
                var duplicateNameRowCount = await duplicateNameRows.CountAsync();
                
                verifyStopwatch.Stop();
                System.Console.WriteLine($"Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                
                operationStopwatch.Stop();
                System.Console.WriteLine($"Total time for duplicate check: {operationStopwatch.ElapsedMilliseconds}ms");
                
                // There should be only one row with the duplicate ID (the original teacher)
                Assert.That(duplicateRowCount, Is.EqualTo(1), "There should be exactly one teacher with the duplicate ID");
                
                // There should be no rows with the duplicate teacher name
                Assert.That(duplicateNameRowCount, Is.EqualTo(0), "The duplicate teacher should not have been added");
                
                // Take a final screenshot
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "duplicate-teacher-check-test.png" });
                System.Console.WriteLine("Final screenshot saved to duplicate-teacher-check-test.png");
                
                // Report total test time
                totalStopwatch.Stop();
                System.Console.WriteLine($"\n=== TEST COMPLETED ===");
                System.Console.WriteLine($"Total test execution time: {totalStopwatch.ElapsedMilliseconds}ms");
            }
            catch (System.Exception ex)
            {
                // Take a screenshot on error
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "duplicate-teacher-check-error.png" });
                System.Console.WriteLine($"Error screenshot saved to duplicate-teacher-check-error.png");
                System.Console.WriteLine($"Error: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
