using SchoolManagement.Domain.Aggregates.Classes.Events;
using SchoolManagement.Domain.Aggregates.Classes.Payloads;
using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Events;
using Sekiban.Pure.Projectors;

namespace SchoolManagement.Domain.Aggregates.Classes;

public class ClassProjector : IAggregateProjector
{
    public IAggregatePayload Project(IAggregatePayload payload, IEvent ev)
        => (payload, ev.GetPayload()) switch
        {
            (EmptyAggregatePayload, ClassCreated created) => new Class(
                created.Name,
                created.ClassCode,
                created.Description),
                
            (Class @class, ClassDeleted _) => new DeletedClass(
                @class.Name,
                @class.ClassCode,
                @class.Description,
                @class.TeacherId,
                @class.StudentIds),
                
            (Class @class, ClassUpdated updated) => @class with
            {
                Name = updated.Name ?? @class.Name,
                ClassCode = updated.ClassCode ?? @class.ClassCode,
                Description = updated.Description ?? @class.Description
            },
            
            (Class @class, ClassTeacherAssigned assigned) => @class with
            {
                TeacherId = assigned.TeacherId
            },
            
            (Class @class, ClassTeacherRemoved _) => @class with
            {
                TeacherId = null
            },
            
            (Class @class, ClassStudentAdded added) => @class with
            {
                StudentIds = @class.StudentIds.Contains(added.StudentId) 
                    ? @class.StudentIds 
                    : @class.StudentIds.Append(added.StudentId).ToList()
            },
            
            (Class @class, ClassStudentRemoved removed) => @class with
            {
                StudentIds = @class.StudentIds.Where(id => id != removed.StudentId).ToList()
            },
            
            _ => payload
        };
}
