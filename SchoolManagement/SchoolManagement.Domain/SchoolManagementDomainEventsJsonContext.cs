using System.Text.Json.Serialization;
using SchoolManagement.Domain.Aggregates.Classes.Events;
using SchoolManagement.Domain.Aggregates.Students.Events;
using SchoolManagement.Domain.Aggregates.Students.Queries;
using SchoolManagement.Domain.Aggregates.Teachers.Events;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

// Query Types
[JsonSerializable(typeof(StudentIdExistsQuery))]
[JsonSerializable(typeof(TeacherIdExistsQuery))]

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(EventDocumentCommon))]
[JsonSerializable(typeof(EventDocumentCommon[]))]

// WeatherForecast Events
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.WeatherForecastInputted>))]
[JsonSerializable(typeof(SchoolManagement.Domain.WeatherForecastInputted))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.WeatherForecastDeleted>))]
[JsonSerializable(typeof(SchoolManagement.Domain.WeatherForecastDeleted))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.WeatherForecastLocationUpdated>))]
[JsonSerializable(typeof(SchoolManagement.Domain.WeatherForecastLocationUpdated))]

// Student Events
[JsonSerializable(typeof(EventDocument<StudentRegistered>))]
[JsonSerializable(typeof(StudentRegistered))]
[JsonSerializable(typeof(EventDocument<StudentDeleted>))]
[JsonSerializable(typeof(StudentDeleted))]
[JsonSerializable(typeof(EventDocument<StudentUpdated>))]
[JsonSerializable(typeof(StudentUpdated))]
[JsonSerializable(typeof(EventDocument<StudentAssignedToClass>))]
[JsonSerializable(typeof(StudentAssignedToClass))]
[JsonSerializable(typeof(EventDocument<StudentRemovedFromClass>))]
[JsonSerializable(typeof(StudentRemovedFromClass))]

// Teacher Events
[JsonSerializable(typeof(EventDocument<TeacherRegistered>))]
[JsonSerializable(typeof(TeacherRegistered))]
[JsonSerializable(typeof(EventDocument<TeacherDeleted>))]
[JsonSerializable(typeof(TeacherDeleted))]
[JsonSerializable(typeof(EventDocument<TeacherUpdated>))]
[JsonSerializable(typeof(TeacherUpdated))]
[JsonSerializable(typeof(EventDocument<TeacherAssignedToClass>))]
[JsonSerializable(typeof(TeacherAssignedToClass))]
[JsonSerializable(typeof(EventDocument<TeacherRemovedFromClass>))]
[JsonSerializable(typeof(TeacherRemovedFromClass))]

// Class Events
[JsonSerializable(typeof(EventDocument<ClassCreated>))]
[JsonSerializable(typeof(ClassCreated))]
[JsonSerializable(typeof(EventDocument<ClassDeleted>))]
[JsonSerializable(typeof(ClassDeleted))]
[JsonSerializable(typeof(EventDocument<ClassUpdated>))]
[JsonSerializable(typeof(ClassUpdated))]
[JsonSerializable(typeof(EventDocument<ClassTeacherAssigned>))]
[JsonSerializable(typeof(ClassTeacherAssigned))]
[JsonSerializable(typeof(EventDocument<ClassTeacherRemoved>))]
[JsonSerializable(typeof(ClassTeacherRemoved))]
[JsonSerializable(typeof(EventDocument<ClassStudentAdded>))]
[JsonSerializable(typeof(ClassStudentAdded))]
[JsonSerializable(typeof(EventDocument<ClassStudentRemoved>))]
[JsonSerializable(typeof(ClassStudentRemoved))]

public partial class SchoolManagementDomainEventsJsonContext : JsonSerializerContext
{
}
