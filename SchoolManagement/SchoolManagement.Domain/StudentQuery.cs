using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

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
