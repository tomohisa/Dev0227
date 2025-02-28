using SchoolManagement.Domain;

namespace SchoolManagement.Web;

public class TeacherApiClient(HttpClient httpClient)
{
    public async Task<TeacherQuery.TeacherRecord[]> GetTeachersAsync(string nameContains = null, string subjectContains = null, CancellationToken cancellationToken = default)
    {
        List<TeacherQuery.TeacherRecord>? teachers = null;
        
        var queryString = "";
        if (!string.IsNullOrEmpty(nameContains))
        {
            queryString += $"nameContains={Uri.EscapeDataString(nameContains)}";
        }
        
        if (!string.IsNullOrEmpty(subjectContains))
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString += "&";
            }
            queryString += $"subjectContains={Uri.EscapeDataString(subjectContains)}";
        }
        
        var url = "/api/teachers";
        if (!string.IsNullOrEmpty(queryString))
        {
            url += $"?{queryString}";
        }

        await foreach (var teacher in httpClient.GetFromJsonAsAsyncEnumerable<TeacherQuery.TeacherRecord>(url, cancellationToken))
        {
            if (teacher is not null)
            {
                teachers ??= [];
                teachers.Add(teacher);
            }
        }

        return teachers?.ToArray() ?? [];
    }
    
    public async Task<TeacherQuery.TeacherRecord> GetTeacherByIdAsync(Guid teacherId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<TeacherQuery.TeacherRecord>($"/api/teachers/{teacherId}", cancellationToken);
    }
    
    public async Task<TeacherQuery.TeacherRecord[]> GetTeachersByClassIdAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        List<TeacherQuery.TeacherRecord>? teachers = null;

        await foreach (var teacher in httpClient.GetFromJsonAsAsyncEnumerable<TeacherQuery.TeacherRecord>($"/api/classes/{classId}/teachers", cancellationToken))
        {
            if (teacher is not null)
            {
                teachers ??= [];
                teachers.Add(teacher);
            }
        }

        return teachers?.ToArray() ?? [];
    }
    
    public async Task RegisterTeacherAsync(
        string name,
        string teacherId,
        string email,
        string phoneNumber,
        string address,
        string subject,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterTeacherCommand(name, teacherId, email, phoneNumber, address, subject);
        await httpClient.PostAsJsonAsync("/api/teachers/register", command, cancellationToken);
    }
    
    public async Task UpdateTeacherAsync(
        Guid teacherId,
        string name = null,
        string email = null,
        string phoneNumber = null,
        string address = null,
        string subject = null,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTeacherCommand(teacherId, name, email, phoneNumber, address, subject);
        await httpClient.PostAsJsonAsync("/api/teachers/update", command, cancellationToken);
    }
    
    public async Task DeleteTeacherAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTeacherCommand(teacherId);
        await httpClient.PostAsJsonAsync("/api/teachers/delete", command, cancellationToken);
    }
    
    public async Task AssignTeacherToClassAsync(
        Guid teacherId,
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var command = new AssignTeacherToClassCommand(teacherId, classId);
        await httpClient.PostAsJsonAsync("/api/teachers/assigntoclass", command, cancellationToken);
    }
    
    public async Task RemoveTeacherFromClassAsync(
        Guid teacherId,
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveTeacherFromClassCommand(teacherId, classId);
        await httpClient.PostAsJsonAsync("/api/teachers/removefromclass", command, cancellationToken);
    }
}
