using Sekiban.Pure.xUnit;
using Xunit;
using System;
using System.Linq;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Generated;
using Sekiban.Pure;

namespace SchoolManagement.Domain.Tests;

public class RelationshipTests : SekibanInMemoryTestBase
{
    protected override SekibanDomainTypes GetDomainTypes() => 
        SchoolManagementDomainDomainTypes.Generate(SchoolManagementDomainEventsJsonContext.Default.Options);

    [Fact]
    public void StudentClassRelationship_ShouldBeConsistent()
    {
        // Arrange - Create a class and a student
        var createClassCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var classResponse = GivenCommand(createClassCommand);
        var classId = classResponse.PartitionKeys.AggregateId;
        
        var registerStudentCommand = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var studentResponse = GivenCommand(registerStudentCommand);
        var studentId = studentResponse.PartitionKeys.AggregateId;
        
        // Act - Create bidirectional relationship
        var addStudentToClassCommand = new AddStudentToClassCommand(classId, studentId);
        var addStudentResponse = WhenCommand(addStudentToClassCommand);
        
        var assignStudentToClassCommand = new AssignStudentToClassCommand(studentId, classId);
        var assignStudentResponse = WhenCommand(assignStudentToClassCommand);
        
        // Assert
        var classAggregate = ThenGetAggregate<ClassProjector>(classResponse.PartitionKeys);
        var studentAggregate = ThenGetAggregate<StudentProjector>(studentResponse.PartitionKeys);
        
        var @class = (Class)classAggregate.Payload;
        var student = (Student)studentAggregate.Payload;
        
        // Verify bidirectional relationship
        Assert.Contains(studentId, @class.StudentIds);
        Assert.Equal(classId, student.ClassId);
    }

    [Fact]
    public void TeacherClassRelationship_ShouldBeConsistent()
    {
        // Arrange - Create a class and a teacher
        var createClassCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var classResponse = GivenCommand(createClassCommand);
        var classId = classResponse.PartitionKeys.AggregateId;
        
        var registerTeacherCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var teacherResponse = GivenCommand(registerTeacherCommand);
        var teacherId = teacherResponse.PartitionKeys.AggregateId;
        
        // Act - Create bidirectional relationship
        var assignTeacherToClassCommand = new ClassAssignTeacherCommand(classId, teacherId);
        var assignTeacherResponse = WhenCommand(assignTeacherToClassCommand);
        
        var assignClassToTeacherCommand = new AssignTeacherToClassCommand(teacherId, classId);
        var assignClassResponse = WhenCommand(assignClassToTeacherCommand);
        
        // Assert
        var classAggregate = ThenGetAggregate<ClassProjector>(classResponse.PartitionKeys);
        var teacherAggregate = ThenGetAggregate<TeacherProjector>(teacherResponse.PartitionKeys);
        
        var @class = (Class)classAggregate.Payload;
        var teacher = (Teacher)teacherAggregate.Payload;
        
        // Verify bidirectional relationship
        Assert.Equal(teacherId, @class.TeacherId);
        Assert.Contains(classId, teacher.ClassIds);
    }

    [Fact]
    public void CompleteSchoolRelationships_ShouldBeConsistent()
    {
        // Arrange - Create a teacher, a class, and two students
        var registerTeacherCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var teacherResponse = GivenCommand(registerTeacherCommand);
        var teacherId = teacherResponse.PartitionKeys.AggregateId;
        
        var createClassCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var classResponse = GivenCommand(createClassCommand);
        var classId = classResponse.PartitionKeys.AggregateId;
        
        var registerStudent1Command = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var student1Response = GivenCommand(registerStudent1Command);
        var student1Id = student1Response.PartitionKeys.AggregateId;
        
        var registerStudent2Command = new RegisterStudentCommand(
            "Jane Doe",
            "S67890",
            new DateTime(2001, 2, 2),
            "jane.doe@example.com",
            "987-654-3210",
            "456 Oak Ave"
        );
        var student2Response = GivenCommand(registerStudent2Command);
        var student2Id = student2Response.PartitionKeys.AggregateId;
        
        // Act - Create all relationships
        // 1. Assign teacher to class (both directions)
        var assignTeacherToClassCommand = new ClassAssignTeacherCommand(classId, teacherId);
        WhenCommand(assignTeacherToClassCommand);
        
        var assignClassToTeacherCommand = new AssignTeacherToClassCommand(teacherId, classId);
        WhenCommand(assignClassToTeacherCommand);
        
        // 2. Add students to class (both directions)
        var addStudent1ToClassCommand = new AddStudentToClassCommand(classId, student1Id);
        WhenCommand(addStudent1ToClassCommand);
        
        var assignStudent1ToClassCommand = new AssignStudentToClassCommand(student1Id, classId);
        WhenCommand(assignStudent1ToClassCommand);
        
        var addStudent2ToClassCommand = new AddStudentToClassCommand(classId, student2Id);
        WhenCommand(addStudent2ToClassCommand);
        
        var assignStudent2ToClassCommand = new AssignStudentToClassCommand(student2Id, classId);
        WhenCommand(assignStudent2ToClassCommand);
        
        // Assert
        var classAggregate = ThenGetAggregate<ClassProjector>(classResponse.PartitionKeys);
        var teacherAggregate = ThenGetAggregate<TeacherProjector>(teacherResponse.PartitionKeys);
        var student1Aggregate = ThenGetAggregate<StudentProjector>(student1Response.PartitionKeys);
        var student2Aggregate = ThenGetAggregate<StudentProjector>(student2Response.PartitionKeys);
        
        var @class = (Class)classAggregate.Payload;
        var teacher = (Teacher)teacherAggregate.Payload;
        var student1 = (Student)student1Aggregate.Payload;
        var student2 = (Student)student2Aggregate.Payload;
        
        // Verify all relationships
        Assert.Equal(teacherId, @class.TeacherId);
        Assert.Contains(classId, teacher.ClassIds);
        
        Assert.Equal(2, @class.StudentIds.Count);
        Assert.Contains(student1Id, @class.StudentIds);
        Assert.Contains(student2Id, @class.StudentIds);
        
        Assert.Equal(classId, student1.ClassId);
        Assert.Equal(classId, student2.ClassId);
    }

