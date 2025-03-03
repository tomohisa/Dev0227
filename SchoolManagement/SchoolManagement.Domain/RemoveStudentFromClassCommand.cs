using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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