using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright;

[TestClass]
public class SchoolManagementTests
{
    private const string BaseUrl = "https://localhost:7201";
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    [TestInitialize]
    public async Task TestInitialize()
    {
        // Install browsers if needed
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "--with-deps" });
        if (exitCode != 0)
        {
            throw new System.Exception($"Playwright exited with code {exitCode}");
        }

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

    [TestCleanup]
    public async Task TestCleanup()
    {
        // Close browser
        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }
    }

    [TestMethod]
    public async Task AddStudentsAndClass()
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
            await studentsNavLink.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
            Assert.IsTrue(await studentsNavLink.IsVisibleAsync(), "Students navigation link is not visible");
            
            // Step 1: Add first student (John Smith)
            string firstStudentName = "John Smith";
            string firstStudentId = "S12345";
            await AddStudent(
                name: firstStudentName,
                studentId: firstStudentId,
                email: "john.smith@example.com",
                phone: "555-123-4567"
            );
            
            // Verify first student was added by checking for the student name and ID in the table
            // Take a screenshot of the students table for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "students-table-after-first.png" });
            
            // Print the HTML content of the table for debugging
            var tableContent = await _page.Locator("table").InnerHTMLAsync();
            System.Console.WriteLine($"Table content after adding first student: {tableContent.Substring(0, Math.Min(500, tableContent.Length))}...");
            
            // Count the number of rows containing the student name and ID
            var firstStudentRows = _page.Locator("tr", new() { HasText = firstStudentName }).Filter(new() { HasText = firstStudentId });
            var firstStudentRowCount = await firstStudentRows.CountAsync();
            System.Console.WriteLine($"Found {firstStudentRowCount} rows containing '{firstStudentName}' and '{firstStudentId}'");
            
            // Verify at least one row exists with the student name and ID
            Assert.IsTrue(firstStudentRowCount > 0, $"Student {firstStudentName} with ID {firstStudentId} was not added successfully");
            
            // Step 2: Add second student (Jane Doe)
            string secondStudentName = "Jane Doe";
            string secondStudentId = "S67890";
            
            // Reload the page to ensure we have a clean state
            await _page.ReloadAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            await AddStudent(
                name: secondStudentName,
                studentId: secondStudentId,
                email: "jane.doe@example.com",
                phone: "555-987-6543"
            );
            
            // Verify second student was added by checking for the student name and ID in the table
            // Take a screenshot of the students table for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "students-table-after-second.png" });
            
            // Count the number of rows containing the student name and ID
            var secondStudentRows = _page.Locator("tr", new() { HasText = secondStudentName }).Filter(new() { HasText = secondStudentId });
            var secondStudentRowCount = await secondStudentRows.CountAsync();
            System.Console.WriteLine($"Found {secondStudentRowCount} rows containing '{secondStudentName}' and '{secondStudentId}'");
            
            // Verify at least one row exists with the student name and ID
            Assert.IsTrue(secondStudentRowCount > 0, $"Student {secondStudentName} with ID {secondStudentId} was not added successfully");
            
            // Step 3: Add a class (Mathematics 101)
            string className = "Mathematics 101";
            string classCode = "MATH101";
            
            // Reload the page to ensure we have a clean state
            await _page.ReloadAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            await AddClass(
                name: className,
                classCode: classCode,
                description: "An introductory course to basic mathematics concepts and principles."
            );
            
            // Verify class was added by checking for the class name and code in the table
            // Take a screenshot of the classes table for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "classes-table-after.png" });
            
            // Count the number of rows containing the class name and code
            var classRows = _page.Locator("tr", new() { HasText = className }).Filter(new() { HasText = classCode });
            var classRowCount = await classRows.CountAsync();
            System.Console.WriteLine($"Found {classRowCount} rows containing '{className}' and '{classCode}'");
            
            // Verify at least one row exists with the class name and code
            Assert.IsTrue(classRowCount > 0, $"Class {className} with code {classCode} was not added successfully");
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
            
            // Take a screenshot before submitting
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-student-form-{studentId}.png" });
            
            // Submit the form with force option
            await addStudentModal.Locator("button:has-text('Add Student')").ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the student list to update
            await _page.WaitForSelectorAsync("table");
            
            // Take a screenshot after adding
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"students-page-after-{studentId}.png" });
        }
        catch (System.Exception ex)
        {
            // Take a screenshot on error
            await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-add-student-{studentId}.png" });
            System.Console.WriteLine($"Error screenshot saved to error-add-student-{studentId}.png");
            System.Console.WriteLine($"Error adding student {name} with ID {studentId}: {ex.Message}");
            throw;
        }
    }
    
    private async Task AddClass(string name, string classCode, string description)
    {
        try
        {
            // Make sure no modals are open
            await CloseAnyOpenModals();
            
            // Navigate to Classes page
            // Use a more specific selector for the Classes link in the navigation menu
            var classesLink = _page!.Locator("nav a.nav-link", new() { HasText = "Classes" });
            
            // Use force option to bypass any intercepting elements
            await classesLink.ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the page to load
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Take a screenshot for debugging
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "classes-page-before.png" });
            
            // Click "Add New Class" button with force option
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Class" }).ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the modal to appear with a specific title
            // Use a more specific selector for the modal dialog
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Class')");
            
            // Take a screenshot of the modal
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "add-class-modal.png" });
            
            // Use a more specific selector for the modal dialog
            var addClassModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Class'))");
            
            // Print the HTML content of the modal for debugging
            var modalContent = await addClassModal.InnerHTMLAsync();
            System.Console.WriteLine($"Modal content for class {classCode}: {modalContent.Substring(0, Math.Min(500, modalContent.Length))}...");
            
            // Find all input elements in the modal and print their IDs
            var inputs = addClassModal.Locator("input, textarea");
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
            await addClassModal.Locator("input#name").FillAsync(name);
            await addClassModal.Locator("input#classCode").FillAsync(classCode);
            await addClassModal.Locator("textarea#description").FillAsync(description);
            
            // Take a screenshot before submitting
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "add-class-form.png" });
            
            // Submit the form with force option
            await addClassModal.Locator("button:has-text('Add Class')").ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the class list to update
            await _page.WaitForSelectorAsync("table");
            
            // Take a screenshot after adding
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "classes-page-after.png" });
        }
        catch (System.Exception ex)
        {
            // Take a screenshot on error
            await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = "error-add-class.png" });
            System.Console.WriteLine($"Error screenshot saved to error-add-class.png");
            System.Console.WriteLine($"Error adding class {name} with code {classCode}: {ex.Message}");
            throw;
        }
    }
}
