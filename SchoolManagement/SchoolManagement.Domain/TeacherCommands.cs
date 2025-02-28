using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record RegisterTeacherCommand(
    string Name,
    string TeacherId,
    string Email,
    string PhoneNumber,
    string Address,
    string Subject
) : ICommandWithHandler<RegisterTeacherCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(RegisterTeacherCommand command) => 
        PartitionKeys.Generate<TeacherProjector>();

    public ResultBox<EventOrNone> Handle(RegisterTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherRegistered(
            command.Name,
            command.TeacherId,
            command.Email,
            command.PhoneNumber,
            command.Address,
            command.Subject));
}

[GenerateSerializer]
public record UpdateTeacherCommand(
    Guid TeacherId,
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null,
    string Subject = null
) : ICommandWithHandler<UpdateTeacherCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(UpdateTeacherCommand command) => 
        PartitionKeys.Existing<TeacherProjector>(command.TeacherId);

    public ResultBox<EventOrNone> Handle(UpdateTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherUpdated(
            command.Name,
            command.Email,
            command.PhoneNumber,
            command.Address,
            command.Subject));
}

[GenerateSerializer]
public record DeleteTeacherCommand(
    Guid TeacherId
) : ICommandWithHandler<DeleteTeacherCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(DeleteTeacherCommand command) => 
        PartitionKeys.Existing<TeacherProjector>(command.TeacherId);

    public ResultBox<EventOrNone> Handle(DeleteTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherDeleted());
}

[GenerateSerializer]
public record AssignTeacherToClassCommand(
    Guid TeacherId,
    Guid ClassId
) : ICommandWithHandler<AssignTeacherToClassCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(AssignTeacherToClassCommand command) => 
        PartitionKeys.Existing<TeacherProjector>(command.TeacherId);

    public ResultBox<EventOrNone> Handle(AssignTeacherToClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherAssignedToClass(command.ClassId));
}

[GenerateSerializer]
public record RemoveTeacherFromClassCommand(
    Guid TeacherId,
    Guid ClassId
) : ICommandWithHandler<RemoveTeacherFromClassCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(RemoveTeacherFromClassCommand command) => 
        PartitionKeys.Existing<TeacherProjector>(command.TeacherId);

    public ResultBox<EventOrNone> Handle(RemoveTeacherFromClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherRemovedFromClass(command.ClassId));
}
