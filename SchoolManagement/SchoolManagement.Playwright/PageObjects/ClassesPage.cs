using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.PageObjects
{
    public class ClassesPage
    {
        private readonly IPage _page;
        private const string PageName = "Classes";

        public ClassesPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToClassesPage()
        {
            // Navigate to Classes page
            var classesLink = _page.Locator("nav a.nav-link", new() { HasText = PageName });
            await classesLink.ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task ClickAddNewClassButton()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Class" })
                .ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Class')");
        }

        public async Task FillClassForm(string name, string classCode, string description)
        {
            var addClassModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Class'))");
            
            await addClassModal.Locator("input#name").FillAsync(name);
            await addClassModal.Locator("input#classCode").FillAsync(classCode);
            await addClassModal.Locator("textarea#description").FillAsync(description);
        }

        public async Task SubmitClassForm()
        {
            var addClassModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Class'))");
            var addButton = addClassModal.Locator("button:has-text('Add Class')");
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            
            try {
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { 
                    State = WaitForSelectorState.Hidden, 
                    Timeout = 10000 
                });
            } catch (Exception ex) {
                System.Console.WriteLine($"Modal did not close: {ex.Message}");
                await _page.EvaluateAsync("document.querySelector('.modal.show .btn-close')?.click()");
                await Task.Delay(1000);
                await _page.Keyboard.PressAsync("Escape");
                await Task.Delay(1000);
            }
            
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task<bool> VerifyClassAdded(string name, string classCode)
        {
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            var classRows = _page.Locator("tr", new() { HasText = name }).Filter(new() { HasText = classCode });
            var classRowCount = await classRows.CountAsync();
            return classRowCount > 0;
        }

        public async Task AddClass(string name, string classCode, string description)
        {
            await NavigateToClassesPage();
            await ClickAddNewClassButton();
            await FillClassForm(name, classCode, description);
            await SubmitClassForm();
            await VerifyClassAdded(name, classCode);
        }

        public async Task ClickAssignTeacherButton(string className)
        {
            var classRow = _page.Locator("tr", new() { HasText = className });
            await classRow.Locator("button:has-text('Assign Teacher')").ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Assign Teacher')");
        }

        public async Task SelectTeacherFromDropdown(string teacherName)
        {
            var assignTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Assign Teacher'))");
            var teacherDropdown = assignTeacherModal.Locator("select");
            await teacherDropdown.SelectOptionAsync(new[] { teacherName });
        }

        public async Task SubmitAssignTeacherForm()
        {
            var assignTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Assign Teacher'))");
            var assignButton = assignTeacherModal.Locator("button:has-text('Assign')");
            await assignButton.ClickAsync(new LocatorClickOptions { Force = true });
            
            try {
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { 
                    State = WaitForSelectorState.Hidden, 
                    Timeout = 10000 
                });
            } catch (Exception ex) {
                System.Console.WriteLine($"Modal did not close: {ex.Message}");
                await _page.EvaluateAsync("document.querySelector('.modal.show .btn-close')?.click()");
                await Task.Delay(1000);
                await _page.Keyboard.PressAsync("Escape");
                await Task.Delay(1000);
            }
            
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task AssignTeacherToClass(string teacherName, string className)
        {
            await NavigateToClassesPage();
            await ClickAssignTeacherButton(className);
            await SelectTeacherFromDropdown(teacherName);
            await SubmitAssignTeacherForm();
        }

        public async Task ClickAssignStudentButton(string className)
        {
            var classRow = _page.Locator("tr", new() { HasText = className });
            await classRow.Locator("button:has-text('Assign Student')").ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Assign Student')");
        }

        public async Task SelectStudentFromDropdown(string studentName)
        {
            var assignStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Assign Student'))");
            var studentDropdown = assignStudentModal.Locator("select");
            await studentDropdown.SelectOptionAsync(new[] { studentName });
        }

        public async Task SubmitAssignStudentForm()
        {
            var assignStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Assign Student'))");
            var assignButton = assignStudentModal.Locator("button:has-text('Assign')");
            await assignButton.ClickAsync(new LocatorClickOptions { Force = true });
            
            try {
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { 
                    State = WaitForSelectorState.Hidden, 
                    Timeout = 10000 
                });
            } catch (Exception ex) {
                System.Console.WriteLine($"Modal did not close: {ex.Message}");
                await _page.EvaluateAsync("document.querySelector('.modal.show .btn-close')?.click()");
                await Task.Delay(1000);
                await _page.Keyboard.PressAsync("Escape");
                await Task.Delay(1000);
            }
            
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task AssignStudentToClass(string studentName, string className)
        {
            await NavigateToClassesPage();
            await ClickAssignStudentButton(className);
            await SelectStudentFromDropdown(studentName);
            await SubmitAssignStudentForm();
        }
    }
}
