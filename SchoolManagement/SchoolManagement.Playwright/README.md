# SchoolManagement Playwright Tests

This project contains automated UI tests for the SchoolManagement application using Playwright and MSTest.

## Prerequisites

- .NET 9.0 SDK or later
- The SchoolManagement application must be running at https://localhost:7201

## Test Description

The test suite automates the following actions:

1. Navigates to the SchoolManagement application
2. Adds two student records:
   - John Smith (ID: S12345)
   - Jane Doe (ID: S67890)
3. Adds a class:
   - Mathematics 101 (Code: MATH101)
4. Verifies that all records were added successfully

This test validates the core functionality of adding students and classes to the system.

## Running the Tests

### Option 1: Using the run-tests.sh script

The easiest way to run the tests is to use the provided shell script:

```bash
cd SchoolManagement/SchoolManagement.Playwright
./run-tests.sh
```

This script will:
1. Check if the SchoolManagement application is running
2. Run the tests
3. Display the results and list the screenshots

### Option 2: Manual execution

To run the tests manually, make sure the SchoolManagement application is running first:

```bash
cd SchoolManagement/SchoolManagement.AppHost
dotnet run --launch-profile https
```

Then, in a separate terminal, run the tests:

```bash
cd SchoolManagement/SchoolManagement.Playwright
dotnet test
```

## Implementation Details

The test uses a robust approach with:

1. Automatic browser installation during test initialization
2. Non-headless browser mode for easier debugging
3. Detailed error handling with screenshots at each step
4. Specific selectors for UI elements to avoid ambiguity
5. Explicit waits for elements and page loads
6. Verification of added records using unique identifiers
7. Page reloads between operations to ensure a clean state
8. Force option for click actions to bypass intercepting elements
9. Enhanced modal closing logic with multiple approaches

## Debugging

The test generates several screenshots during execution to help with debugging:

- `homepage.png`: The home page of the application
- `students-page-before-*.png`: The Students page before adding a student
- `add-student-modal-*.png`: The Add Student modal dialog
- `add-student-form-*.png`: The Add Student form after filling in the details
- `students-page-after-*.png`: The Students page after adding a student
- `students-table-after-*.png`: The Students table after adding a student
- `classes-page-before.png`: The Classes page before adding a class
- `add-class-modal.png`: The Add Class modal dialog
- `add-class-form.png`: The Add Class form after filling in the details
- `classes-page-after.png`: The Classes page after adding a class
- `classes-table-after.png`: The Classes table after adding a class
- `error-*.png`: Screenshots taken when an error occurs

Additionally, the test outputs detailed information to the console, including:
- Page title
- HTML content
- Modal content
- Input element details (IDs, names, types)
- Error messages and stack traces

This comprehensive approach ensures that the test is robust and easy to debug.
