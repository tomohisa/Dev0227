using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Classes;
using SchoolManagement.Domain.Aggregates.Classes.Payloads;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain.Aggregates.Classes.Queries;

[GenerateSerializer]
public record ClassByIdQuery(Guid ClassId)
    : IMultiProjectionQuery<AggregateListProjector<ClassProjector>, ClassByIdQuery, ClassQuery.ClassRecord>
{
    public static ResultBox<ClassQuery.ClassRecord> HandleQuery(
        MultiProjectionState<AggregateListProjector<ClassProjector>> projection,
        ClassByIdQuery query,
        IQueryContext context)
    {
        var @class = projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Class && m.Value.PartitionKeys.AggregateId == query.ClassId)
            .Select(m => ((Class)m.Value.GetPayload(), m.Value.PartitionKeys))
            .FirstOrDefault();
            
        if (@class == default)
        {
            return new Exception("Class not found");
        }
        
        return new ClassQuery.ClassRecord(
            @class.PartitionKeys.AggregateId,
            @class.Item1.Name,
            @class.Item1.ClassCode,
            @class.Item1.Description,
            @class.Item1.TeacherId,
            @class.Item1.StudentIds.ToArray());
    }
}