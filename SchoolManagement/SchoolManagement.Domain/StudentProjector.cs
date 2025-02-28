using Sekiban.Pure.Aggregates;
using Sekiban.Pure.Events;
using Sekiban.Pure.Projectors;

namespace SchoolManagement.Domain;

public class StudentProjector : IAggregateProjector
{
    public IAggregatePayload Project(IAggregatePayload payload, IEvent ev)
        => (payload, ev.GetPayload()) switch
        {
            (EmptyAggregatePayload, StudentRegistered registered) => new Student(
                registered.Name,
                registered.StudentId,
                registered.DateOfBirth,
                registered.Email,
                registered.PhoneNumber,
                registered.Address),
                
            (Student student, StudentDeleted _) => new DeletedStudent(
                student.Name,
                student.StudentId,
                student.DateOfBirth,
                student.Email,
                student.PhoneNumber,
                student.Address,
                student.ClassId),
                
            (Student student, StudentUpdated updated) => student with
            {
                Name = updated.Name ?? student.Name,
                Email = updated.Email ?? student.Email,
                PhoneNumber = updated.PhoneNumber ?? student.PhoneNumber,
                Address = updated.Address ?? student.Address
            },
            
            (Student student, StudentAssignedToClass assigned) => student with
            {
                ClassId = assigned.ClassId
            },
            
            (Student student, StudentRemovedFromClass _) => student with
            {
                ClassId = null
            },
            
            _ => payload
        };
}
