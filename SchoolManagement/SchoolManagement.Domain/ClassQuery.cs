using ResultBoxes;
using Sekiban.Pure.Projectors;
using Sekiban.Pure.Query;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record ClassQuery(string NameContains = null, string ClassCodeContains = null)
    : IMultiProjectionListQuery<AggregateListProjector<ClassProjector>, ClassQuery, ClassQuery.ClassRecord>
{
    public static ResultBox<IEnumerable<ClassRecord>> HandleFilter(
        MultiProjectionState<AggregateListProjector<ClassProjector>> projection, 
        ClassQuery query, 
        IQueryContext context)
    {
        var classes = projection.Payload.Aggregates
            .Where(m => m.Value.GetPayload() is Class)
            .Select(m => ((Class)m.Value.GetPayload(), m.Value.PartitionKeys));
            
        if (!string.IsNullOrEmpty(query.NameContains))
        {
            classes = classes.Where(c => c.Item1.Name.Contains(query.NameContains, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrEmpty(query.ClassCodeContains))
        {
            classes = classes.Where(c => c.Item1.ClassCode.Contains(query.ClassCodeContains, StringComparison.OrdinalIgnoreCase));
        }
        
        return classes
            .Select(tuple => new ClassRecord(
                tuple.PartitionKeys.AggregateId,
                tuple.Item1.Name,
                tuple.Item1.ClassCode,
                tuple.Item1.Description,
                tuple.Item1.TeacherId,
                tuple.Item1.StudentIds.ToArray()))
            .ToResultBox();
    }

    public static ResultBox<IEnumerable<ClassRecord>> HandleSort(
        IEnumerable<ClassRecord> filteredList, 
        ClassQuery query, 
        IQueryContext context)
    {
        return filteredList.OrderBy(m => m.Name).AsEnumerable().ToResultBox();
    }

    [GenerateSerializer]
    public record ClassRecord(
        Guid ClassId,
        string Name,
        string ClassCode,
        string Description,
        Guid? TeacherId,
        Guid[] StudentIds
    );
}