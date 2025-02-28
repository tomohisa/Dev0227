#!/bin/bash

# Check if Playwright CLI is installed globally
if ! command -v playwright &> /dev/null; then
    echo "Playwright CLI not found. Installing..."
    dotnet tool install --global Microsoft.Playwright.CLI
    
    # Add to PATH for current session
    export PATH="$PATH:$HOME/.dotnet/tools"
fi

echo "Installing Playwright browsers..."
playwright install

echo "Browsers installation complete!"
