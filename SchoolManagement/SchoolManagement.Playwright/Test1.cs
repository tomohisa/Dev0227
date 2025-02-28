﻿using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright;

[TestFixture] 
public partial class SchoolManagementTests
{
    private const string BaseUrl = "https://localhost:7201";
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    // Student data
    private readonly (string Name, string Id, string Email, string Phone)[] _students = new[]
    {
        ("John Smith", "S12345", "john.smith@example.com", "555-123-4567"),
        ("Jane Doe", "S67890", "jane.doe@example.com", "555-987-6543"),
        ("Michael Johnson", "S24680", "michael.johnson@example.com", "555-246-8024")
    };

    // Teacher data
    private readonly (string Name, string Id, string Email, string Phone, string Subject)[] _teachers = new[]
    {
        ("Dr. Robert Brown", "T12345", "robert.brown@example.com", "555-111-2222", "Mathematics"),
        ("Prof. Sarah Wilson", "T67890", "sarah.wilson@example.com", "555-333-4444", "Science"),
        ("Ms. Emily Davis", "T24680", "emily.davis@example.com", "555-555-6666", "English")
    };

    // Class data
    private readonly (string Name, string Code, string Description)[] _classes = new[]
    {
        ("Mathematics 101", "MATH101", "An introductory course to basic mathematics concepts and principles."),
        ("Science 101", "SCI101", "An introductory course to basic science concepts and principles.")
    };

