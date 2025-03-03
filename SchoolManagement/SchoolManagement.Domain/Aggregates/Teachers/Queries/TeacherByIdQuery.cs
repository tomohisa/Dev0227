using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record TeacherByIdQuery(Guid TeacherId)
    : IMultiProjectionQuery<AggregateListProjector<TeacherProjector>, TeacherByIdQuery, TeacherQuery.TeacherRecord>
{
    public static ResultBox<TeacherQuery.TeacherRecord> HandleQuery(
        MultiProjectionState<AggregateListProjector<TeacherProjector>> projection,
        TeacherByIdQuery query,
        IQueryContext context)
    {
        var teacher = projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Teacher && m.Value.PartitionKeys.AggregateId == query.TeacherId)
            .Select(m => ((Teacher)m.Value.GetPayload(), m.Value.PartitionKeys))
            .FirstOrDefault();
            
        if (teacher == default)
        {
            return new Exception("Teacher not found");
        }
        
        return new TeacherQuery.TeacherRecord(
            teacher.PartitionKeys.AggregateId,
            teacher.Item1.Name,
            teacher.Item1.TeacherId,
            teacher.Item1.Email,
            teacher.Item1.PhoneNumber,
            teacher.Item1.Address,
            teacher.Item1.Subject,
            teacher.Item1.ClassIds.ToArray());
    }
}