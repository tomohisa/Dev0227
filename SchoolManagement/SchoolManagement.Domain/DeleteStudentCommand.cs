using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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