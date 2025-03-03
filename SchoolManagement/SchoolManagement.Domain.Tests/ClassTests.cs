using Sekiban.Pure.xUnit;
using Xunit;
using System;
using System.Linq;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Aggregates.Classes;
using SchoolManagement.Domain.Aggregates.Classes.Commands;
using SchoolManagement.Domain.Aggregates.Classes.Payloads;
using SchoolManagement.Domain.Generated;
using Sekiban.Pure;

namespace SchoolManagement.Domain.Tests;

public class ClassTests : SekibanInMemoryTestBase
{
    protected override SekibanDomainTypes GetDomainTypes() => 
        SchoolManagementDomainDomainTypes.Generate(SchoolManagementDomainEventsJsonContext.Default.Options);

    [Fact]
    public void CreateClass_ShouldCreateClass()
    {
        // Arrange
        var command = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );

        // Act
        var response = GivenCommand(command);

        // Assert
        Assert.Equal(1, response.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(response.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Equal("Mathematics 101", @class.Name);
        Assert.Equal("MATH101", @class.ClassCode);
        Assert.Equal("Introduction to Mathematics", @class.Description);
        Assert.Null(@class.TeacherId);
        Assert.Empty(@class.StudentIds);
    }

    [Fact]
    public void UpdateClass_ShouldUpdateClassProperties()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        // Act - Update the class
        var updateCommand = new UpdateClassCommand(
            createResponse.PartitionKeys.AggregateId,
            "Advanced Mathematics",
            "MATH201",
            "Advanced topics in Mathematics"
        );
        var updateResponse = WhenCommand(updateCommand);
        
        // Assert
        Assert.Equal(2, updateResponse.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(updateResponse.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Equal("Advanced Mathematics", @class.Name);
        Assert.Equal("MATH201", @class.ClassCode);
        Assert.Equal("Advanced topics in Mathematics", @class.Description);
    }

    [Fact]
    public void DeleteClass_ShouldChangeStateToDeletedClass()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        // Act - Delete the class
        var deleteCommand = new DeleteClassCommand(createResponse.PartitionKeys.AggregateId);
        var deleteResponse = WhenCommand(deleteCommand);
        
        // Assert
        Assert.Equal(2, deleteResponse.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(deleteResponse.PartitionKeys);
        Assert.IsType<DeletedClass>(aggregate.Payload);
        
        var deletedClass = (DeletedClass)aggregate.Payload;
        Assert.Equal("Mathematics 101", deletedClass.Name);
        Assert.Equal("MATH101", deletedClass.ClassCode);
    }

    [Fact]
    public void AssignTeacherToClass_ShouldSetTeacherId()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        // Act - Assign teacher to class
        var teacherId = Guid.NewGuid();
        var assignCommand = new ClassAssignTeacherCommand(
            createResponse.PartitionKeys.AggregateId,
            teacherId
        );
        var assignResponse = WhenCommand(assignCommand);
        
        // Assert
        Assert.Equal(2, assignResponse.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(assignResponse.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Equal(teacherId, @class.TeacherId);
    }

    [Fact]
    public void RemoveTeacherFromClass_ShouldClearTeacherId()
    {
        // Arrange - Create a class and assign teacher
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        var teacherId = Guid.NewGuid();
        var assignCommand = new ClassAssignTeacherCommand(
            createResponse.PartitionKeys.AggregateId,
            teacherId
        );
        var assignResponse = WhenCommand(assignCommand);
        
        // Act - Remove teacher from class
        var removeCommand = new ClassRemoveTeacherCommand(createResponse.PartitionKeys.AggregateId);
        var removeResponse = WhenCommand(removeCommand);
        
        // Assert
        Assert.Equal(3, removeResponse.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(removeResponse.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Null(@class.TeacherId);
    }

    [Fact]
    public void AddStudentToClass_ShouldAddStudentIdToList()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        // Act - Add student to class
        var studentId = Guid.NewGuid();
        var addCommand = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId
        );
        var addResponse = WhenCommand(addCommand);
        
        // Assert
        Assert.Equal(2, addResponse.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(addResponse.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Single(@class.StudentIds);
        Assert.Contains(studentId, @class.StudentIds);
    }

    [Fact]
    public void AddMultipleStudentsToClass_ShouldAddAllStudentIdsToList()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        // Act - Add multiple students to class
        var studentId1 = Guid.NewGuid();
        var addCommand1 = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId1
        );
        var addResponse1 = WhenCommand(addCommand1);
        
        var studentId2 = Guid.NewGuid();
        var addCommand2 = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId2
        );
        var addResponse2 = WhenCommand(addCommand2);
        
        // Assert
        Assert.Equal(3, addResponse2.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(addResponse2.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Equal(2, @class.StudentIds.Count);
        Assert.Contains(studentId1, @class.StudentIds);
        Assert.Contains(studentId2, @class.StudentIds);
    }

    [Fact]
    public void RemoveStudentFromClass_ShouldRemoveStudentIdFromList()
    {
        // Arrange - Create a class and add students
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        var studentId1 = Guid.NewGuid();
        var addCommand1 = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId1
        );
        var addResponse1 = WhenCommand(addCommand1);
        
        var studentId2 = Guid.NewGuid();
        var addCommand2 = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId2
        );
        var addResponse2 = WhenCommand(addCommand2);
        
        // Act - Remove one student from class
        var removeCommand = new ClassRemoveStudentCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId1
        );
        var removeResponse = WhenCommand(removeCommand);
        
        // Assert
        Assert.Equal(4, removeResponse.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(removeResponse.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Single(@class.StudentIds);
        Assert.DoesNotContain(studentId1, @class.StudentIds);
        Assert.Contains(studentId2, @class.StudentIds);
    }

    [Fact]
    public void AddSameStudentToClassTwice_ShouldNotDuplicateStudentId()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        
        // Act - Add the same student to class twice
        var studentId = Guid.NewGuid();
        var addCommand1 = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId
        );
        var addResponse1 = WhenCommand(addCommand1);
        
        var addCommand2 = new AddStudentToClassCommand(
            createResponse.PartitionKeys.AggregateId,
            studentId
        );
        var addResponse2 = WhenCommand(addCommand2);
        
        // Assert
        Assert.Equal(3, addResponse2.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(addResponse2.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Single(@class.StudentIds);
        Assert.Contains(studentId, @class.StudentIds);
    }

    [Fact]
    public void CompleteClassSetup_ShouldHaveTeacherAndStudents()
    {
        // Arrange - Create a class
        var createCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var createResponse = GivenCommand(createCommand);
        var classId = createResponse.PartitionKeys.AggregateId;
        
        // Act - Set up a complete class with teacher and students
        var teacherId = Guid.NewGuid();
        var assignTeacherCommand = new ClassAssignTeacherCommand(classId, teacherId);
        var assignTeacherResponse = WhenCommand(assignTeacherCommand);
        
        var studentId1 = Guid.NewGuid();
        var addStudent1Command = new AddStudentToClassCommand(classId, studentId1);
        var addStudent1Response = WhenCommand(addStudent1Command);
        
        var studentId2 = Guid.NewGuid();
        var addStudent2Command = new AddStudentToClassCommand(classId, studentId2);
        var addStudent2Response = WhenCommand(addStudent2Command);
        
        // Assert
        Assert.Equal(4, addStudent2Response.Version);
        
        var aggregate = ThenGetAggregate<ClassProjector>(addStudent2Response.PartitionKeys);
        Assert.IsType<Class>(aggregate.Payload);
        
        var @class = (Class)aggregate.Payload;
        Assert.Equal(teacherId, @class.TeacherId);
        Assert.Equal(2, @class.StudentIds.Count);
        Assert.Contains(studentId1, @class.StudentIds);
        Assert.Contains(studentId2, @class.StudentIds);
    }
}