    [Fact]
    public void RemoveRelationships_ShouldBeConsistent()
    {
        // Arrange - Create a complete school setup
        var registerTeacherCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var teacherResponse = GivenCommand(registerTeacherCommand);
        var teacherId = teacherResponse.PartitionKeys.AggregateId;
        
        var createClassCommand = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var classResponse = GivenCommand(createClassCommand);
        var classId = classResponse.PartitionKeys.AggregateId;
        
        var registerStudentCommand = new RegisterStudentCommand(
            "John Doe",
            "S12345",
            new DateTime(2000, 1, 1),
            "john.doe@example.com",
            "123-456-7890",
            "123 Main St"
        );
        var studentResponse = GivenCommand(registerStudentCommand);
        var studentId = studentResponse.PartitionKeys.AggregateId;
        
        // Create relationships
        WhenCommand(new ClassAssignTeacherCommand(classId, teacherId));
        WhenCommand(new AssignTeacherToClassCommand(teacherId, classId));
        WhenCommand(new AddStudentToClassCommand(classId, studentId));
        WhenCommand(new AssignStudentToClassCommand(studentId, classId));
        
        // Act - Remove relationships
        var removeTeacherCommand = new ClassRemoveTeacherCommand(classId);
        WhenCommand(removeTeacherCommand);
        
        var removeTeacherFromClassCommand = new RemoveTeacherFromClassCommand(teacherId, classId);
        WhenCommand(removeTeacherFromClassCommand);
        
        var removeStudentCommand = new ClassRemoveStudentCommand(classId, studentId);
        WhenCommand(removeStudentCommand);
        
        var removeStudentFromClassCommand = new RemoveStudentFromClassCommand(studentId);
        WhenCommand(removeStudentFromClassCommand);
        
        // Assert
        var classAggregate = ThenGetAggregate<ClassProjector>(classResponse.PartitionKeys);
        var teacherAggregate = ThenGetAggregate<TeacherProjector>(teacherResponse.PartitionKeys);
        var studentAggregate = ThenGetAggregate<StudentProjector>(studentResponse.PartitionKeys);
        
        var @class = (Class)classAggregate.Payload;
        var teacher = (Teacher)teacherAggregate.Payload;
        var student = (Student)studentAggregate.Payload;
        
        // Verify all relationships are removed
        Assert.Null(@class.TeacherId);
        Assert.Empty(teacher.ClassIds);
        Assert.Empty(@class.StudentIds);
        Assert.Null(student.ClassId);
    }

    [Fact]
    public void TeacherWithMultipleClasses_ShouldTrackAllClasses()
    {
        // Arrange - Create a teacher and multiple classes
        var registerTeacherCommand = new RegisterTeacherCommand(
            "Jane Smith",
            "T54321",
            "jane.smith@example.com",
            "123-456-7890",
            "123 Main St",
            "Mathematics"
        );
        var teacherResponse = GivenCommand(registerTeacherCommand);
        var teacherId = teacherResponse.PartitionKeys.AggregateId;
        
        var createClass1Command = new CreateClassCommand(
            "Mathematics 101",
            "MATH101",
            "Introduction to Mathematics"
        );
        var class1Response = GivenCommand(createClass1Command);
        var class1Id = class1Response.PartitionKeys.AggregateId;
        
        var createClass2Command = new CreateClassCommand(
            "Mathematics 201",
            "MATH201",
            "Advanced Mathematics"
        );
        var class2Response = GivenCommand(createClass2Command);
        var class2Id = class2Response.PartitionKeys.AggregateId;
        
        // Act - Assign teacher to multiple classes
        WhenCommand(new ClassAssignTeacherCommand(class1Id, teacherId));
        WhenCommand(new AssignTeacherToClassCommand(teacherId, class1Id));
        
        WhenCommand(new ClassAssignTeacherCommand(class2Id, teacherId));
        WhenCommand(new AssignTeacherToClassCommand(teacherId, class2Id));
        
        // Assert
        var teacherAggregate = ThenGetAggregate<TeacherProjector>(teacherResponse.PartitionKeys);
        var class1Aggregate = ThenGetAggregate<ClassProjector>(class1Response.PartitionKeys);
        var class2Aggregate = ThenGetAggregate<ClassProjector>(class2Response.PartitionKeys);
        
        var teacher = (Teacher)teacherAggregate.Payload;
        var class1 = (Class)class1Aggregate.Payload;
        var class2 = (Class)class2Aggregate.Payload;
        
        // Verify teacher is assigned to both classes
        Assert.Equal(2, teacher.ClassIds.Count);
        Assert.Contains(class1Id, teacher.ClassIds);
        Assert.Contains(class2Id, teacher.ClassIds);
        
        // Verify each class has the teacher assigned
        Assert.Equal(teacherId, class1.TeacherId);
        Assert.Equal(teacherId, class2.TeacherId);
    }
}
