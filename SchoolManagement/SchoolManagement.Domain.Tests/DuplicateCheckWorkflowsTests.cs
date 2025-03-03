using ResultBoxes;
using Sekiban.Pure;
using Sekiban.Pure.Executors;
using Sekiban.Pure.xUnit;
using SchoolManagement.Domain.Generated;
using SchoolManagement.Domain.Workflows;
using Xunit;
using System;
using System.Threading.Tasks;
using SchoolManagement.Domain.Aggregates.Students.Commands;

namespace SchoolManagement.Domain.Tests;

public class DuplicateCheckWorkflowsTests : SekibanInMemoryTestBase
{
    protected override SekibanDomainTypes GetDomainTypes() => 
        SchoolManagementDomainDomainTypes.Generate(SchoolManagementDomainEventsJsonContext.Default.Options);

    [Fact]
    public async Task CheckStudentIdDuplicate_WhenStudentIdExists_ReturnsDuplicate()
    {
        // Arrange
        var existingStudentId = "S12345";
        var command = new RegisterStudentCommand(
            "John Doe",
            existingStudentId,
            new DateTime(2000, 1, 1),
            "john@example.com",
            "123-456-7890",
            "123 Main St");

        // Register a student with the same ID to ensure it exists
        GivenCommand(command);

        // Act
        var result = await DuplicateCheckWorkflows.CheckStudentIdDuplicate(command, Executor);

        // Assert
        Assert.True(result.IsDuplicate);
        Assert.Contains(existingStudentId, result.ErrorMessage);
        Assert.Null(result.CommandResult);
    }

    [Fact]
    public async Task CheckStudentIdDuplicate_WhenStudentIdDoesNotExist_ReturnsSuccess()
    {
        // Arrange
        var newStudentId = "S67890";
        var command = new RegisterStudentCommand(
            "Jane Doe",
            newStudentId,
            new DateTime(2001, 2, 2),
            "jane@example.com",
            "987-654-3210",
            "456 Oak St");

        // Act
        var result = await DuplicateCheckWorkflows.CheckStudentIdDuplicate(command, Executor);

        // Assert
        Assert.False(result.IsDuplicate);
        Assert.Null(result.ErrorMessage);
        Assert.NotNull(result.CommandResult);
    }

    [Fact]
    public async Task CheckTeacherIdDuplicate_WhenTeacherIdExists_ReturnsDuplicate()
    {
        // Arrange
        var existingTeacherId = "T12345";
        var command = new RegisterTeacherCommand(
            "John Smith",
            existingTeacherId,
            "john.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics");

        // Register a teacher with the same ID to ensure it exists
        GivenCommand(command);

        // Act
        var result = await DuplicateCheckWorkflows.CheckTeacherIdDuplicate(command, Executor);

        // Assert
        Assert.True(result.IsDuplicate);
        Assert.Contains(existingTeacherId, result.ErrorMessage);
        Assert.Null(result.CommandResult);
    }

    [Fact]
    public async Task CheckTeacherIdDuplicate_WhenTeacherIdDoesNotExist_ReturnsSuccess()
    {
        // Arrange
        var newTeacherId = "T67890";
        var command = new RegisterTeacherCommand(
            "Jane Smith",
            newTeacherId,
            "jane.smith@example.com",
            "987-654-3210",
            "456 Oak St",
            "Science");

        // Act
        var result = await DuplicateCheckWorkflows.CheckTeacherIdDuplicate(command, Executor);

        // Assert
        Assert.False(result.IsDuplicate);
        Assert.Null(result.ErrorMessage);
        Assert.NotNull(result.CommandResult);
    }
}
