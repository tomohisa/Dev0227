using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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