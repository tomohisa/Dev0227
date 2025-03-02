using Sekiban.Pure.xUnit;
using Xunit;
using System;
using System.Linq;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Generated;
using Sekiban.Pure;

namespace SchoolManagement.Domain.Tests;

public class TeacherTests : SekibanInMemoryTestBase
{
    protected override SekibanDomainTypes GetDomainTypes() => 
        SchoolManagementDomainDomainTypes.Generate(SchoolManagementDomainEventsJsonContext.Default.Options);

    [Fact]
    public void RegisterTeacher_ShouldCreateTeacher()
    {
        // Arrange
        var command = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );

        // Act
        var response = GivenCommand(command);

        // Assert
        Assert.Equal(1, response.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(response.PartitionKeys);
        Assert.IsType<Teacher>(aggregate.Payload);
        
        var teacher = (Teacher)aggregate.Payload;
        Assert.Equal("Jane Smith", teacher.Name);
        Assert.Equal("T54321", teacher.TeacherId);
        Assert.Equal("jane.smith@example.com", teacher.Email);
        Assert.Equal("123-456-7890", teacher.PhoneNumber);
        Assert.Equal("123 Main St", teacher.Address);
        Assert.Equal("Mathematics", teacher.Subject);
        Assert.Empty(teacher.ClassIds);
    }

    [Fact]
    public void UpdateTeacher_ShouldUpdateTeacherProperties()
    {
        // Arrange - Create a teacher
        var registerCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Update the teacher
        var updateCommand = new UpdateTeacherCommand(
            registerResponse.PartitionKeys.AggregateId,
            "Jane Doe",
            "jane.doe@example.com",
            "987-654-3210",
            "456 Oak Ave",
            "Physics"
        );
        var updateResponse = WhenCommand(updateCommand);
        
        // Assert
        Assert.Equal(2, updateResponse.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(updateResponse.PartitionKeys);
        Assert.IsType<Teacher>(aggregate.Payload);
        
        var teacher = (Teacher)aggregate.Payload;
        Assert.Equal("Jane Doe", teacher.Name);
        Assert.Equal("T54321", teacher.TeacherId); // TeacherId should not change
        Assert.Equal("jane.doe@example.com", teacher.Email);
        Assert.Equal("987-654-3210", teacher.PhoneNumber);
        Assert.Equal("456 Oak Ave", teacher.Address);
        Assert.Equal("Physics", teacher.Subject);
    }

    [Fact]
    public void DeleteTeacher_ShouldChangeStateToDeletedTeacher()
    {
        // Arrange - Create a teacher
        var registerCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Delete the teacher
        var deleteCommand = new DeleteTeacherCommand(registerResponse.PartitionKeys.AggregateId);
        var deleteResponse = WhenCommand(deleteCommand);
        
        // Assert
        Assert.Equal(2, deleteResponse.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(deleteResponse.PartitionKeys);
        Assert.IsType<DeletedTeacher>(aggregate.Payload);
        
        var deletedTeacher = (DeletedTeacher)aggregate.Payload;
        Assert.Equal("Jane Smith", deletedTeacher.Name);
        Assert.Equal("T54321", deletedTeacher.TeacherId);
    }

    [Fact]
    public void AssignTeacherToClass_ShouldAddClassIdToList()
    {
        // Arrange - Create a teacher
        var registerCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Assign teacher to class
        var classId = Guid.NewGuid();
        var assignCommand = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId
        );
        var assignResponse = WhenCommand(assignCommand);
        
        // Assert
        Assert.Equal(2, assignResponse.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(assignResponse.PartitionKeys);
        Assert.IsType<Teacher>(aggregate.Payload);
        
        var teacher = (Teacher)aggregate.Payload;
        Assert.Single(teacher.ClassIds);
        Assert.Contains(classId, teacher.ClassIds);
    }

    [Fact]
    public void AssignTeacherToMultipleClasses_ShouldAddAllClassIdsToList()
    {
        // Arrange - Create a teacher
        var registerCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Assign teacher to multiple classes
        var classId1 = Guid.NewGuid();
        var assignCommand1 = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId1
        );
        var assignResponse1 = WhenCommand(assignCommand1);
        
        var classId2 = Guid.NewGuid();
        var assignCommand2 = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId2
        );
        var assignResponse2 = WhenCommand(assignCommand2);
        
        // Assert
        Assert.Equal(3, assignResponse2.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(assignResponse2.PartitionKeys);
        Assert.IsType<Teacher>(aggregate.Payload);
        
        var teacher = (Teacher)aggregate.Payload;
        Assert.Equal(2, teacher.ClassIds.Count);
        Assert.Contains(classId1, teacher.ClassIds);
        Assert.Contains(classId2, teacher.ClassIds);
    }

    [Fact]
    public void RemoveTeacherFromClass_ShouldRemoveClassIdFromList()
    {
        // Arrange - Create a teacher and assign to classes
        var registerCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        var classId1 = Guid.NewGuid();
        var assignCommand1 = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId1
        );
        var assignResponse1 = WhenCommand(assignCommand1);
        
        var classId2 = Guid.NewGuid();
        var assignCommand2 = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId2
        );
        var assignResponse2 = WhenCommand(assignCommand2);
        
        // Act - Remove teacher from one class
        var removeCommand = new RemoveTeacherFromClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId1
        );
        var removeResponse = WhenCommand(removeCommand);
        
        // Assert
        Assert.Equal(4, removeResponse.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(removeResponse.PartitionKeys);
        Assert.IsType<Teacher>(aggregate.Payload);
        
        var teacher = (Teacher)aggregate.Payload;
        Assert.Single(teacher.ClassIds);
        Assert.DoesNotContain(classId1, teacher.ClassIds);
        Assert.Contains(classId2, teacher.ClassIds);
    }

    [Fact]
    public void AssignTeacherToSameClassTwice_ShouldNotDuplicateClassId()
    {
        // Arrange - Create a teacher
        var registerCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var registerResponse = GivenCommand(registerCommand);
        
        // Act - Assign teacher to the same class twice
        var classId = Guid.NewGuid();
        var assignCommand1 = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId
        );
        var assignResponse1 = WhenCommand(assignCommand1);
        
        var assignCommand2 = new AssignTeacherToClassCommand(
            registerResponse.PartitionKeys.AggregateId,
            classId
        );
        var assignResponse2 = WhenCommand(assignCommand2);
        
        // Assert
        Assert.Equal(3, assignResponse2.Version);
        
        var aggregate = ThenGetAggregate<TeacherProjector>(assignResponse2.PartitionKeys);
        Assert.IsType<Teacher>(aggregate.Payload);
        
        var teacher = (Teacher)aggregate.Payload;
        Assert.Single(teacher.ClassIds);
        Assert.Contains(classId, teacher.ClassIds);
    }
}
