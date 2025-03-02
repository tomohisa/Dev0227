using Sekiban.Pure.xUnit;
using Xunit;
using System;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Generated;
using Sekiban.Pure;

namespace SchoolManagement.Domain.Tests;

public class StudentTests : SekibanInMemoryTestBase
{
    protected override SekibanDomainTypes GetDomainTypes() => 
        SchoolManagementDomainDomainTypes.Generate(SchoolManagementDomainEventsJsonContext.Default.Options);

    [Fact]
    public void RegisterStudent_ShouldCreateStudent()
    {
        // Arrange
        var command = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );

        // Act
        var response = GivenCommand(command);

        // Assert
        Assert.Equal(1, response.Version);
        
        var aggregate = ThenGetAggregate<StudentProjector>(response.PartitionKeys);
        Assert.IsType<Student>(aggregate.Payload);
        
        var student = (Student)aggregate.Payload;
        Assert.Equal("John Doe", student.Name);
        Assert.Equal("S12345", student.StudentId);
        Assert.Equal(new DateTime(2000, 1, 1), student.DateOfBirth);
        Assert.Equal("john.doe@example.com", student.Email);
        Assert.Equal("123-456-7890", student.PhoneNumber);
        Assert.Equal("123 Main St", student.Address);
        Assert.Null(student.ClassId);
    }

    [Fact]
    public void UpdateStudent_ShouldUpdateStudentProperties()
    {
        // Arrange - Create a student
        var registerCommand = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Update the student
        var updateCommand = new UpdateStudentCommand(
            registerResponse.PartitionKeys.AggregateId,
            "John Smith",
            "john.smith@example.com",
            "987-654-3210",
            "456 Oak Ave"
        );
        var updateResponse = WhenCommand(updateCommand);
        
        // Assert
        Assert.Equal(2, updateResponse.Version);
        
        var aggregate = ThenGetAggregate<StudentProjector>(updateResponse.PartitionKeys);
        Assert.IsType<Student>(aggregate.Payload);
        
        var student = (Student)aggregate.Payload;
        Assert.Equal("John Smith", student.Name);
        Assert.Equal("S12345", student.StudentId); // StudentId should not change
        Assert.Equal(new DateTime(2000, 1, 1), student.DateOfBirth); // DateOfBirth should not change
        Assert.Equal("john.smith@example.com", student.Email);
        Assert.Equal("987-654-3210", student.PhoneNumber);
        Assert.Equal("456 Oak Ave", student.Address);
    }

    [Fact]
    public void DeleteStudent_ShouldChangeStateToDeletedStudent()
    {
        // Arrange - Create a student
        var registerCommand = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Delete the student
        var deleteCommand = new DeleteStudentCommand(registerResponse.PartitionKeys.AggregateId);
        var deleteResponse = WhenCommand(deleteCommand);
        
        // Assert
        Assert.Equal(2, deleteResponse.Version);
        
        var aggregate = ThenGetAggregate<StudentProjector>(deleteResponse.PartitionKeys);
        Assert.IsType<DeletedStudent>(aggregate.Payload);
        
        var deletedStudent = (DeletedStudent)aggregate.Payload;
        Assert.Equal("John Doe", deletedStudent.Name);
        Assert.Equal("S12345", deletedStudent.StudentId);
    }

    [Fact]
    public void AssignStudentToClass_ShouldSetClassId()
    {
        // Arrange - Create a student
        var registerCommand = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Assign student to class
        var classId = Guid.NewGuid();
        var assignCommand = new AssignStudentToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId
        );
        var assignResponse = WhenCommand(assignCommand);
        
        // Assert
        Assert.Equal(2, assignResponse.Version);
        
        var aggregate = ThenGetAggregate<StudentProjector>(assignResponse.PartitionKeys);
        Assert.IsType<Student>(aggregate.Payload);
        
        var student = (Student)aggregate.Payload;
        Assert.Equal(classId, student.ClassId);
    }

    [Fact]
    public void RemoveStudentFromClass_ShouldClearClassId()
    {
        // Arrange - Create a student and assign to class
        var registerCommand = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        var classId = Guid.NewGuid();
        var assignCommand = new AssignStudentToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId
        );
        var assignResponse = WhenCommand(assignCommand);
        
        // Act - Remove student from class
        var removeCommand = new RemoveStudentFromClassCommand(registerResponse.PartitionKeys.AggregateId);
        var removeResponse = WhenCommand(removeCommand);
        
        // Assert
        Assert.Equal(3, removeResponse.Version);
        
        var aggregate = ThenGetAggregate<StudentProjector>(removeResponse.PartitionKeys);
        Assert.IsType<Student>(aggregate.Payload);
        
        var student = (Student)aggregate.Payload;
        Assert.Null(student.ClassId);
    }

    [Fact]
    public void GetAge_ShouldCalculateCorrectAge()
    {
        // Arrange
        var birthDate = DateTime.Today.AddYears(-20);
        var command = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            birthDate,
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var response = GivenCommand(command);
        
        // Act
        var aggregate = ThenGetAggregate<StudentProjector>(response.PartitionKeys);
        var student = (Student)aggregate.Payload;
        var age = student.GetAge();
        
        // Assert
        Assert.Equal(20, age);
    }
}
