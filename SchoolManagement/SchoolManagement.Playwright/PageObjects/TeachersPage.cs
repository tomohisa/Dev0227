using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.PageObjects
{
    public class TeachersPage
    {
        private readonly IPage _page;
        private const string PageName = "Teachers";

        public TeachersPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToTeachersPage()
        {
            // Navigate to Teachers page
            var teachersLink = _page.Locator("nav a.nav-link", new() { HasText = PageName });
            await teachersLink.ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task ClickAddNewTeacherButton()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add New Teacher" })
                .ClickAsync(new LocatorClickOptions { Force = true });
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Teacher')");
        }

        public async Task FillTeacherForm(string name, string teacherId, string email, string phone, string subject, string address)
        {
            var addTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Teacher'))");
            
            await addTeacherModal.Locator("input#name").FillAsync(name);
            await addTeacherModal.Locator("input#teacherId").FillAsync(teacherId);
            await addTeacherModal.Locator("input#email").FillAsync(email);
            await addTeacherModal.Locator("input#phoneNumber").FillAsync(phone);
            await addTeacherModal.Locator("input#subject").FillAsync(subject);
            
            // Fill address field
            try {
                var addressField = addTeacherModal.Locator("input[name*='address'], input[id*='address'], textarea[name*='address'], textarea[id*='address']").First;
                if (await addressField.CountAsync() > 0) {
                    await addressField.FillAsync(address);
                } else {
                    var addressLabel = addTeacherModal.Locator("label:has-text('Address')");
                    if (await addressLabel.CountAsync() > 0) {
                        var forAttr = await addressLabel.GetAttributeAsync("for");
                        if (forAttr != null) {
                            await addTeacherModal.Locator($"#{forAttr}").FillAsync(address);
                        }
                    }
                }
            } catch (Exception ex) {
                System.Console.WriteLine($"Error finding/filling address field: {ex.Message}");
            }
        }

        public async Task SubmitTeacherForm()
        {
            var addTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Teacher'))");
            var addButton = addTeacherModal.Locator("button:has-text('Add Teacher')");
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

        public async Task<bool> VerifyTeacherAdded(string name, string teacherId)
        {
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            var teacherRows = _page.Locator("tr", new() { HasText = name }).Filter(new() { HasText = teacherId });
            var teacherRowCount = await teacherRows.CountAsync();
            return teacherRowCount > 0;
        }

        public async Task AddTeacher(string name, string teacherId, string email, string phone, string subject, string address)
        {
            await NavigateToTeachersPage();
            await ClickAddNewTeacherButton();
            await FillTeacherForm(name, teacherId, email, phone, subject, address);
            await SubmitTeacherForm();
            await VerifyTeacherAdded(name, teacherId);
        }
    }
}
