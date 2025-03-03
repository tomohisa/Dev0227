using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record UpdateStudentCommand(
    Guid StudentId,
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null
) : ICommandWithHandler<UpdateStudentCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(UpdateStudentCommand command) => 
        PartitionKeys.Existing<StudentProjector>(command.StudentId);

    public ResultBox<EventOrNone> Handle(UpdateStudentCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new StudentUpdated(
            command.Name,
            command.Email,
            command.PhoneNumber,
            command.Address));
}