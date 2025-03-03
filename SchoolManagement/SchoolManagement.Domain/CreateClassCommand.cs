using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record CreateClassCommand(
    string Name,
    string ClassCode,
    string Description
) : ICommandWithHandler<CreateClassCommand, ClassProjector>
{
    public PartitionKeys SpecifyPartitionKeys(CreateClassCommand command) => 
        PartitionKeys.Generate<ClassProjector>();

    public ResultBox<EventOrNone> Handle(CreateClassCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new ClassCreated(
            command.Name,
            command.ClassCode,
            command.Description));
}