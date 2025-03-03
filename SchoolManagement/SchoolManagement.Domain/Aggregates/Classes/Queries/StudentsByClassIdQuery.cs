using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Students;
using SchoolManagement.Domain.Aggregates.Students.Payloads;
using SchoolManagement.Domain.Aggregates.Students.Queries;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain.Aggregates.Classes.Queries;

[GenerateSerializer]
public record StudentsByClassIdQuery(Guid ClassId)
    : IMultiProjectionListQuery<AggregateListProjector<StudentProjector>, StudentsByClassIdQuery, StudentQuery.StudentRecord>
{
    public static ResultBox<IEnumerable<StudentQuery.StudentRecord>> HandleFilter(
        MultiProjectionState<AggregateListProjector<StudentProjector>> projection, 
        StudentsByClassIdQuery query, 
        IQueryContext context)
    {
        return projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Student student && student.ClassId == query.ClassId)
            .Select(m => ((Student)m.Value.GetPayload(), m.Value.PartitionKeys))
            .Select(tuple => new StudentQuery.StudentRecord(
                tuple.PartitionKeys.AggregateId,
                tuple.Item1.Name,
                tuple.Item1.StudentId,
                tuple.Item1.DateOfBirth,
                tuple.Item1.Email,
                tuple.Item1.PhoneNumber,
                tuple.Item1.Address,
                tuple.Item1.ClassId,
                tuple.Item1.GetAge()))
            .ToResultBox();
    }

    public static ResultBox<IEnumerable<StudentQuery.StudentRecord>> HandleSort(
        IEnumerable<StudentQuery.StudentRecord> filteredList, 
        StudentsByClassIdQuery query, 
        IQueryContext context)
    {
        return filteredList.OrderBy(m => m.Name).AsEnumerable().ToResultBox();
    }
}