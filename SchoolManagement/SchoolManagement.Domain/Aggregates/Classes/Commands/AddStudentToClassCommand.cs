using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Classes;
using SchoolManagement.Domain.Aggregates.Classes.Events;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain.Aggregates.Classes.Commands;

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