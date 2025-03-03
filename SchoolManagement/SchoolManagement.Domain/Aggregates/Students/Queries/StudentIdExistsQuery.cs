using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Students;
using SchoolManagement.Domain.Aggregates.Students.Payloads;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain.Aggregates.Students.Queries;

[GenerateSerializer]
public record StudentIdExistsQuery(string StudentId)
    : IMultiProjectionQuery<AggregateListProjector<StudentProjector>, StudentIdExistsQuery, bool>
{
    public static ResultBox<bool> HandleQuery(
        MultiProjectionState<AggregateListProjector<StudentProjector>> projection,
        StudentIdExistsQuery query,
        IQueryContext context)
    {
        return projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Student)
            .Select(m => (Student)m.Value.GetPayload())
            .Any(student => student.StudentId == query.StudentId)
            .ToResultBox();
    }
}
