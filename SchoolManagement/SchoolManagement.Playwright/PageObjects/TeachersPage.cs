using Microsoft.Playwright;
using System;
using System.Diagnostics;
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
            System.Console.WriteLine("  [TeachersPage] Starting navigation to Teachers page");
            var sw = Stopwatch.StartNew();
            
            // Navigate to Teachers page
            var teachersLink = _page.Locator("nav a.nav-link", new() { HasText = PageName });
            System.Console.WriteLine($"  [TeachersPage] Found nav link in {sw.ElapsedMilliseconds}ms");
            
            await teachersLink.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"  [TeachersPage] Clicked nav link in {sw.ElapsedMilliseconds}ms");
            
            var networkSw = Stopwatch.StartNew();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            networkSw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Waited for network idle in {networkSw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Navigation completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task ClickAddNewTeacherButton()
        {
            System.Console.WriteLine("  [TeachersPage] Clicking Add New Teacher button");
            var sw = Stopwatch.StartNew();
            
            var addButton = _page.GetByRole(AriaRole.Button, new() { Name = "Add New Teacher" });
            System.Console.WriteLine($"  [TeachersPage] Found Add New Teacher button in {sw.ElapsedMilliseconds}ms");
            
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"  [TeachersPage] Clicked Add New Teacher button in {sw.ElapsedMilliseconds}ms");
            
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Teacher')");
            System.Console.WriteLine($"  [TeachersPage] Modal appeared in {sw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Add button click completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task FillTeacherForm(string name, string teacherId, string email, string phone, string subject, string address)
        {
            System.Console.WriteLine("  [TeachersPage] Filling teacher form");
            var sw = Stopwatch.StartNew();
            
            var addTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Teacher'))");
            System.Console.WriteLine($"  [TeachersPage] Found modal in {sw.ElapsedMilliseconds}ms");
            
            // Fill each field and log time
            await addTeacherModal.Locator("input#name").FillAsync(name);
            System.Console.WriteLine($"  [TeachersPage] Filled name field in {sw.ElapsedMilliseconds}ms");
            
            await addTeacherModal.Locator("input#teacherId").FillAsync(teacherId);
            System.Console.WriteLine($"  [TeachersPage] Filled teacherId field in {sw.ElapsedMilliseconds}ms");
            
            await addTeacherModal.Locator("input#email").FillAsync(email);
            System.Console.WriteLine($"  [TeachersPage] Filled email field in {sw.ElapsedMilliseconds}ms");
            
            await addTeacherModal.Locator("input#phoneNumber").FillAsync(phone);
            System.Console.WriteLine($"  [TeachersPage] Filled phone field in {sw.ElapsedMilliseconds}ms");
            
            await addTeacherModal.Locator("input#subject").FillAsync(subject);
            System.Console.WriteLine($"  [TeachersPage] Filled subject field in {sw.ElapsedMilliseconds}ms");
            
            // Fill address field
            try {
                var addressFieldSw = Stopwatch.StartNew();
                var addressField = addTeacherModal.Locator("input[name*='address'], input[id*='address'], textarea[name*='address'], textarea[id*='address']").First;
                System.Console.WriteLine($"  [TeachersPage] Locating address field took {addressFieldSw.ElapsedMilliseconds}ms");
                
                if (await addressField.CountAsync() > 0) {
                    await addressField.FillAsync(address);
                    System.Console.WriteLine($"  [TeachersPage] Filled address field in {addressFieldSw.ElapsedMilliseconds}ms");
                } else {
                    var addressLabel = addTeacherModal.Locator("label:has-text('Address')");
                    System.Console.WriteLine($"  [TeachersPage] Looking for address label took {addressFieldSw.ElapsedMilliseconds}ms");
                    
                    if (await addressLabel.CountAsync() > 0) {
                        var forAttr = await addressLabel.GetAttributeAsync("for");
                        System.Console.WriteLine($"  [TeachersPage] Getting for attribute took {addressFieldSw.ElapsedMilliseconds}ms");
                        
                        if (forAttr != null) {
                            await addTeacherModal.Locator($"#{forAttr}").FillAsync(address);
                            System.Console.WriteLine($"  [TeachersPage] Filled address field by label in {addressFieldSw.ElapsedMilliseconds}ms");
                        }
                    }
                }
                addressFieldSw.Stop();
            } catch (Exception ex) {
                System.Console.WriteLine($"  [TeachersPage] Error finding/filling address field: {ex.Message}");
            }
            
            sw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Form filling completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task SubmitTeacherForm()
        {
            System.Console.WriteLine("  [TeachersPage] Submitting teacher form");
            var sw = Stopwatch.StartNew();
            
            var addTeacherModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Teacher'))");
            var addButton = addTeacherModal.Locator("button:has-text('Add Teacher')");
            System.Console.WriteLine($"  [TeachersPage] Found Add Teacher button in {sw.ElapsedMilliseconds}ms");
            
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"  [TeachersPage] Clicked Add Teacher button in {sw.ElapsedMilliseconds}ms");
            
            try {
                var modalCloseSw = Stopwatch.StartNew();
                await _page.WaitForSelectorAsync("#addTeacherModal:not(.show)", new PageWaitForSelectorOptions { 
                    State = WaitForSelectorState.Visible, 
                    Timeout = 1000 
                });
                modalCloseSw.Stop();
                System.Console.WriteLine($"  [TeachersPage] Modal closed in {modalCloseSw.ElapsedMilliseconds}ms");
            } catch (Exception ex) {
                System.Console.WriteLine($"  [TeachersPage] Modal did not close: {ex.Message}");
                System.Console.WriteLine($"  [TeachersPage] Attempting to force close modal after {sw.ElapsedMilliseconds}ms");
                
                await _page.EvaluateAsync("document.querySelector('#addTeacherModal.show .btn-close')?.click()");
                System.Console.WriteLine($"  [TeachersPage] Clicked close button via JS in {sw.ElapsedMilliseconds}ms");
                
                await Task.Delay(1000);
                System.Console.WriteLine($"  [TeachersPage] Waited 1000ms after JS click");
                
                await _page.Keyboard.PressAsync("Escape");
                System.Console.WriteLine($"  [TeachersPage] Pressed Escape key in {sw.ElapsedMilliseconds}ms");
                
                await Task.Delay(1000);
                System.Console.WriteLine($"  [TeachersPage] Waited 1000ms after Escape key");
            }
            
            var networkSw = Stopwatch.StartNew();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            networkSw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Waited for network idle in {networkSw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Form submission completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task<bool> VerifyTeacherAdded(string name, string teacherId)
        {
            System.Console.WriteLine("  [TeachersPage] Verifying teacher was added");
            var sw = Stopwatch.StartNew();
            
            var tableSw = Stopwatch.StartNew();
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            tableSw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Table appeared in {tableSw.ElapsedMilliseconds}ms");
            
            var teacherRows = _page.Locator("tr", new() { HasText = name }).Filter(new() { HasText = teacherId });
            System.Console.WriteLine($"  [TeachersPage] Located teacher rows in {sw.ElapsedMilliseconds}ms");
            
            var teacherRowCount = await teacherRows.CountAsync();
            System.Console.WriteLine($"  [TeachersPage] Counted {teacherRowCount} matching rows in {sw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [TeachersPage] Verification completed in {sw.ElapsedMilliseconds}ms");
            
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