    [SetUp]
    public async Task SetUp()
    {
        // Create Playwright instance
        var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        // Launch browser with increased timeout and viewport size
        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false, // Set to false for debugging
            Timeout = 60000 // 60 seconds
        });
        
        // Create a new browser context with larger viewport
        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize
            {
                Width = 1280,
                Height = 800
            }
        });
        
        // Create a new page
        _page = await _context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        // Close browser
        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }
    }

    [Test]
    public async Task SchoolManagementEndToEndTest()
    {
        try
        {
            // Navigate to the application
            await _page!.GotoAsync(BaseUrl);
            
            // Wait for the page to load
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Print the page title for debugging
            System.Console.WriteLine($"Page title: {await _page.TitleAsync()}");
            
            // Take a screenshot for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "homepage.png" });
            System.Console.WriteLine("Screenshot saved to homepage.png");
            
            // Print the HTML content for debugging
            var htmlContent = await _page.ContentAsync();
            System.Console.WriteLine($"HTML content: {htmlContent.Substring(0, 500)}...");
            
            // Verify the page has loaded by checking for the presence of navigation elements
            // Use a more specific selector for the Students link in the navigation menu
            var studentsNavLink = _page.Locator("nav a.nav-link", new() { HasText = "Students" });
            
            // Wait for the navigation link to be visible
            await studentsNavLink.WaitForAsync(new() { Timeout = 10000 });
            Assert.That(await studentsNavLink.IsVisibleAsync(), Is.True, "Students navigation link is not visible");
            
            // Step 1: Add 3 students
            for (int i = 0; i < _students.Length; i++)
            {
                var student = _students[i];
                System.Console.WriteLine($"Adding student {i+1}/{_students.Length}: {student.Name} ({student.Id})");
                
                // Reload the page to ensure we have a clean state
                if (i > 0)
                {
                    await _page.ReloadAsync();
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                }
                
                await AddStudent(
                    name: student.Name,
                    studentId: student.Id,
                    email: student.Email,
                    phone: student.Phone
                );
                
                // Verify student was added by checking for the student name and ID in the table
                var studentRows = _page.Locator("tr", new() { HasText = student.Name }).Filter(new() { HasText = student.Id });
                var studentRowCount = await studentRows.CountAsync();
                System.Console.WriteLine($"Found {studentRowCount} rows containing '{student.Name}' and '{student.Id}'");
                
                // Verify at least one row exists with the student name and ID
                Assert.That(studentRowCount, Is.GreaterThan(0), $"Student {student.Name} with ID {student.Id} was not added successfully");
            }
            
            // Step 2: Add 3 teachers
            for (int i = 0; i < _teachers.Length; i++)
            {
                var teacher = _teachers[i];
                System.Console.WriteLine($"Adding teacher {i+1}/{_teachers.Length}: {teacher.Name} ({teacher.Id})");
                
                // Reload the page to ensure we have a clean state
                await _page.ReloadAsync();
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                
                await AddTeacher(
                    name: teacher.Name,
                    teacherId: teacher.Id,
                    email: teacher.Email,
                    phone: teacher.Phone,
                    subject: teacher.Subject
                );
                
                // Verify teacher was added by checking for the teacher name and ID in the table
                var teacherRows = _page.Locator("tr", new() { HasText = teacher.Name }).Filter(new() { HasText = teacher.Id });
                var teacherRowCount = await teacherRows.CountAsync();
                System.Console.WriteLine($"Found {teacherRowCount} rows containing '{teacher.Name}' and '{teacher.Id}'");
                
                // Verify at least one row exists with the teacher name and ID
                Assert.That(teacherRowCount, Is.GreaterThan(0), $"Teacher {teacher.Name} with ID {teacher.Id} was not added successfully");
            }
            
            // Step 3: Add 2 classes
            for (int i = 0; i < _classes.Length; i++)
            {
                var classData = _classes[i];
                System.Console.WriteLine($"Adding class {i+1}/{_classes.Length}: {classData.Name} ({classData.Code})");
                
                // Reload the page to ensure we have a clean state
                await _page.ReloadAsync();
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                
                await AddClass(
                    name: classData.Name,
                    classCode: classData.Code,
                    description: classData.Description
                );
                
                // Verify class was added by checking for the class name and code in the table
                var classRows = _page.Locator("tr", new() { HasText = classData.Name }).Filter(new() { HasText = classData.Code });
                var classRowCount = await classRows.CountAsync();
                System.Console.WriteLine($"Found {classRowCount} rows containing '{classData.Name}' and '{classData.Code}'");
                
                // Verify at least one row exists with the class name and code
                Assert.That(classRowCount, Is.GreaterThan(0), $"Class {classData.Name} with code {classData.Code} was not added successfully");
            }
            
            // Step 4: Assign teachers to classes
            for (int i = 0; i < _classes.Length; i++)
            {
                var classData = _classes[i];
                var teacher = _teachers[i]; // Assign each teacher to a different class
                System.Console.WriteLine($"Assigning teacher {teacher.Name} to class {classData.Name}");
                
                // Reload the page to ensure we have a clean state
                await _page.ReloadAsync();
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                
                await AssignTeacherToClass(
                    teacherName: teacher.Name,
                    className: classData.Name
                );
                
                // Verify teacher was assigned to the class
                // This will depend on how the UI shows the assignment
                // For now, we'll just check if the operation completed without errors
            }
            
            // Step 5: Assign students to classes
            // For each class, assign 2 students
            for (int i = 0; i < _classes.Length; i++)
            {
                var classData = _classes[i];
                
                // Assign 2 students to each class
                for (int j = 0; j < 2; j++)
                {
                    var student = _students[j + i]; // Distribute students across classes
                    if (j + i >= _students.Length)
                    {
                        student = _students[j]; // Wrap around if we run out of students
                    }
                    
                    System.Console.WriteLine($"Assigning student {student.Name} to class {classData.Name}");
                    
                    // Reload the page to ensure we have a clean state
                    await _page.ReloadAsync();
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    
                    await AssignStudentToClass(
                        studentName: student.Name,
                        className: classData.Name
                    );
                    
                    // Verify student was assigned to the class
                    // This will depend on how the UI shows the assignment
                    // For now, we'll just check if the operation completed without errors
                }
            }
            
            // Take a final screenshot
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "final-state.png" });
            System.Console.WriteLine("Final screenshot saved to final-state.png");
        }
        catch (System.Exception ex)
        {
            // Take a screenshot on error
            await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = "error.png" });
            System.Console.WriteLine($"Error screenshot saved to error.png");
            System.Console.WriteLine($"Error: {ex.Message}");
            System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
    
    private async Task CloseAnyOpenModals()
    {
        try
        {
            // Check if there are any open modals
            var modalElements = _page!.Locator(".modal.show");
            var count = await modalElements.CountAsync();
            
            if (count > 0)
            {
                System.Console.WriteLine($"Found {count} open modals, attempting to close them");
                
                // Try to close modals using the close button
                var closeButtons = _page.Locator(".modal.show .btn-close");
                var buttonCount = await closeButtons.CountAsync();
                
                if (buttonCount > 0)
                {
                    System.Console.WriteLine($"Found {buttonCount} close buttons");
                    
                    // Click all close buttons
                    for (int i = 0; i < buttonCount; i++)
                    {
                        var closeButton = closeButtons.Nth(i);
                        if (await closeButton.IsVisibleAsync())
                        {
                            await closeButton.ClickAsync(new LocatorClickOptions { Force = true });
                            System.Console.WriteLine($"Clicked close button {i+1}");
                            
                            // Wait a bit for the modal to close
                            await Task.Delay(500);
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("No close buttons found, trying to click outside the modal");
                    
                    // Try clicking outside the modal
                    await _page.Mouse.ClickAsync(10, 10);
                    await Task.Delay(500);
                }
                
                // Check if modals are still open
                count = await modalElements.CountAsync();
                if (count > 0)
                {
                    System.Console.WriteLine($"Still have {count} open modals, trying to press Escape key");
                    
                    // Try pressing Escape key
                    await _page.Keyboard.PressAsync("Escape");
                    await Task.Delay(500);
                    
                    // Check again
                    count = await modalElements.CountAsync();
                    if (count > 0)
                    {
                        System.Console.WriteLine($"Still have {count} open modals, will try to reload the page");
                    }
                    else
                    {
                        System.Console.WriteLine("Successfully closed all modals");
                    }
                }
                else
                {
                    System.Console.WriteLine("Successfully closed all modals");
                }
            }
            else
            {
                System.Console.WriteLine("No open modals found");
            }
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine($"Error closing modals: {ex.Message}");
        }
    }
    
    private async Task AddTeacher(string name, string teacherId, string email, string phone, string subject)
    {
        try
        {
            // Make sure no modals are open
            await CloseAnyOpenModals();
            
            // Navigate to Teachers page if not already there
            // Use a more specific selector for the Teachers link in the navigation menu
            var teachersLink = _page!.Locator("nav a.nav-link", new() { HasText = "Teachers" });
            
            // Use force option to bypass any intercepting elements
            await teachersLink.ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the page to load
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Take a screenshot for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"teachers-page-before-{teacherId}.png" });
            
            // Check if the page has a table already
            var tableExists = await _page.Locator("table").IsVisibleAsync();
            System.Console.WriteLine($"Table exists before adding teacher: {tableExists}");
            
            // Click "Add New Teacher" button with force option
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Teacher" }).ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the modal to appear with a specific title
            // Use a more specific selector for the modal dialog
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Teacher')");
            
            // Take a screenshot of the modal
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-teacher-modal-{teacherId}.png" });
            
            // Use a more specific selector for the modal dialog
            var addTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Teacher'))");
            
            // Print the HTML content of the modal for debugging
            var modalContent = await addTeacherModal.InnerHTMLAsync();
            System.Console.WriteLine($"Modal content for teacher {teacherId}: {modalContent.Substring(0, Math.Min(500, modalContent.Length))}...");
            
            // Find all input elements in the modal and print their IDs
            var inputs = addTeacherModal.Locator("input");
            var count = await inputs.CountAsync();
            System.Console.WriteLine($"Found {count} input elements in the modal");
            
            for (int i = 0; i < count; i++)
            {
                var input = inputs.Nth(i);
                var inputId = await input.GetAttributeAsync("id");
                var inputName = await input.GetAttributeAsync("name");
                var inputType = await input.GetAttributeAsync("type");
                System.Console.WriteLine($"Input {i}: id={inputId}, name={inputName}, type={inputType}");
            }
            
            // Use more specific selectors based on the actual IDs found in the modal
            await addTeacherModal.Locator("input#name").FillAsync(name);
            await addTeacherModal.Locator("input#teacherId").FillAsync(teacherId);
            await addTeacherModal.Locator("input#email").FillAsync(email);
            await addTeacherModal.Locator("input#phoneNumber").FillAsync(phone);
            await addTeacherModal.Locator("input#subject").FillAsync(subject);
            
            // Take a screenshot before submitting
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-teacher-form-{teacherId}.png" });
            
            // Submit the form with force option
            var addButton = addTeacherModal.Locator("button:has-text('Add Teacher')");
            System.Console.WriteLine($"Add button exists: {await addButton.IsVisibleAsync()}");
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the modal to close
            System.Console.WriteLine("Waiting for modal to close...");
            try {
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { State = WaitForSelectorState.Hidden, Timeout = 30000 });
                System.Console.WriteLine("Modal closed");
            } catch (Exception ex) {
                System.Console.WriteLine($"Modal did not close: {ex.Message}");
                // Try to force close the modal
                await _page.EvaluateAsync("document.querySelector('.modal.show .btn-close').click()");
                await Task.Delay(1000);
                // Try pressing Escape key
                await _page.Keyboard.PressAsync("Escape");
                await Task.Delay(1000);
            }
            
            // Wait for the page to load
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Take a screenshot after submitting
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"teachers-page-after-submit-{teacherId}.png" });
            
            // Wait for the teacher list to update with a longer timeout
            System.Console.WriteLine("Waiting for table to be visible...");
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            System.Console.WriteLine("Table is visible");
            
            // Take a screenshot after adding
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"teachers-page-after-{teacherId}.png" });
            
            // Print the HTML content of the page for debugging
            var pageContent = await _page.ContentAsync();
            System.Console.WriteLine($"Page content after adding teacher: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
        }
        catch (System.Exception ex)
        {
            // Take a screenshot on error
            await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-add-teacher-{teacherId}.png" });
            System.Console.WriteLine($"Error screenshot saved to error-add-teacher-{teacherId}.png");
            System.Console.WriteLine($"Error adding teacher {name} with ID {teacherId}: {ex.Message}");
            
            // Print the HTML content of the page for debugging
            var pageContent = await _page.ContentAsync();
            System.Console.WriteLine($"Page content on error: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
            
            throw;
        }
    }
    
    private async Task AddStudent(string name, string studentId, string email, string phone)
    {
        try
        {
            // Make sure no modals are open
            await CloseAnyOpenModals();
            
            // Navigate to Students page if not already there
            // Use a more specific selector for the Students link in the navigation menu
            var studentsLink = _page!.Locator("nav a.nav-link", new() { HasText = "Students" });
            
            // Use force option to bypass any intercepting elements
            await studentsLink.ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the page to load
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Take a screenshot for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"students-page-before-{studentId}.png" });
            
            // Check if the page has a table already
            var tableExists = await _page.Locator("table").IsVisibleAsync();
            System.Console.WriteLine($"Table exists before adding student: {tableExists}");
            
            // Click "Add New Student" button with force option
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Student" }).ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the modal to appear with a specific title
            // Use a more specific selector for the modal dialog
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Student')");
            
            // Take a screenshot of the modal
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-student-modal-{studentId}.png" });
            
            // Use a more specific selector for the modal dialog
            var addStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Student'))");
            
            // Print the HTML content of the modal for debugging
            var modalContent = await addStudentModal.InnerHTMLAsync();
            System.Console.WriteLine($"Modal content for student {studentId}: {modalContent.Substring(0, Math.Min(500, modalContent.Length))}...");
            
            // Find all input elements in the modal and print their IDs
            var inputs = addStudentModal.Locator("input");
            var count = await inputs.CountAsync();
            System.Console.WriteLine($"Found {count} input elements in the modal");
            
            for (int i = 0; i < count; i++)
            {
                var input = inputs.Nth(i);
                var inputId = await input.GetAttributeAsync("id");
                var inputName = await input.GetAttributeAsync("name");
                var inputType = await input.GetAttributeAsync("type");
                System.Console.WriteLine($"Input {i}: id={inputId}, name={inputName}, type={inputType}");
            }
            
            // Use more specific selectors based on the actual IDs found in the modal
            await addStudentModal.Locator("input#name").FillAsync(name);
            await addStudentModal.Locator("input#studentId").FillAsync(studentId);
            // Date of Birth is already filled with default value
            await addStudentModal.Locator("input#email").FillAsync(email);
            await addStudentModal.Locator("input#phoneNumber").FillAsync(phone);
            
            // Find and fill the address field which is required
            // Try different possible selectors for the address field
            try {
                // Print all input fields for debugging
                var allInputs = addStudentModal.Locator("input, textarea");
                var inputCount = await allInputs.CountAsync();
                System.Console.WriteLine($"Found {inputCount} input elements in the modal");
                
                for (int i = 0; i < inputCount; i++) {
                    var input = allInputs.Nth(i);
                    var inputId = await input.GetAttributeAsync("id") ?? "no-id";
                    var inputName = await input.GetAttributeAsync("name") ?? "no-name";
                    var inputType = await input.GetAttributeAsync("type") ?? "no-type";
                    var placeholder = await input.GetAttributeAsync("placeholder") ?? "no-placeholder";
                    System.Console.WriteLine($"Input {i}: id={inputId}, name={inputName}, type={inputType}, placeholder={placeholder}");
                }
                
                // Try different possible selectors for the address field
                var addressField = addStudentModal.Locator("input[name*='address'], input[id*='address'], textarea[name*='address'], textarea[id*='address']").First;
                if (await addressField.CountAsync() > 0) {
                    await addressField.FillAsync("123 Main St, Anytown, CA 12345");
                    System.Console.WriteLine("Found and filled address field");
                } else {
                    System.Console.WriteLine("Could not find address field, trying alternative approach");
                    // If we can't find the address field by name or id, try by label text
                    var addressLabel = addStudentModal.Locator("label:has-text('Address')");
                    if (await addressLabel.CountAsync() > 0) {
                        var forAttr = await addressLabel.GetAttributeAsync("for");
                        if (forAttr != null) {
                            await addStudentModal.Locator($"#{forAttr}").FillAsync("123 Main St, Anytown, CA 12345");
                            System.Console.WriteLine($"Found address field by label with for={forAttr}");
                        }
                    }
                }
            } catch (Exception ex) {
                System.Console.WriteLine($"Error finding/filling address field: {ex.Message}");
            }
            
            // Take a screenshot before submitting
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-student-form-{studentId}.png" });
            
            // Submit the form with force option
            var addButton = addStudentModal.Locator("button:has-text('Add Student')");
            System.Console.WriteLine($"Add button exists: {await addButton.IsVisibleAsync()}");
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the modal to close
            System.Console.WriteLine("Waiting for modal to close...");
            try {
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { State = WaitForSelectorState.Hidden, Timeout = 30000 });
                System.Console.WriteLine("Modal closed");
            } catch (Exception ex) {
                System.Console.WriteLine($"Modal did not close: {ex.Message}");
                // Try to force close the modal
                await _page.EvaluateAsync("document.querySelector('.modal.show .btn-close').click()");
                await Task.Delay(1000);
                // Try pressing Escape key
                await _page.Keyboard.PressAsync("Escape");
                await Task.Delay(1000);
            }
            
            // Wait for the page to load
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Take a screenshot after submitting
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"students-page-after-submit-{studentId}.png" });
            
            // Wait for the student list to update with a longer timeout
            System.Console.WriteLine("Waiting for table to be visible...");
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            System.Console.WriteLine("Table is visible");
            
            // Take a screenshot after adding
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"students-page-after-{studentId}.png" });
            
            // Print the HTML content of the page for debugging
            var pageContent = await _page.ContentAsync();
            System.Console.WriteLine($"Page content after adding student: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
        }
        catch (System.Exception ex)
        {
            // Take a screenshot on error
            await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-add-student-{studentId}.png" });
            System.Console.WriteLine($"Error screenshot saved to error-add-student-{studentId}.png");
            System.Console.WriteLine($"Error adding student {name} with ID {studentId}: {ex.Message}");
            
            // Print the HTML content of the page for debugging
            var pageContent = await _page.ContentAsync();
            System.Console.WriteLine($"Page content on error: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
            
            throw;
        }
    }
}
