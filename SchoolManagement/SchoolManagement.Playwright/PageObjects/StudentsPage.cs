using Microsoft.Playwright;
using System;
using System.Diagnostics;
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
            System.Console.WriteLine("  [StudentsPage] Starting navigation to Students page");
            var sw = Stopwatch.StartNew();
            
            // Navigate to Students page
            var studentsLink = _page.Locator("nav a.nav-link", new() { HasText = PageName });
            System.Console.WriteLine($"  [StudentsPage] Found nav link in {sw.ElapsedMilliseconds}ms");
            
            await studentsLink.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"  [StudentsPage] Clicked nav link in {sw.ElapsedMilliseconds}ms");
            
            // Uncomment if needed for debugging
            // await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            // System.Console.WriteLine($"  [StudentsPage] Waited for network idle in {sw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Navigation completed in {sw.ElapsedMilliseconds}ms");
        } 

        public async Task ClickAddNewStudentButton()
        {
            System.Console.WriteLine("  [StudentsPage] Clicking Add New Student button");
            var sw = Stopwatch.StartNew();
            
            var addButton = _page.GetByRole(AriaRole.Button, new() { Name = "Add New Student" });
            System.Console.WriteLine($"  [StudentsPage] Found Add New Student button in {sw.ElapsedMilliseconds}ms");
            
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"  [StudentsPage] Clicked Add New Student button in {sw.ElapsedMilliseconds}ms");
            
            await _page.WaitForSelectorAsync("div.modal-header:has-text('Add Student')");
            System.Console.WriteLine($"  [StudentsPage] Modal appeared in {sw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Add button click completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task FillStudentForm(string name, string studentId, string email, string phone)
        {
            System.Console.WriteLine("  [StudentsPage] Filling student form");
            var sw = Stopwatch.StartNew();
            
            var addStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Student'))");
            System.Console.WriteLine($"  [StudentsPage] Found modal in {sw.ElapsedMilliseconds}ms");
            
            // Fill each field and log time
            await addStudentModal.Locator("input#name").FillAsync(name);
            System.Console.WriteLine($"  [StudentsPage] Filled name field in {sw.ElapsedMilliseconds}ms");
            
            await addStudentModal.Locator("input#studentId").FillAsync(studentId);
            System.Console.WriteLine($"  [StudentsPage] Filled studentId field in {sw.ElapsedMilliseconds}ms");
            
            await addStudentModal.Locator("input#email").FillAsync(email);
            System.Console.WriteLine($"  [StudentsPage] Filled email field in {sw.ElapsedMilliseconds}ms");
            
            await addStudentModal.Locator("input#phoneNumber").FillAsync(phone);
            System.Console.WriteLine($"  [StudentsPage] Filled phone field in {sw.ElapsedMilliseconds}ms");
            
            // Fill address field
            try {
                var addressFieldSw = Stopwatch.StartNew();
                var addressField = addStudentModal.Locator("input[name*='address'], input[id*='address'], textarea[name*='address'], textarea[id*='address']").First;
                System.Console.WriteLine($"  [StudentsPage] Locating address field took {addressFieldSw.ElapsedMilliseconds}ms");
                
                if (await addressField.CountAsync() > 0) {
                    await addressField.FillAsync("123 Main St, Anytown, CA 12345");
                    System.Console.WriteLine($"  [StudentsPage] Filled address field in {addressFieldSw.ElapsedMilliseconds}ms");
                } else {
                    var addressLabel = addStudentModal.Locator("label:has-text('Address')");
                    System.Console.WriteLine($"  [StudentsPage] Looking for address label took {addressFieldSw.ElapsedMilliseconds}ms");
                    
                    if (await addressLabel.CountAsync() > 0) {
                        var forAttr = await addressLabel.GetAttributeAsync("for");
                        System.Console.WriteLine($"  [StudentsPage] Getting for attribute took {addressFieldSw.ElapsedMilliseconds}ms");
                        
                        if (forAttr != null) {
                            await addStudentModal.Locator($"#{forAttr}").FillAsync("123 Main St, Anytown, CA 12345");
                            System.Console.WriteLine($"  [StudentsPage] Filled address field by label in {addressFieldSw.ElapsedMilliseconds}ms");
                        }
                    }
                }
                addressFieldSw.Stop();
            } catch (Exception ex) {
                System.Console.WriteLine($"  [StudentsPage] Error finding/filling address field: {ex.Message}");
            }
            
            sw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Form filling completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task SubmitStudentForm()
        {
            System.Console.WriteLine("  [StudentsPage] Submitting student form");
            var sw = Stopwatch.StartNew();
            
            var addStudentModal = _page.Locator("div.modal-content:has(div.modal-header:has-text('Add Student'))");
            var addButton = addStudentModal.Locator("button:has-text('Add Student')");
            System.Console.WriteLine($"  [StudentsPage] Found Add Student button in {sw.ElapsedMilliseconds}ms");
            
            await addButton.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"  [StudentsPage] Clicked Add Student button in {sw.ElapsedMilliseconds}ms");
            
            try {
                var modalCloseSw = Stopwatch.StartNew();
                await _page.WaitForSelectorAsync(".modal.show", new PageWaitForSelectorOptions { 
                    State = WaitForSelectorState.Hidden, 
                    Timeout = 30000 
                });
                modalCloseSw.Stop();
                System.Console.WriteLine($"  [StudentsPage] Modal closed in {modalCloseSw.ElapsedMilliseconds}ms");
            } catch (Exception ex) {
                System.Console.WriteLine($"  [StudentsPage] Modal did not close: {ex.Message}");
                System.Console.WriteLine($"  [StudentsPage] Attempting to force close modal after {sw.ElapsedMilliseconds}ms");
                
                await _page.EvaluateAsync("document.querySelector('.modal.show .btn-close')?.click()");
                System.Console.WriteLine($"  [StudentsPage] Clicked close button via JS in {sw.ElapsedMilliseconds}ms");
                
                await Task.Delay(1000);
                System.Console.WriteLine($"  [StudentsPage] Waited 1000ms after JS click");
                
                await _page.Keyboard.PressAsync("Escape");
                System.Console.WriteLine($"  [StudentsPage] Pressed Escape key in {sw.ElapsedMilliseconds}ms");
                
                await Task.Delay(1000);
                System.Console.WriteLine($"  [StudentsPage] Waited 1000ms after Escape key");
            }
            
            var networkSw = Stopwatch.StartNew();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            networkSw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Waited for network idle in {networkSw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Form submission completed in {sw.ElapsedMilliseconds}ms");
        }

        public async Task<bool> VerifyStudentAdded(string name, string studentId)
        {
            System.Console.WriteLine("  [StudentsPage] Verifying student was added");
            var sw = Stopwatch.StartNew();
            
            var tableSw = Stopwatch.StartNew();
            await _page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 60000 });
            tableSw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Table appeared in {tableSw.ElapsedMilliseconds}ms");
            
            var studentRows = _page.Locator("tr", new() { HasText = name }).Filter(new() { HasText = studentId });
            System.Console.WriteLine($"  [StudentsPage] Located student rows in {sw.ElapsedMilliseconds}ms");
            
            var studentRowCount = await studentRows.CountAsync();
            System.Console.WriteLine($"  [StudentsPage] Counted {studentRowCount} matching rows in {sw.ElapsedMilliseconds}ms");
            
            sw.Stop();
            System.Console.WriteLine($"  [StudentsPage] Verification completed in {sw.ElapsedMilliseconds}ms");
            
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
