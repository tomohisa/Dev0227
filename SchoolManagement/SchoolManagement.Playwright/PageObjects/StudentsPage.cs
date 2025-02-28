using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.PageObjects
{
    public class StudentsPage
    {
        private readonly IPage _page;
        private const string PageName = "Students";

        public StudentsPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToStudentsPage()
        {
            // Navigate to Students page
            var studentsLink = _page.Locator("nav a.nav-link", new() { HasText = PageName });
            await studentsLink.ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task ClickAddNewStudentButton()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Student" })
                .ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Student')");
        }

        public async Task FillStudentForm(string name, string studentId, string email, string phone)
        {
            var addStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Student'))");
            
            await addStudentModal.Locator("input#name").FillAsync(name);
            await addStudentModal.Locator("input#studentId").FillAsync(studentId);
            await addStudentModal.Locator("input#email").FillAsync(email);
            await addStudentModal.Locator("input#phoneNumber").FillAsync(phone);
            
            // Fill address field
            try {
                var addressField = addStudentModal.Locator("input[name*='address'], input[id*='address'], textarea[name*='address'], textarea[id*='address']").First;
                if (await addressField.CountAsync() > 0) {
                    await addressField.FillAsync("123 Main St, Anytown, CA 12345");
                } else {
                    var addressLabel = addStudentModal.Locator("label:has-text('Address')");
                    if (await addressLabel.CountAsync() > 0) {
                        var forAttr = await addressLabel.GetAttributeAsync("for");
                        if (forAttr != null) {
                            await addStudentModal.Locator($"#{forAttr}").FillAsync("123 Main St, Anytown, CA 12345");
                        }
                    }
                }
            } catch (Exception ex) {
                System.Console.WriteLine($"Error finding/filling address field: {ex.Message}");
            }
        }

        public async Task SubmitStudentForm()
        {
            var addStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Student'))");
            var addButton = addStudentModal.Locator("button:has-text('Add Student')");
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            
            try {
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { 
                    State = WaitForSelectorState.Hidden, 
                    Timeout = 30000 
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

        public async Task<bool> VerifyStudentAdded(string name, string studentId)
        {
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            var studentRows = _page.Locator("tr", new() { HasText = name }).Filter(new() { HasText = studentId });
            var studentRowCount = await studentRows.CountAsync();
            return studentRowCount > 0;
        }

        public async Task AddStudent(string name, string studentId, string email, string phone)
        {
            await NavigateToStudentsPage();
            await ClickAddNewStudentButton();
            await FillStudentForm(name, studentId, email, phone);
            await SubmitStudentForm();
            await VerifyStudentAdded(name, studentId);
        }
    }
}
