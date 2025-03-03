using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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