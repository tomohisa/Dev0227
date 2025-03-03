using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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