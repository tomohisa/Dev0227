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
public record UpdateClassCommand(
    Guid ClassId,
    string Name = null,
    string ClassCode = null,
    string Description = null
) : ICommandWithHandler<UpdateClassCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(UpdateClassCommand command) => 
        PartitionKeys.Existing<ClassProjector>(command.ClassId);

    public ResultBox<EventOrNone> Handle(UpdateClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassUpdated(
            command.Name,
            command.ClassCode,
            command.Description));
}