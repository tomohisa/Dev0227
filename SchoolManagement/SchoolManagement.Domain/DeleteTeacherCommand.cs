using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

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