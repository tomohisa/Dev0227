using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassCreated(
    string Name,
    string ClassCode,
    string Description
) : IEventPayload;

[GenerateSerializer]
public record ClassDeleted() : IEventPayload;

[GenerateSerializer]
public record ClassUpdated(
    string Name = null,
    string ClassCode = null,
    string Description = null
) : IEventPayload;

[GenerateSerializer]
public record ClassTeacherAssigned(
    Guid TeacherId
) : IEventPayload;

[GenerateSerializer]
public record ClassTeacherRemoved() : IEventPayload;

[GenerateSerializer]
public record ClassStudentAdded(
    Guid StudentId
) : IEventPayload;

[GenerateSerializer]
public record ClassStudentRemoved(
    Guid StudentId
) : IEventPayload;
