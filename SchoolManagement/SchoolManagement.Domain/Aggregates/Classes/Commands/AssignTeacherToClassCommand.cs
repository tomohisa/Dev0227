using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Teachers.Events;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Commands;

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