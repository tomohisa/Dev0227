# SchoolManagement Playwright Tests

This project contains automated UI tests for the SchoolManagement application using Playwright and MSTest.

## Prerequisites

- .NET 9.0 SDK or later
- The SchoolManagement application must be running at https://localhost:7201

## Test Description

The test suite automates the following actions:

1. Navigates to the SchoolManagement application
2. Adds 3 student records:
   - John Smith (ID: S12345)
   - Jane Doe (ID: S67890)
   - Michael Johnson (ID: S24680)
3. Adds 3 teacher records:
   - Dr. Robert Brown (ID: T12345, Subject: Mathematics)
   - Prof. Sarah Wilson (ID: T67890, Subject: Science)
   - Ms. Emily Davis (ID: T24680, Subject: English)
4. Adds 2 classes:
   - Mathematics 101 (Code: MATH101)
   - Science 101 (Code: SCI101)
5. Assigns teachers to classes:
   - Dr. Robert Brown to Mathematics 101
   - Prof. Sarah Wilson to Science 101
6. Assigns students to classes:
   - John Smith and Jane Doe to Mathematics 101
   - Jane Doe and Michael Johnson to Science 101

This test validates the core functionality of adding students, teachers, and classes to the system, as well as assigning teachers and students to classes.

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
- `teachers-page-before-*.png`: The Teachers page before adding a teacher
- `add-teacher-modal-*.png`: The Add Teacher modal dialog
- `add-teacher-form-*.png`: The Add Teacher form after filling in the details
- `teachers-page-after-*.png`: The Teachers page after adding a teacher
- `classes-page-before-*.png`: The Classes page before adding a class
- `add-class-modal-*.png`: The Add Class modal dialog
- `add-class-form-*.png`: The Add Class form after filling in the details
- `classes-page-after-*.png`: The Classes page after adding a class
- `classes-page-before-assign-teacher-*.png`: The Classes page before assigning a teacher
- `assign-teacher-modal-*.png`: The Assign Teacher modal dialog
- `assign-teacher-form-*.png`: The Assign Teacher form after selecting a teacher
- `classes-page-after-assign-teacher-*.png`: The Classes page after assigning a teacher
- `classes-page-before-assign-student-*.png`: The Classes page before assigning a student
- `assign-student-modal-*.png`: The Assign Student modal dialog
- `assign-student-form-*.png`: The Assign Student form after selecting a student
- `classes-page-after-assign-student-*.png`: The Classes page after assigning a student
- `final-state.png`: The final state of the application after all operations
- `error-*.png`: Screenshots taken when an error occurs

Additionally, the test outputs detailed information to the console, including:
- Page title
- HTML content
- Modal content
- Input element details (IDs, names, types)
- Error messages and stack traces

This comprehensive approach ensures that the test is robust and easy to debug.
