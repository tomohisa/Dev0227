using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record CreateClassCommand(
    string Name,
    string ClassCode,
    string Description
) : ICommandWithHandler<CreateClassCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(CreateClassCommand command) => 
        PartitionKeys.Generate<ClassProjector>();

    public ResultBox<EventOrNone> Handle(CreateClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassCreated(
            command.Name,
            command.ClassCode,
            command.Description));
}

[GenerateSerializer]
public record UpdateClassCommand(
    Guid ClassId,
    string Name = null,
    string ClassCode = null,
    string Description = null
) : ICommandWithHandler<UpdateClassCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(UpdateClassCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(UpdateClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassUpdated(
            command.Name,
            command.ClassCode,
            command.Description));
}

[GenerateSerializer]
public record DeleteClassCommand(
    Guid ClassId
) : ICommandWithHandler<DeleteClassCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(DeleteClassCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(DeleteClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassDeleted());
}

[GenerateSerializer]
public record ClassAssignTeacherCommand(
    Guid ClassId,
    Guid TeacherId
) : ICommandWithHandler<ClassAssignTeacherCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(ClassAssignTeacherCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(ClassAssignTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassTeacherAssigned(command.TeacherId));
}

[GenerateSerializer]
public record ClassRemoveTeacherCommand(
    Guid ClassId
) : ICommandWithHandler<ClassRemoveTeacherCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(ClassRemoveTeacherCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(ClassRemoveTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassTeacherRemoved());
}

[GenerateSerializer]
public record AddStudentToClassCommand(
    Guid ClassId,
    Guid StudentId
) : ICommandWithHandler<AddStudentToClassCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(AddStudentToClassCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(AddStudentToClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassStudentAdded(command.StudentId));
}

[GenerateSerializer]
public record ClassRemoveStudentCommand(
    Guid ClassId,
    Guid StudentId
) : ICommandWithHandler<ClassRemoveStudentCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(ClassRemoveStudentCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(ClassRemoveStudentCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassStudentRemoved(command.StudentId));
}
