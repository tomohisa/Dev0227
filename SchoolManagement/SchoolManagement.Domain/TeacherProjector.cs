using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Events;
using Sekiban.Pure.Projectors;

namespace SchoolManagement.Domain;

public class TeacherProjector : IAggregateProjector
{
    public IAggregatePayload Project(IAggregatePayload payload, IEvent ev)
        => (payload, ev.GetPayload()) switch
        {
            (EmptyAggregatePayload, TeacherRegistered registered) => new Teacher(
                registered.Name,
                registered.TeacherId,
                registered.Email,
                registered.PhoneNumber,
                registered.Address,
                registered.Subject),
                
            (Teacher teacher, TeacherDeleted _) => new DeletedTeacher(
                teacher.Name,
                teacher.TeacherId,
                teacher.Email,
                teacher.PhoneNumber,
                teacher.Address,
                teacher.Subject,
                teacher.ClassIds),
                
            (Teacher teacher, TeacherUpdated updated) => teacher with
            {
                Name = updated.Name ?? teacher.Name,
                Email = updated.Email ?? teacher.Email,
                PhoneNumber = updated.PhoneNumber ?? teacher.PhoneNumber,
                Address = updated.Address ?? teacher.Address,
                Subject = updated.Subject ?? teacher.Subject
            },
            
            (Teacher teacher, TeacherAssignedToClass assigned) => teacher with
            {
                ClassIds = teacher.ClassIds.Contains(assigned.ClassId) 
                    ? teacher.ClassIds 
                    : teacher.ClassIds.Append(assigned.ClassId).ToList()
            },
            
            (Teacher teacher, TeacherRemovedFromClass removed) => teacher with
            {
                ClassIds = teacher.ClassIds.Where(id => id != removed.ClassId).ToList()
            },
            
            _ => payload
        };
}
