using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record StudentByIdQuery(Guid StudentId)
    : IMultiProjectionQuery<AggregateListProjector<StudentProjector>, StudentByIdQuery, StudentQuery.StudentRecord>
{
    public static ResultBox<StudentQuery.StudentRecord> HandleQuery(
        MultiProjectionState<AggregateListProjector<StudentProjector>> projection,
        StudentByIdQuery query,
        IQueryContext context)
    {
        var student = projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Student && m.Value.PartitionKeys.AggregateId == query.StudentId)
            .Select(m => ((Student)m.Value.GetPayload(), m.Value.PartitionKeys))
            .FirstOrDefault();
            
        if (student == default)
        {
            return new Exception("Student not found");
        }
        
        return new StudentQuery.StudentRecord(
            student.PartitionKeys.AggregateId,
            student.Item1.Name,
            student.Item1.StudentId,
            student.Item1.DateOfBirth,
            student.Item1.Email,
            student.Item1.PhoneNumber,
            student.Item1.Address,
            student.Item1.ClassId,
            student.Item1.GetAge());
    }
}