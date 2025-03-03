using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Students;
using SchoolManagement.Domain.Aggregates.Students.Events;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Commands;

[GenerateSerializer]
public record AssignStudentToClassCommand(
    Guid StudentId,
    Guid ClassId
) : ICommandWithHandler<AssignStudentToClassCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(AssignStudentToClassCommand command) => 
        PartitionKeys.Existing<StudentProjector>(command.StudentId);

    public ResultBox<EventOrNone> Handle(AssignStudentToClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new StudentAssignedToClass(command.ClassId));
}