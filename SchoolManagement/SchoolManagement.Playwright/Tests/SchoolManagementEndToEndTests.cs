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
    public class SchoolManagementEndToEndTests : BaseTest
    {
        private StudentsPage _studentsPage = null!;
        private TeachersPage _teachersPage = null!;
        private ClassesPage _classesPage = null!;

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
            
            // Print the page title for debugging
            var titleSw = Stopwatch.StartNew();
            var pageTitle = await Page.TitleAsync();
            titleSw.Stop();
            System.Console.WriteLine($"Page title: {pageTitle} (retrieved in {titleSw.ElapsedMilliseconds}ms)");
            
            // Take a screenshot for debugging
            System.Console.WriteLine("Taking screenshot...");
            var screenshotSw = Stopwatch.StartNew();
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "homepage.png" });
            screenshotSw.Stop();
            System.Console.WriteLine($"Screenshot saved to homepage.png in {screenshotSw.ElapsedMilliseconds}ms");
            
            // Initialize page objects
            System.Console.WriteLine("Initializing page objects...");
            var initSw = Stopwatch.StartNew();
            _studentsPage = new StudentsPage(Page);
            _teachersPage = new TeachersPage(Page);
            _classesPage = new ClassesPage(Page);
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
        public async Task SchoolManagementEndToEndTest()
        {
            // Create a stopwatch for timing operations
            var totalStopwatch = Stopwatch.StartNew();
            var operationStopwatch = new Stopwatch();
            
            try
            {
                System.Console.WriteLine("=== PERFORMANCE DEBUGGING ===");
                System.Console.WriteLine("Starting end-to-end test");
                // Step 1: Add 3 students
                System.Console.WriteLine("\n=== ADDING STUDENTS ===");

                // Navigate to Students page
                System.Console.WriteLine($"  Navigating to Students page...");
                var navStopwatch = Stopwatch.StartNew();
                await _studentsPage.NavigateToStudentsPage();
                navStopwatch.Stop();
                System.Console.WriteLine($"  Navigation completed in {navStopwatch.ElapsedMilliseconds}ms");

                for (int i = 0; i < TestData.Students.Length; i++)
                {
                    var student = TestData.Students[i];
                    System.Console.WriteLine($"Adding student {i+1}/{TestData.Students.Length}: {student.Name} ({student.Id})");
                    
                    // For students after the first one, reload the page to ensure a clean state
                    // This helps avoid timing issues with the UI
                    if (i > 0)
                    {
                        // Add a small delay to ensure the page is fully loaded and ready
                        System.Console.WriteLine($"  Waiting 500ms before adding next student...");
                        await Task.Delay(500);
                    }
                    
                    // Time the entire student addition process
                    operationStopwatch.Restart();
                    
                    
                    // Click Add New Student button
                    System.Console.WriteLine($"  Clicking Add New Student button...");
                    var buttonStopwatch = Stopwatch.StartNew();
                    await _studentsPage.ClickAddNewStudentButton();
                    buttonStopwatch.Stop();
                    System.Console.WriteLine($"  Button click completed in {buttonStopwatch.ElapsedMilliseconds}ms");
                    
                    // Fill student form
                    System.Console.WriteLine($"  Filling student form...");
                    var formStopwatch = Stopwatch.StartNew();
                    await _studentsPage.FillStudentForm(
                        name: student.Name,
                        studentId: student.Id,
                        email: student.Email,
                        phone: student.Phone
                    );
                    formStopwatch.Stop();
                    System.Console.WriteLine($"  Form filling completed in {formStopwatch.ElapsedMilliseconds}ms");
                    
                    // Submit student form
                    System.Console.WriteLine($"  Submitting student form...");
                    var submitStopwatch = Stopwatch.StartNew();
                    await _studentsPage.SubmitStudentForm();
                    submitStopwatch.Stop();
                    System.Console.WriteLine($"  Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                    
                    // Verify student was added
                    System.Console.WriteLine($"  Verifying student was added...");
                    var verifyStopwatch = Stopwatch.StartNew();
                    var isStudentAdded = await _studentsPage.VerifyStudentAdded(student.Name, student.Id);
                    verifyStopwatch.Stop();
                    System.Console.WriteLine($"  Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                    
                    operationStopwatch.Stop();
                    System.Console.WriteLine($"  Total time to add student: {operationStopwatch.ElapsedMilliseconds}ms");
                    
                    Assert.That(isStudentAdded, Is.True, $"Student {student.Name} with ID {student.Id} was not added successfully");
                }
                // click esc key 
                await Page!.Keyboard.PressAsync("Escape");
                await Task.Delay(500);
                // Step 2: Add 3 teachers
                System.Console.WriteLine("\n=== ADDING TEACHERS ===");
                // Navigate to Teachers page
                System.Console.WriteLine($"  Navigating to Teachers page...");
                navStopwatch = Stopwatch.StartNew();
                await _teachersPage.NavigateToTeachersPage();
                navStopwatch.Stop();
                System.Console.WriteLine($"  Navigation completed in {navStopwatch.ElapsedMilliseconds}ms");
                // wait 500ms to ensure the page is fully loaded and ready
                System.Console.WriteLine($"  Waiting 500ms before adding teachers...");
                await Task.Delay(500);
                
                for (int i = 0; i < TestData.Teachers.Length; i++)
                {
                    var teacher = TestData.Teachers[i];
                    System.Console.WriteLine($"Adding teacher {i+1}/{TestData.Teachers.Length}: {teacher.Name} ({teacher.Id})");
                    
                    // For teachers after the first one, reload the page to ensure a clean state
                    if (i > 0)
                    {
                        System.Console.WriteLine($"  Reloading page before adding next teacher...");
                        var reloadStopwatch = Stopwatch.StartNew();
                        await Page!.ReloadAsync();
                        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                        reloadStopwatch.Stop();
                        System.Console.WriteLine($"  Page reload completed in {reloadStopwatch.ElapsedMilliseconds}ms");
                        
                        // Add a small delay to ensure the page is fully loaded and ready
                        System.Console.WriteLine($"  Waiting 1000ms after reload...");
                        await Task.Delay(1000);
                    }
                    
                    // Time the entire teacher addition process
                    operationStopwatch.Restart();
                    
                    
                    // Click Add New Teacher button
                    System.Console.WriteLine($"  Clicking Add New Teacher button...");
                    var buttonStopwatch = Stopwatch.StartNew();
                    await _teachersPage.ClickAddNewTeacherButton();
                    buttonStopwatch.Stop();
                    System.Console.WriteLine($"  Button click completed in {buttonStopwatch.ElapsedMilliseconds}ms");
                    
                    // Fill teacher form
                    System.Console.WriteLine($"  Filling teacher form...");
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
                    System.Console.WriteLine($"  Form filling completed in {formStopwatch.ElapsedMilliseconds}ms");
                    
                    // Submit teacher form
                    System.Console.WriteLine($"  Submitting teacher form...");
                    var submitStopwatch = Stopwatch.StartNew();
                    await _teachersPage.SubmitTeacherForm();
                    submitStopwatch.Stop();
                    System.Console.WriteLine($"  Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                    
                    // Verify teacher was added
                    System.Console.WriteLine($"  Verifying teacher was added...");
                    var verifyStopwatch = Stopwatch.StartNew();
                    var isTeacherAdded = await _teachersPage.VerifyTeacherAdded(teacher.Name, teacher.Id);
                    verifyStopwatch.Stop();
                    System.Console.WriteLine($"  Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                    
                    operationStopwatch.Stop();
                    System.Console.WriteLine($"  Total time to add teacher: {operationStopwatch.ElapsedMilliseconds}ms");
                    
                    Assert.That(isTeacherAdded, Is.True, $"Teacher {teacher.Name} with ID {teacher.Id} was not added successfully");
                }

                // click esc key 
                await Page!.Keyboard.PressAsync("Escape");
                await Task.Delay(500);
                System.Console.WriteLine($"  Navigating to Classes page...");
                navStopwatch = Stopwatch.StartNew();
                await _classesPage.NavigateToClassesPage();
                await Task.Delay(500);
                navStopwatch.Stop();

                // Step 3: Add 2 classes
                System.Console.WriteLine("\n=== ADDING CLASSES ===");
                for (int i = 0; i < TestData.Classes.Length; i++)
                {
                    var classData = TestData.Classes[i];
                    System.Console.WriteLine($"Adding class {i+1}/{TestData.Classes.Length}: {classData.Name} ({classData.Code})");
                    
                    // For classes after the first one, reload the page to ensure a clean state
                    if (i > 0)
                    {
                        System.Console.WriteLine($"  Reloading page before adding next class...");
                        var reloadStopwatch = Stopwatch.StartNew();
                        await Page!.ReloadAsync();
                        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                        reloadStopwatch.Stop();
                        System.Console.WriteLine($"  Page reload completed in {reloadStopwatch.ElapsedMilliseconds}ms");
                        
                        // Add a small delay to ensure the page is fully loaded and ready
                        System.Console.WriteLine($"  Waiting 1000ms after reload...");
                        await Task.Delay(1000);
                    }
                    
                    // Time the entire class addition process
                    operationStopwatch.Restart();
                    
                    System.Console.WriteLine($"  Starting class addition process...");
                    var addClassSw = Stopwatch.StartNew();
                    await _classesPage.AddClass(
                        name: classData.Name,
                        classCode: classData.Code,
                        description: classData.Description
                    );
                    addClassSw.Stop();
                    System.Console.WriteLine($"  Class addition process completed in {addClassSw.ElapsedMilliseconds}ms");
                    
                    // Verify class was added
                    System.Console.WriteLine($"  Verifying class was added...");
                    var verifyStopwatch = Stopwatch.StartNew();
                    var isClassAdded = await _classesPage.VerifyClassAdded(classData.Name, classData.Code);
                    verifyStopwatch.Stop();
                    System.Console.WriteLine($"  Verification completed in {verifyStopwatch.ElapsedMilliseconds}ms");
                    
                    operationStopwatch.Stop();
                    System.Console.WriteLine($"  Total time to add class: {operationStopwatch.ElapsedMilliseconds}ms");
                    
                    Assert.That(isClassAdded, Is.True, $"Class {classData.Name} with code {classData.Code} was not added successfully");
                }
                
                // Step 4: Assign teachers to classes
                System.Console.WriteLine("\n=== ASSIGNING TEACHERS TO CLASSES ===");
                // Navigate to Classes page
                // click esc key 
                await Page!.Keyboard.PressAsync("Escape");
                await Task.Delay(500);
                System.Console.WriteLine($"  Navigating to Classes page...");
                navStopwatch = Stopwatch.StartNew();
                await _classesPage.NavigateToClassesPage();
                await Task.Delay(500);
                navStopwatch.Stop();
                System.Console.WriteLine($"  Navigation completed in {navStopwatch.ElapsedMilliseconds}ms");
                for (int i = 0; i < TestData.Classes.Length; i++)
                {
                    var classData = TestData.Classes[i];
                    var teacher = TestData.Teachers[i]; // Assign each teacher to a different class
                    System.Console.WriteLine($"Assigning teacher {teacher.Name} to class {classData.Name}");
                    
                    // Time the entire teacher assignment process
                    operationStopwatch.Restart();
                    try
                    {
                        // Wait for the table to be visible
                        System.Console.WriteLine($"  Waiting for table to be visible...");
                        var tableStopwatch = Stopwatch.StartNew();
                        await Page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 30000 });
                        tableStopwatch.Stop();
                        System.Console.WriteLine($"  Table became visible in {tableStopwatch.ElapsedMilliseconds}ms");
                        
                        // Wait for the specific class row to be visible
                        System.Console.WriteLine($"  Waiting for class row to be visible...");
                        var rowStopwatch = Stopwatch.StartNew();
                        var classRow = Page.Locator("tr", new() { HasText = classData.Name });
                        await classRow.WaitForAsync(new() { Timeout = 30000 });
                        rowStopwatch.Stop();
                        System.Console.WriteLine($"  Class row became visible in {rowStopwatch.ElapsedMilliseconds}ms");
                        
                        // Wait for the Assign Teacher button to be visible
                        System.Console.WriteLine($"  Waiting for Manage button to be visible...");
                        var buttonStopwatch = Stopwatch.StartNew();
                        var assignButton = classRow.Locator("button:has-text('Manage')");
                        await assignButton.WaitForAsync(new() { Timeout = 30000 });
                        await assignButton.ClickAsync(new LocatorClickOptions { Force = true });
                        buttonStopwatch.Stop();
                        System.Console.WriteLine($"  Manage button became visible in {buttonStopwatch.ElapsedMilliseconds}ms");
                        
                        // Now proceed with the assignment
                        System.Console.WriteLine($"  Starting teacher assignment process...");
                        var assignStopwatch = Stopwatch.StartNew();
                        await _classesPage.AssignTeacherToClass(
                            index: i + 1,
                            className: classData.Name
                        );
                        assignStopwatch.Stop();
                        System.Console.WriteLine($"  Teacher assignment process completed in {assignStopwatch.ElapsedMilliseconds}ms");
                        
                        operationStopwatch.Stop();
                        System.Console.WriteLine($"  Total time to assign teacher: {operationStopwatch.ElapsedMilliseconds}ms");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error assigning teacher {teacher.Name} to class {classData.Name}: {ex.Message}");
                        // Take a screenshot for debugging
                        await Page.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-assign-teacher-{i}.png" });
                        throw;
                    }
                }
                
                // Step 5: Assign students to classes
                // For each class, assign 2 students
                System.Console.WriteLine("\n=== ASSIGNING STUDENTS TO CLASSES ===");
                // Navigate to Classes page
                System.Console.WriteLine($"  Navigating to Classes page...");
                navStopwatch = Stopwatch.StartNew();
                await _classesPage.NavigateToClassesPage();
                navStopwatch.Stop();
                System.Console.WriteLine($"  Navigation completed in {navStopwatch.ElapsedMilliseconds}ms");

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
                        
                        // Always reload the page before assigning a student to ensure a clean state
                        System.Console.WriteLine($"  Reloading page before student assignment...");
                        var reloadStopwatch = Stopwatch.StartNew();
                        await Page!.ReloadAsync();
                        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                        reloadStopwatch.Stop();
                        System.Console.WriteLine($"  Page reload completed in {reloadStopwatch.ElapsedMilliseconds}ms");
                        
                        // Add a longer delay to ensure the page is fully loaded and ready
                        System.Console.WriteLine($"  Waiting 2000ms after reload...");
                        await Task.Delay(2000);
                        
                        // Time the entire student assignment process
                        operationStopwatch.Restart();
                        
                        try
                        {
                            
                            // Wait for the table to be visible
                            System.Console.WriteLine($"  Waiting for table to be visible...");
                            var tableStopwatch = Stopwatch.StartNew();
                            await Page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 30000 });
                            tableStopwatch.Stop();
                            System.Console.WriteLine($"  Table became visible in {tableStopwatch.ElapsedMilliseconds}ms");
                            
                            // Wait for the specific class row to be visible
                            System.Console.WriteLine($"  Waiting for class row to be visible...");
                            var rowStopwatch = Stopwatch.StartNew();
                            var classRow = Page.Locator("tr", new() { HasText = classData.Name });
                            await classRow.WaitForAsync(new() { Timeout = 30000 });
                            rowStopwatch.Stop();
                            System.Console.WriteLine($"  Class row became visible in {rowStopwatch.ElapsedMilliseconds}ms");
                            
                            // Wait for the Assign Student button to be visible
                            System.Console.WriteLine($"  Waiting for Assign Student button to be visible...");
                            var buttonStopwatch = Stopwatch.StartNew();
                            var assignButton = classRow.Locator("button:has-text('Assign Student')");
                            await assignButton.WaitForAsync(new() { Timeout = 30000 });
                            buttonStopwatch.Stop();
                            System.Console.WriteLine($"  Assign Student button became visible in {buttonStopwatch.ElapsedMilliseconds}ms");
                            
                            // Click the Assign Student button
                            System.Console.WriteLine($"  Clicking Assign Student button...");
                            var clickStopwatch = Stopwatch.StartNew();
                            await assignButton.ClickAsync(new LocatorClickOptions { Force = true });
                            clickStopwatch.Stop();
                            System.Console.WriteLine($"  Assign Student button clicked in {clickStopwatch.ElapsedMilliseconds}ms");
                            
                            // Wait for the modal to appear
                            System.Console.WriteLine($"  Waiting for Assign Student modal to appear...");
                            var modalStopwatch = Stopwatch.StartNew();
                            await Page.WaitForSelectorAsync("div.modal-header:has-text('Assign Student')", new PageWaitForSelectorOptions { Timeout = 30000 });
                            modalStopwatch.Stop();
                            System.Console.WriteLine($"  Assign Student modal appeared in {modalStopwatch.ElapsedMilliseconds}ms");
                            
                            // Select student from dropdown
                            System.Console.WriteLine($"  Selecting student from dropdown...");
                            var selectStopwatch = Stopwatch.StartNew();
                            await _classesPage.SelectStudentFromDropdown(student.Name);
                            selectStopwatch.Stop();
                            System.Console.WriteLine($"  Student selection completed in {selectStopwatch.ElapsedMilliseconds}ms");
                            
                            // Submit the form
                            System.Console.WriteLine($"  Submitting assign student form...");
                            var submitStopwatch = Stopwatch.StartNew();
                            await _classesPage.SubmitAssignStudentForm();
                            submitStopwatch.Stop();
                            System.Console.WriteLine($"  Form submission completed in {submitStopwatch.ElapsedMilliseconds}ms");
                            
                            operationStopwatch.Stop();
                            System.Console.WriteLine($"  Total time to assign student: {operationStopwatch.ElapsedMilliseconds}ms");
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine($"Error assigning student {student.Name} to class {classData.Name}: {ex.Message}");
                            // Take a screenshot for debugging
                            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-assign-student-{i}-{j}.png" });
                            throw;
                        }
                    }
                }
                
                // Take a final screenshot
                await Page!.ScreenshotAsync(new PageScreenshotOptions { Path = "final-state.png" });
                System.Console.WriteLine("Final screenshot saved to final-state.png");
                
                // Report total test time
                totalStopwatch.Stop();
                System.Console.WriteLine($"\n=== TEST COMPLETED ===");
                System.Console.WriteLine($"Total test execution time: {totalStopwatch.ElapsedMilliseconds}ms");
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
