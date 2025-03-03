using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Classes;
using SchoolManagement.Domain.Aggregates.Classes.Payloads;
using SchoolManagement.Domain.Aggregates.Classes.Queries;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassesByTeacherIdQuery(Guid TeacherId)
    : IMultiProjectionListQuery<AggregateListProjector<ClassProjector>, ClassesByTeacherIdQuery, ClassQuery.ClassRecord>
{
    public static ResultBox<IEnumerable<ClassQuery.ClassRecord>> HandleFilter(
        MultiProjectionState<AggregateListProjector<ClassProjector>> projection, 
        ClassesByTeacherIdQuery query, 
        IQueryContext context)
    {
        return projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Class @class && @class.TeacherId == query.TeacherId)
            .Select(m => ((Class)m.Value.GetPayload(), m.Value.PartitionKeys))
            .Select(tuple => new ClassQuery.ClassRecord(
                tuple.PartitionKeys.AggregateId,
                tuple.Item1.Name,
                tuple.Item1.ClassCode,
                tuple.Item1.Description,
                tuple.Item1.TeacherId,
                tuple.Item1.StudentIds.ToArray()))
            .ToResultBox();
    }

    public static ResultBox<IEnumerable<ClassQuery.ClassRecord>> HandleSort(
        IEnumerable<ClassQuery.ClassRecord> filteredList, 
        ClassesByTeacherIdQuery query, 
        IQueryContext context)
    {
        return filteredList.OrderBy(m => m.Name).AsEnumerable().ToResultBox();
    }
}