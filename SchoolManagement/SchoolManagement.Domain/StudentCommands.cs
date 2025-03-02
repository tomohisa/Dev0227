using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record RegisterStudentCommand(
    string Name,
    string StudentId,
    DateTime DateOfBirth,
    string Email,
    string PhoneNumber,
    string Address
) : ICommandWithHandler<RegisterStudentCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(RegisterStudentCommand command) => 
        PartitionKeys.Generate<StudentProjector>();

    public ResultBox<EventOrNone> Handle(RegisterStudentCommand command, ICommandContext<IAggregatePayload> context)
    {
        // For duplicate check, we need to use a different approach
        // We'll add a validation event to check for duplicates in the API layer
        return EventOrNone.Event(new StudentRegistered(
            command.Name,
            command.StudentId,
            command.DateOfBirth,
            command.Email,
            command.PhoneNumber,
            command.Address));
    }
}

[GenerateSerializer]
public record UpdateStudentCommand(
    Guid StudentId,
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null
) : ICommandWithHandler<UpdateStudentCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(UpdateStudentCommand command) => 
        PartitionKeys.Existing<StudentProjector>(command.StudentId);

    public ResultBox<EventOrNone> Handle(UpdateStudentCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new StudentUpdated(
            command.Name,
            command.Email,
            command.PhoneNumber,
            command.Address));
}

[GenerateSerializer]
public record DeleteStudentCommand(
    Guid StudentId
) : ICommandWithHandler<DeleteStudentCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(DeleteStudentCommand command) => 
        PartitionKeys.Existing<StudentProjector>(command.StudentId);

    public ResultBox<EventOrNone> Handle(DeleteStudentCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new StudentDeleted());
}

[GenerateSerializer]
public record AssignStudentToClassCommand(
    Guid StudentId,
    Guid ClassId
) : ICommandWithHandler<AssignStudentToClassCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(AssignStudentToClassCommand command) => 
        PartitionKeys.Existing<StudentProjector>(command.StudentId);

    public ResultBox<EventOrNone> Handle(AssignStudentToClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new StudentAssignedToClass(command.ClassId));
}

[GenerateSerializer]
public record RemoveStudentFromClassCommand(
    Guid StudentId
) : ICommandWithHandler<RemoveStudentFromClassCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(RemoveStudentFromClassCommand command) => 
        PartitionKeys.Existing<StudentProjector>(command.StudentId);

    public ResultBox<EventOrNone> Handle(RemoveStudentFromClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new StudentRemovedFromClass());
}
