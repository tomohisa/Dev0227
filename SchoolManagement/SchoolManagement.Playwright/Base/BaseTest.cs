using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SchoolManagement.Playwright.Base
{
    public class BaseTest
    {
        protected const string BaseUrl = "https://localhost:7201";
        protected IBrowser? Browser;
        protected IBrowserContext? Context;
        protected IPage? Page;
        protected Process? AppHostProcess;
        private readonly HttpClient _httpClient = new HttpClient();

        [SetUp]
        public async Task SetUp()
        {
            System.Console.WriteLine("=== SETUP PERFORMANCE DEBUGGING ===");
            var totalSetupSw = Stopwatch.StartNew();
            
            // Start AppHost
            System.Console.WriteLine("Starting AppHost...");
            var appHostSw = Stopwatch.StartNew();
            
            try
            {
                // Kill any existing SchoolManagement processes first
                KillSchoolManagementProcesses();
                
                // Get the absolute path to the AppHost directory
                string currentDirectory = Directory.GetCurrentDirectory();
                System.Console.WriteLine($"Current directory: {currentDirectory}");
                
                // Try multiple possible paths to find the AppHost directory
                string appHostPath = FindAppHostDirectory(currentDirectory);
                
                if (string.IsNullOrEmpty(appHostPath))
                {
                    throw new DirectoryNotFoundException("Could not find SchoolManagement.AppHost directory in any of the expected locations");
                }
                
                System.Console.WriteLine($"Found AppHost directory at: {appHostPath}");
                
                // Start the AppHost
                AppHostProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "run --launch-profile https",
                        WorkingDirectory = appHostPath,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };
                
                System.Console.WriteLine("Starting AppHost process...");
                AppHostProcess.Start();
                
                // Wait for the server to start
                System.Console.WriteLine("Waiting for AppHost to start...");
                bool serverStarted = await WaitForServerToStart();
                
                if (!serverStarted)
                {
                    System.Console.WriteLine("WARNING: Server did not respond within the timeout period");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error starting AppHost: {ex.Message}");
            }
            
            appHostSw.Stop();
            System.Console.WriteLine($"AppHost started in {appHostSw.ElapsedMilliseconds}ms");
            
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
            
            // Kill SchoolManagement processes
            System.Console.WriteLine("Killing SchoolManagement processes...");
            var killSw = Stopwatch.StartNew();
            
            KillSchoolManagementProcesses();
            
            killSw.Stop();
            System.Console.WriteLine($"SchoolManagement processes killed in {killSw.ElapsedMilliseconds}ms");
        }
        
        private string FindAppHostDirectory(string startingDirectory)
        {
            // List of possible relative paths to try
            string[] possiblePaths = new[]
            {
                "../../SchoolManagement.AppHost",                           // From bin/Debug/net9.0
                "../../../SchoolManagement.AppHost",                        // From bin/Debug
                "../../../../SchoolManagement.AppHost",                     // From bin
                "../../../../../SchoolManagement.AppHost",                  // From Playwright
                "../../..",                                                 // To solution root
                "../../../.."                                               // One level up from solution root
            };
            
            // First try to find the solution root by looking for SchoolManagement.sln
            string solutionRoot = FindSolutionRoot(startingDirectory);
            
            if (!string.IsNullOrEmpty(solutionRoot))
            {
                string appHostPath = Path.Combine(solutionRoot, "SchoolManagement.AppHost");
                System.Console.WriteLine($"Checking solution-relative path: {appHostPath}");
                
                if (Directory.Exists(appHostPath))
                {
                    return appHostPath;
                }
            }
            
            // Try each possible relative path
            foreach (string relativePath in possiblePaths)
            {
                try
                {
                    string fullPath = Path.GetFullPath(Path.Combine(startingDirectory, relativePath));
                    System.Console.WriteLine($"Checking path: {fullPath}");
                    
                    if (Directory.Exists(fullPath))
                    {
                        // Check if this is the AppHost directory
                        if (Path.GetFileName(fullPath) == "SchoolManagement.AppHost")
                        {
                            return fullPath;
                        }
                        
                        // Check if AppHost is a subdirectory
                        string appHostSubdir = Path.Combine(fullPath, "SchoolManagement.AppHost");
                        if (Directory.Exists(appHostSubdir))
                        {
                            return appHostSubdir;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error checking path: {ex.Message}");
                }
            }
            
            // As a last resort, try to find it from the root of the project
            try
            {
                string projectRoot = "/Users/tomohisa/dev/test/Dev0227/SchoolManagement";
                System.Console.WriteLine($"Trying hardcoded project root: {projectRoot}");
                
                if (Directory.Exists(projectRoot))
                {
                    string appHostPath = Path.Combine(projectRoot, "SchoolManagement.AppHost");
                    if (Directory.Exists(appHostPath))
                    {
                        return appHostPath;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error checking hardcoded path: {ex.Message}");
            }
            
            return null;
        }
        
        private string FindSolutionRoot(string startingDirectory)
        {
            try
            {
                string directory = startingDirectory;
                
                // Go up the directory tree looking for SchoolManagement.sln
                while (!string.IsNullOrEmpty(directory))
                {
                    System.Console.WriteLine($"Checking for solution in: {directory}");
                    
                    // Check if the solution file exists in this directory
                    if (File.Exists(Path.Combine(directory, "SchoolManagement.sln")))
                    {
                        System.Console.WriteLine($"Found solution root at: {directory}");
                        return directory;
                    }
                    
                    // Check if we've reached the root directory
                    string parent = Directory.GetParent(directory)?.FullName;
                    if (string.IsNullOrEmpty(parent) || parent == directory)
                    {
                        break;
                    }
                    
                    directory = parent;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error finding solution root: {ex.Message}");
            }
            
            return null;
        }
        
        private async Task<bool> WaitForServerToStart()
        {
            const int maxRetries = 30;
            const int retryDelayMs = 1000;
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    System.Console.WriteLine($"Checking if server is running (attempt {i+1}/{maxRetries})...");
                    var response = await _httpClient.GetAsync(BaseUrl);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        System.Console.WriteLine("Server is running!");
                        return true;
                    }
                    
                    System.Console.WriteLine($"Server returned status code: {response.StatusCode}");
                }
                catch (HttpRequestException ex)
                {
                    System.Console.WriteLine($"Server not yet available: {ex.Message}");
                }
                
                await Task.Delay(retryDelayMs);
            }
            
            return false;
        }
        
        private void KillSchoolManagementProcesses()
        {
            try
            {
                // Kill the AppHost process if we started it
                if (AppHostProcess != null && !AppHostProcess.HasExited)
                {
                    try
                    {
                        AppHostProcess.Kill();
                        System.Console.WriteLine("AppHost process killed");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error killing AppHost process: {ex.Message}");
                    }
                }
                
                // Use pkill to kill all SchoolManagement processes
                var pkillProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "pkill",
                        Arguments = "-9 -f \"SchoolManagement.\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                
                pkillProcess.Start();
                string output = pkillProcess.StandardOutput.ReadToEnd();
                string error = pkillProcess.StandardError.ReadToEnd();
                pkillProcess.WaitForExit();
                
                if (!string.IsNullOrEmpty(output))
                {
                    System.Console.WriteLine($"pkill output: {output}");
                }
                
                if (!string.IsNullOrEmpty(error))
                {
                    System.Console.WriteLine($"pkill error: {error}");
                }
                
                System.Console.WriteLine("All SchoolManagement processes killed");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error in KillSchoolManagementProcesses: {ex.Message}");
            }
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
