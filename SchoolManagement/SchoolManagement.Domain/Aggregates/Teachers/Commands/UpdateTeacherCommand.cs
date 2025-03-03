using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Teachers.Events;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.Documents;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record UpdateTeacherCommand(
    Guid TeacherId,
    string Name = null,
    string Email = null,
    string PhoneNumber = null,
    string Address = null,
    string Subject = null
) : ICommandWithHandler<UpdateTeacherCommand, TeacherProjector>
{
    public PartitionKeys SpecifyPartitionKeys(UpdateTeacherCommand command) => 
        PartitionKeys.Existing<TeacherProjector>(command.TeacherId);

    public ResultBox<EventOrNone> Handle(UpdateTeacherCommand command, ICommandContext<IAggregatePayload> context)
        => EventOrNone.Event(new TeacherUpdated(
            command.Name,
            command.Email,
            command.PhoneNumber,
            command.Address,
            command.Subject));
}