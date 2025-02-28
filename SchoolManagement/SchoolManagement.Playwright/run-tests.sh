#!/bin/bash

# This script will run the tests using the browser_action tool

# Make sure the application is running
echo "Make sure the application is running with 'cd SchoolManagement/SchoolManagement.AppHost && dotnet run --launch-profile https'"
echo "Then run this script to test the application"

# Wait for user confirmation
read -p "Press Enter to continue..."

# Launch the browser and navigate to the application
echo "Launching browser and navigating to https://localhost:7201"
