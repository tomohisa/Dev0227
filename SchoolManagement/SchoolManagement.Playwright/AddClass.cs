using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright
{
    public partial class SchoolManagementTests
    {
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
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"classes-page-before-{classCode}.png" });
                
                // Check if the page has a table already
                var tableExists = await _page.Locator("table").IsVisibleAsync();
                System.Console.WriteLine($"Table exists before adding class: {tableExists}");
                
                // Click "Add New Class" button with force option
                await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Class" }).ClickAsync(new LocatorClickOptions { Force = true });
                
                // Wait for the modal to appear with a specific title
                // Use a more specific selector for the modal dialog
                await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Class')");
                
                // Take a screenshot of the modal
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-class-modal-{classCode}.png" });
                
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
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"add-class-form-{classCode}.png" });
                
                // Submit the form with force option
                var addButton = addClassModal.Locator("button:has-text('Add Class')");
                System.Console.WriteLine($"Add button exists: {await addButton.IsVisibleAsync()}");
                await addButton.ClickAsync(new LocatorClickOptions { Force = true });
                
                // Wait for the modal to close
                System.Console.WriteLine("Waiting for modal to close...");
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { State = WaitForSelectorState.Hidden, Timeout = 10000 });
                System.Console.WriteLine("Modal closed");
                
                // Wait for the page to load
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                
                // Take a screenshot after submitting
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"classes-page-after-submit-{classCode}.png" });
                
                // Wait for the class list to update with a longer timeout
                System.Console.WriteLine("Waiting for table to be visible...");
                await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
                System.Console.WriteLine("Table is visible");
                
                // Take a screenshot after adding
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"classes-page-after-{classCode}.png" });
                
                // Print the HTML content of the page for debugging
                var pageContent = await _page.ContentAsync();
                System.Console.WriteLine($"Page content after adding class: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
            }
            catch (System.Exception ex)
            {
                // Take a screenshot on error
                await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-add-class-{classCode}.png" });
                System.Console.WriteLine($"Error screenshot saved to error-add-class-{classCode}.png");
                System.Console.WriteLine($"Error adding class {name} with code {classCode}: {ex.Message}");
                
                // Print the HTML content of the page for debugging
                var pageContent = await _page.ContentAsync();
                System.Console.WriteLine($"Page content on error: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
                
                throw;
            }
        }
    }
}
