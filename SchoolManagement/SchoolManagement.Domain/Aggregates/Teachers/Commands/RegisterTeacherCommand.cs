using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Teachers.Events;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record RegisterTeacherCommand(
    string Name,
    string TeacherId,
    string Email,
    string PhoneNumber,
    string Address,
    string Subject
) : ICommandWithHandler<RegisterTeacherCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(RegisterTeacherCommand command) => 
        PartitionKeys.Generate<TeacherProjector>();

    public ResultBox<EventOrNone> Handle(RegisterTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherRegistered(
            command.Name,
            command.TeacherId,
            command.Email,
            command.PhoneNumber,
            command.Address,
            command.Subject));
}