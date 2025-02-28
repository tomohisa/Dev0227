#!/bin/bash

# Make sure the application is running
echo "Checking if the SchoolManagement application is running..."
if ! curl -s https://localhost:7201 > /dev/null; then
    echo "The SchoolManagement application is not running."
    echo "Please start it with: cd SchoolManagement/SchoolManagement.AppHost && dotnet run --launch-profile https"
    exit 1
fi

echo "SchoolManagement application is running."
echo "Running Playwright tests..."

# Run the tests
dotnet test

# Check the result
if [ $? -eq 0 ]; then
    echo "Tests passed successfully!"
    echo "The following operations were performed:"
    echo "1. Added student: John Smith (ID: S12345)"
    echo "2. Added student: Jane Doe (ID: S67890)"
    echo "3. Added class: Mathematics 101 (Code: MATH101)"
    
    # Open the screenshots
    echo "Screenshots were saved to:"
    ls -la *.png
else
    echo "Tests failed. Please check the error messages above."
fi
