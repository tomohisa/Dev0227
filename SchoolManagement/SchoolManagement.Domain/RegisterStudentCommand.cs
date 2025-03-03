using ResultBoxes;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record RegisterStudentCommand(
    string Name,
    string StudentId,
    DateTime DateOfBirth,
    string Email,
    string PhoneNumber,
    string Address
) : ICommandWithHandler<RegisterStudentCommand, StudentProjector>
{
    public PartitionKeys SpecifyPartitionKeys(RegisterStudentCommand command) => 
        PartitionKeys.Generate<StudentProjector>();

    public ResultBox<EventOrNone> Handle(RegisterStudentCommand command, ICommandContext<IAggregatePayload> context)
    {
        // For duplicate check, we need to use a different approach
        // We'll add a validation event to check for duplicates in the API layer
        return EventOrNone.Event(new StudentRegistered(
            command.Name,
            command.StudentId,
            command.DateOfBirth,
            command.Email,
            command.PhoneNumber,
            command.Address));
    }
}