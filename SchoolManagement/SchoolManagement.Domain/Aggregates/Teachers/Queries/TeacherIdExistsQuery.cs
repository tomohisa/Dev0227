using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record TeacherIdExistsQuery(string TeacherId)
    : IMultiProjectionQuery<AggregateListProjector<TeacherProjector>, TeacherIdExistsQuery, bool>
{
    public static ResultBox<bool> HandleQuery(
        MultiProjectionState<AggregateListProjector<TeacherProjector>> projection,
        TeacherIdExistsQuery query,
        IQueryContext context)
    {
        return projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Teacher)
            .Select(m => (Teacher)m.Value.GetPayload())
            .Any(teacher => teacher.TeacherId == query.TeacherId)
            .ToResultBox();
    }
}
