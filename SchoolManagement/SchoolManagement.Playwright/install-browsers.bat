@echo off
echo Checking if Playwright CLI is installed...

where playwright >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Playwright CLI not found. Installing...
    dotnet tool install --global Microsoft.Playwright.CLI
) else (
    echo Playwright CLI is already installed.
)

echo Installing Playwright browsers...
playwright install

echo Browsers installation complete!
pause
