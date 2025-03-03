using ResultBoxes;
using SchoolManagement.Domain.Aggregates.Students;
using SchoolManagement.Domain.Aggregates.Students.Payloads;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain.Aggregates.Students.Queries;

[GenerateSerializer]
public record StudentQuery(string NameContains = null, string StudentIdContains = null)
    : IMultiProjectionListQuery<AggregateListProjector<StudentProjector>, StudentQuery, StudentQuery.StudentRecord>
{
    public static ResultBox<IEnumerable<StudentRecord>> HandleFilter(
        MultiProjectionState<AggregateListProjector<StudentProjector>> projection, 
        StudentQuery query, 
        IQueryContext context)
    {
        var students = projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Student)
            .Select(m => ((Student)m.Value.GetPayload(), m.Value.PartitionKeys));
            
        if (!string.IsNullOrEmpty(query.NameContains))
        {
            students = students.Where(s => s.Item1.Name.Contains(query.NameContains, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrEmpty(query.StudentIdContains))
        {
            students = students.Where(s => s.Item1.StudentId.Contains(query.StudentIdContains, StringComparison.OrdinalIgnoreCase));
        }
        
        return students
            .Select(tuple => new StudentRecord(
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

    public static ResultBox<IEnumerable<StudentRecord>> HandleSort(
        IEnumerable<StudentRecord> filteredList, 
        StudentQuery query, 
        IQueryContext context)
    {
        return filteredList.OrderBy(m => m.Name).AsEnumerable().ToResultBox();
    }

    [GenerateSerializer]
    public record StudentRecord(
        Guid StudentId,
        string Name,
        string StudentIdNumber,
        DateTime DateOfBirth,
        string Email,
        string PhoneNumber,
        string Address,
        Guid? ClassId,
        int Age
    );
}
