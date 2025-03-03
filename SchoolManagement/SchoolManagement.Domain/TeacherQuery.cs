using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record TeacherQuery(string NameContains = null, string SubjectContains = null)
    : IMultiProjectionListQuery<AggregateListProjector<TeacherProjector>, TeacherQuery, TeacherQuery.TeacherRecord>
{
    public static ResultBox<IEnumerable<TeacherRecord>> HandleFilter(
        MultiProjectionState<AggregateListProjector<TeacherProjector>> projection, 
        TeacherQuery query, 
        IQueryContext context)
    {
        var teachers = projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Teacher)
            .Select(m => ((Teacher)m.Value.GetPayload(), m.Value.PartitionKeys));
            
        if (!string.IsNullOrEmpty(query.NameContains))
        {
            teachers = teachers.Where(t => t.Item1.Name.Contains(query.NameContains, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrEmpty(query.SubjectContains))
        {
            teachers = teachers.Where(t => t.Item1.Subject.Contains(query.SubjectContains, StringComparison.OrdinalIgnoreCase));
        }
        
        return teachers
            .Select(tuple => new TeacherRecord(
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

    public static ResultBox<IEnumerable<TeacherRecord>> HandleSort(
        IEnumerable<TeacherRecord> filteredList, 
        TeacherQuery query, 
        IQueryContext context)
    {
        return filteredList.OrderBy(m => m.Name).AsEnumerable().ToResultBox();
    }

    [GenerateSerializer]
    public record TeacherRecord(
        Guid TeacherId,
        string Name,
        string TeacherIdNumber,
        string Email,
        string PhoneNumber,
        string Address,
        string Subject,
        Guid[] ClassIds
    );
}