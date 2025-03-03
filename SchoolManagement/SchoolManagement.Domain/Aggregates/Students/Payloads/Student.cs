using Sekiban.Pure.Aggregates;

namespace SchoolManagement.Domain.Aggregates.Students.Payloads;

[GenerateSerializer]
public record Student(
    string Name,
    string StudentId,
    DateTime DateOfBirth,
    string Email,
    string PhoneNumber,
    string Address,
    Guid? ClassId = null
) : IAggregatePayload
{
    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}
