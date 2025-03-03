using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Teachers.Events;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Commands;

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