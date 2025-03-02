using Microsoft.Playwright;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.Base
{
    public class BaseTest
    {
        protected const string BaseUrl = "https://localhost:7201";
        protected IBrowser? Browser;
        protected IBrowserContext? Context;
        protected IPage? Page;

        [SetUp]
        public async Task SetUp()
        {
            System.Console.WriteLine("=== SETUP PERFORMANCE DEBUGGING ===");
            var totalSetupSw = Stopwatch.StartNew();
            
            // Create Playwright instance
            System.Console.WriteLine("Creating Playwright instance...");
            var playwrightSw = Stopwatch.StartNew();
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            playwrightSw.Stop();
            System.Console.WriteLine($"Playwright instance created in {playwrightSw.ElapsedMilliseconds}ms");
            
            // Launch browser with increased timeout and viewport size
            System.Console.WriteLine("Launching browser...");
            var browserSw = Stopwatch.StartNew();
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Set to false for debugging
                Timeout = 60000 // 60 seconds
            });
            browserSw.Stop();
            System.Console.WriteLine($"Browser launched in {browserSw.ElapsedMilliseconds}ms");
            
            // Create a new browser context with larger viewport
            System.Console.WriteLine("Creating browser context...");
            var contextSw = Stopwatch.StartNew();
            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1280,
                    Height = 800
                }
            });
            contextSw.Stop();
            System.Console.WriteLine($"Browser context created in {contextSw.ElapsedMilliseconds}ms");
            
            // Create a new page
            System.Console.WriteLine("Creating new page...");
            var pageSw = Stopwatch.StartNew();
            Page = await Context.NewPageAsync();
            pageSw.Stop();
            System.Console.WriteLine($"New page created in {pageSw.ElapsedMilliseconds}ms");
            
            totalSetupSw.Stop();
            System.Console.WriteLine($"Total setup time: {totalSetupSw.ElapsedMilliseconds}ms");
        }

        [TearDown]
        public async Task TearDown()
        {
            System.Console.WriteLine("=== TEARDOWN PERFORMANCE DEBUGGING ===");
            var teardownSw = Stopwatch.StartNew();
            
            // Close browser
            if (Browser != null)
            {
                System.Console.WriteLine("Disposing browser...");
                var disposeSw = Stopwatch.StartNew();
                await Browser.DisposeAsync();
                disposeSw.Stop();
                System.Console.WriteLine($"Browser disposed in {disposeSw.ElapsedMilliseconds}ms");
            }
            
            teardownSw.Stop();
            System.Console.WriteLine($"Total teardown time: {teardownSw.ElapsedMilliseconds}ms");
        }

        protected async Task CloseAnyOpenModals()
        {
            System.Console.WriteLine("Checking for open modals...");
            var modalSw = Stopwatch.StartNew();
            
            try
            {
                // Check if there are any open modals
                var modalElements = Page!.Locator(".modal.show");
                var count = await modalElements.CountAsync();
                
                if (count > 0)
                {
                    System.Console.WriteLine($"Found {count} open modals, attempting to close them");
                    
                    // Try to close modals using the close button
                    var closeButtons = Page.Locator(".modal.show .btn-close");
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
                                System.Console.WriteLine($"Clicked close button {i+1} at {modalSw.ElapsedMilliseconds}ms");
                                
                                // Wait a bit for the modal to close
                                await Task.Delay(500);
                                System.Console.WriteLine($"Waited 500ms after clicking close button at {modalSw.ElapsedMilliseconds}ms");
                            }
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("No close buttons found, trying to click outside the modal");
                        
                        // Try clicking outside the modal
                        await Page.Mouse.ClickAsync(10, 10);
                        System.Console.WriteLine($"Clicked outside modal at {modalSw.ElapsedMilliseconds}ms");
                        
                        await Task.Delay(500);
                        System.Console.WriteLine($"Waited 500ms after clicking outside at {modalSw.ElapsedMilliseconds}ms");
                    }
                    
                    // Check if modals are still open
                    count = await modalElements.CountAsync();
                    if (count > 0)
                    {
                        System.Console.WriteLine($"Still have {count} open modals, trying to press Escape key");
                        
                        // Try pressing Escape key
                        await Page.Keyboard.PressAsync("Escape");
                        System.Console.WriteLine($"Pressed Escape key at {modalSw.ElapsedMilliseconds}ms");
                        
                        await Task.Delay(500);
                        System.Console.WriteLine($"Waited 500ms after pressing Escape at {modalSw.ElapsedMilliseconds}ms");
                        
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
            
            modalSw.Stop();
            System.Console.WriteLine($"Modal check/close completed in {modalSw.ElapsedMilliseconds}ms");
        }

        protected async Task NavigateToPage(string pageName)
        {
            System.Console.WriteLine($"Navigating to {pageName} page...");
            var navSw = Stopwatch.StartNew();
            
            // Make sure no modals are open
            await CloseAnyOpenModals();
            System.Console.WriteLine($"Modal check completed at {navSw.ElapsedMilliseconds}ms");
            
            // Navigate to the specified page
            var navLink = Page!.Locator("nav a.nav-link", new() { HasText = pageName });
            System.Console.WriteLine($"Found navigation link at {navSw.ElapsedMilliseconds}ms");
            
            // Use force option to bypass any intercepting elements
            await navLink.ClickAsync(new LocatorClickOptions { Force = true });
            System.Console.WriteLine($"Clicked navigation link at {navSw.ElapsedMilliseconds}ms");
            
            // Wait for the page to load
            var networkSw = Stopwatch.StartNew();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            networkSw.Stop();
            System.Console.WriteLine($"Waited for network idle in {networkSw.ElapsedMilliseconds}ms");
            
            // Take a screenshot for debugging
            var screenshotSw = Stopwatch.StartNew();
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = $"{pageName.ToLower()}-page.png" });
            screenshotSw.Stop();
            System.Console.WriteLine($"Screenshot taken in {screenshotSw.ElapsedMilliseconds}ms");
            
            navSw.Stop();
            System.Console.WriteLine($"Navigation to {pageName} completed in {navSw.ElapsedMilliseconds}ms");
        }
    }
}
