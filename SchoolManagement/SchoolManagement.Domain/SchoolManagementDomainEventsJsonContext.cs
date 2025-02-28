using System.Text.Json.Serialization;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.StudentRegistered>))]
[JsonSerializable(typeof(SchoolManagement.Domain.StudentRegistered))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.StudentDeleted>))]
[JsonSerializable(typeof(SchoolManagement.Domain.StudentDeleted))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.StudentUpdated>))]
[JsonSerializable(typeof(SchoolManagement.Domain.StudentUpdated))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.StudentAssignedToClass>))]
[JsonSerializable(typeof(SchoolManagement.Domain.StudentAssignedToClass))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.StudentRemovedFromClass>))]
[JsonSerializable(typeof(SchoolManagement.Domain.StudentRemovedFromClass))]

// Teacher Events
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.TeacherRegistered>))]
[JsonSerializable(typeof(SchoolManagement.Domain.TeacherRegistered))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.TeacherDeleted>))]
[JsonSerializable(typeof(SchoolManagement.Domain.TeacherDeleted))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.TeacherUpdated>))]
[JsonSerializable(typeof(SchoolManagement.Domain.TeacherUpdated))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.TeacherAssignedToClass>))]
[JsonSerializable(typeof(SchoolManagement.Domain.TeacherAssignedToClass))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.TeacherRemovedFromClass>))]
[JsonSerializable(typeof(SchoolManagement.Domain.TeacherRemovedFromClass))]

// Class Events
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassCreated>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassCreated))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassDeleted>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassDeleted))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassUpdated>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassUpdated))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassTeacherAssigned>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassTeacherAssigned))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassTeacherRemoved>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassTeacherRemoved))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassStudentAdded>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassStudentAdded))]
[JsonSerializable(typeof(EventDocument<SchoolManagement.Domain.ClassStudentRemoved>))]
[JsonSerializable(typeof(SchoolManagement.Domain.ClassStudentRemoved))]

public partial class SchoolManagementDomainEventsJsonContext : JsonSerializerContext
{
}
