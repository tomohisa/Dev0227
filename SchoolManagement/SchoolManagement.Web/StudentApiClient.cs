using SchoolManagement.Domain;

namespace SchoolManagement.Web;

public class StudentApiClient(HttpClient httpClient)
{
    public async Task<StudentQuery.StudentRecord[]> GetStudentsAsync(string nameContains = null, string studentIdContains = null, CancellationToken cancellationToken = default)
    {
        List<StudentQuery.StudentRecord>? students = null;
        
        var queryString = "";
        if (!string.IsNullOrEmpty(nameContains))
        {
            queryString += $"nameContains={Uri.EscapeDataString(nameContains)}";
        }
        
        if (!string.IsNullOrEmpty(studentIdContains))
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString += "&";
            }
            queryString += $"studentIdContains={Uri.EscapeDataString(studentIdContains)}";
        }
        
        var url = "/api/students";
        if (!string.IsNullOrEmpty(queryString))
        {
            url += $"?{queryString}";
        }

        await foreach (var student in httpClient.GetFromJsonAsAsyncEnumerable<StudentQuery.StudentRecord>(url, cancellationToken))
        {
            if (student is not null)
            {
                students ??= [];
                students.Add(student);
            }
        }

        return students?.ToArray() ?? [];
    }
    
    public async Task<StudentQuery.StudentRecord> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<StudentQuery.StudentRecord>($"/api/students/{studentId}", cancellationToken);
    }
    
    public async Task<StudentQuery.StudentRecord[]> GetStudentsByClassIdAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        List<StudentQuery.StudentRecord>? students = null;

        await foreach (var student in httpClient.GetFromJsonAsAsyncEnumerable<StudentQuery.StudentRecord>($"/api/classes/{classId}/students", cancellationToken))
        {
            if (student is not null)
            {
                students ??= [];
                students.Add(student);
            }
        }

        return students?.ToArray() ?? [];
    }
    
    public async Task RegisterStudentAsync(
        string name,
        string studentId,
        DateTime dateOfBirth,
        string email,
        string phoneNumber,
        string address,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterStudentCommand(name, studentId, dateOfBirth, email, phoneNumber, address);
        await httpClient.PostAsJsonAsync("/api/students/register", command, cancellationToken);
    }
    
    public async Task UpdateStudentAsync(
        Guid studentId,
        string name = null,
        string email = null,
        string phoneNumber = null,
        string address = null,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateStudentCommand(studentId, name, email, phoneNumber, address);
        await httpClient.PostAsJsonAsync("/api/students/update", command, cancellationToken);
    }
    
    public async Task DeleteStudentAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteStudentCommand(studentId);
        await httpClient.PostAsJsonAsync("/api/students/delete", command, cancellationToken);
    }
    
    public async Task AssignStudentToClassAsync(
        Guid studentId,
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var command = new AssignStudentToClassCommand(studentId, classId);
        await httpClient.PostAsJsonAsync("/api/students/assigntoclass", command, cancellationToken);
    }
    
    public async Task RemoveStudentFromClassAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveStudentFromClassCommand(studentId);
        await httpClient.PostAsJsonAsync("/api/students/removefromclass", command, cancellationToken);
    }
}
