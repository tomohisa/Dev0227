using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain.Aggregates.Classes.Queries;

[GenerateSerializer]
public record TeachersByClassIdQuery(Guid ClassId)
    : IMultiProjectionListQuery<AggregateListProjector<TeacherProjector>, TeachersByClassIdQuery, TeacherQuery.TeacherRecord>
{
    public static ResultBox<IEnumerable<TeacherQuery.TeacherRecord>> HandleFilter(
        MultiProjectionState<AggregateListProjector<TeacherProjector>> projection, 
        TeachersByClassIdQuery query, 
        IQueryContext context)
    {
        return projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Teacher teacher && teacher.ClassIds.Contains(query.ClassId))
            .Select(m => ((Teacher)m.Value.GetPayload(), m.Value.PartitionKeys))
            .Select(tuple => new TeacherQuery.TeacherRecord(
                tuple.PartitionKeys.AggregateId,
                tuple.Item1.Name,
                tuple.Item1.TeacherId,
                tuple.Item1.Email,
                tuple.Item1.PhoneNumber,
                tuple.Item1.Address,
                tuple.Item1.Subject,
                tuple.Item1.ClassIds.ToArray()))
            .ToResultBox();
    }

    public static ResultBox<IEnumerable<TeacherQuery.TeacherRecord>> HandleSort(
        IEnumerable<TeacherQuery.TeacherRecord> filteredList, 
        TeachersByClassIdQuery query, 
        IQueryContext context)
    {
        return filteredList.OrderBy(m => m.Name).AsEnumerable().ToResultBox();
    }
}