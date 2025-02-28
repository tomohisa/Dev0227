using Microsoft.Playwright;
using NUnit.Framework;
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
            // Create Playwright instance
            var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            
            // Launch browser with increased timeout and viewport size
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Set to false for debugging
                Timeout = 60000 // 60 seconds
            });
            
            // Create a new browser context with larger viewport
            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1280,
                    Height = 800
                }
            });
            
            // Create a new page
            Page = await Context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            // Close browser
            if (Browser != null)
            {
                await Browser.DisposeAsync();
            }
        }

        protected async Task CloseAnyOpenModals()
        {
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
                        await Page.Mouse.ClickAsync(10, 10);
                        await Task.Delay(500);
                    }
                    
                    // Check if modals are still open
                    count = await modalElements.CountAsync();
                    if (count > 0)
                    {
                        System.Console.WriteLine($"Still have {count} open modals, trying to press Escape key");
                        
                        // Try pressing Escape key
                        await Page.Keyboard.PressAsync("Escape");
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

        protected async Task NavigateToPage(string pageName)
        {
            // Make sure no modals are open
            await CloseAnyOpenModals();
            
            // Navigate to the specified page
            var navLink = Page!.Locator("nav a.nav-link", new() { HasText = pageName });
            
            // Use force option to bypass any intercepting elements
            await navLink.ClickAsync(new LocatorClickOptions { Force = true });
            
            // Wait for the page to load
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Take a screenshot for debugging
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = $"{pageName.ToLower()}-page.png" });
        }
    }
}
