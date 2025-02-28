using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright
{
    public partial class SchoolManagementTests
    {
        private async Task AssignTeacherToClass(string teacherName, string className)
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
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"classes-page-before-assign-teacher-{className}.png" });
                
                // Find the class row
                var classRow = _page.Locator("tr", new() { HasText = className });
                
                // Click the "Assign Teacher" button for this class
                await classRow.Locator("button:has-text('Assign Teacher')").ClickAsync(new LocatorClickOptions { Force = true });
                
                // Wait for the modal to appear with a specific title
                // Use a more specific selector for the modal dialog
                await _page.WaitForSelectorAsync("div.modal-header:has-text('Assign Teacher')");
                
                // Take a screenshot of the modal
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"assign-teacher-modal-{className}.png" });
                
                // Use a more specific selector for the modal dialog
                var assignTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Assign Teacher'))");
                
                // Print the HTML content of the modal for debugging
                var modalContent = await assignTeacherModal.InnerHTMLAsync();
                System.Console.WriteLine($"Modal content for assigning teacher to {className}: {modalContent.Substring(0, Math.Min(500, modalContent.Length))}...");
                
                // Find the teacher dropdown
                var teacherDropdown = assignTeacherModal.Locator("select");
                
                // Select the teacher by name
                await teacherDropdown.SelectOptionAsync(new SelectOptionValue { Label = teacherName });
                
                // Take a screenshot before submitting
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"assign-teacher-form-{className}.png" });
                
                // Submit the form with force option
                var assignButton = assignTeacherModal.Locator("button:has-text('Assign')");
                System.Console.WriteLine($"Assign button exists: {await assignButton.IsVisibleAsync()}");
                await assignButton.ClickAsync(new LocatorClickOptions { Force = true });
                
                // Wait for the modal to close
                System.Console.WriteLine("Waiting for modal to close...");
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { State = WaitForSelectorState.Hidden, Timeout = 10000 });
                System.Console.WriteLine("Modal closed");
                
                // Wait for the page to load
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                
                // Take a screenshot after assigning
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"classes-page-after-assign-teacher-{className}.png" });
                
                // Print the HTML content of the page for debugging
                var pageContent = await _page.ContentAsync();
                System.Console.WriteLine($"Page content after assigning teacher: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
            }
            catch (System.Exception ex)
            {
                // Take a screenshot on error
                await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = $"error-assign-teacher-{className}.png" });
                System.Console.WriteLine($"Error screenshot saved to error-assign-teacher-{className}.png");
                System.Console.WriteLine($"Error assigning teacher {teacherName} to class {className}: {ex.Message}");
                
                // Print the HTML content of the page for debugging
                var pageContent = await _page.ContentAsync();
                System.Console.WriteLine($"Page content on error: {pageContent.Substring(0, Math.Min(500, pageContent.Length))}...");
                
                throw;
            }
        }
    }
}
