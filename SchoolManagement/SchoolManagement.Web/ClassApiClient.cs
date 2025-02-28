using SchoolManagement.Domain;

namespace SchoolManagement.Web;

public class ClassApiClient(HttpClient httpClient)
{
    public async Task<ClassQuery.ClassRecord[]> GetClassesAsync(string nameContains = null, string classCodeContains = null, CancellationToken cancellationToken = default)
    {
        List<ClassQuery.ClassRecord>? classes = null;
        
        var queryString = "";
        if (!string.IsNullOrEmpty(nameContains))
        {
            queryString += $"nameContains={Uri.EscapeDataString(nameContains)}";
        }
        
        if (!string.IsNullOrEmpty(classCodeContains))
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString += "&";
            }
            queryString += $"classCodeContains={Uri.EscapeDataString(classCodeContains)}";
        }
        
        var url = "/api/classes";
        if (!string.IsNullOrEmpty(queryString))
        {
            url += $"?{queryString}";
        }

        await foreach (var @class in httpClient.GetFromJsonAsAsyncEnumerable<ClassQuery.ClassRecord>(url, cancellationToken))
        {
            if (@class is not null)
            {
                classes ??= [];
                classes.Add(@class);
            }
        }

        return classes?.ToArray() ?? [];
    }
    
    public async Task<ClassQuery.ClassRecord> GetClassByIdAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<ClassQuery.ClassRecord>($"/api/classes/{classId}", cancellationToken);
    }
    
    public async Task<ClassQuery.ClassRecord[]> GetClassesByTeacherIdAsync(Guid teacherId, CancellationToken cancellationToken = default)
    {
        List<ClassQuery.ClassRecord>? classes = null;

        await foreach (var @class in httpClient.GetFromJsonAsAsyncEnumerable<ClassQuery.ClassRecord>($"/api/teachers/{teacherId}/classes", cancellationToken))
        {
            if (@class is not null)
            {
                classes ??= [];
                classes.Add(@class);
            }
        }

        return classes?.ToArray() ?? [];
    }
    
    public async Task<ClassQuery.ClassRecord[]> GetClassesByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        List<ClassQuery.ClassRecord>? classes = null;

        await foreach (var @class in httpClient.GetFromJsonAsAsyncEnumerable<ClassQuery.ClassRecord>($"/api/students/{studentId}/classes", cancellationToken))
        {
            if (@class is not null)
            {
                classes ??= [];
                classes.Add(@class);
            }
        }

        return classes?.ToArray() ?? [];
    }
    
    public async Task CreateClassAsync(
        string name,
        string classCode,
        string description,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateClassCommand(name, classCode, description);
        await httpClient.PostAsJsonAsync("/api/classes/create", command, cancellationToken);
    }
    
    public async Task UpdateClassAsync(
        Guid classId,
        string name = null,
        string classCode = null,
        string description = null,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateClassCommand(classId, name, classCode, description);
        await httpClient.PostAsJsonAsync("/api/classes/update", command, cancellationToken);
    }
    
    public async Task DeleteClassAsync(
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteClassCommand(classId);
        await httpClient.PostAsJsonAsync("/api/classes/delete", command, cancellationToken);
    }
    
    public async Task AssignTeacherToClassAsync(
        Guid classId,
        Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        var command = new ClassAssignTeacherCommand(classId, teacherId);
        await httpClient.PostAsJsonAsync("/api/classes/assignteacher", command, cancellationToken);
    }
    
    public async Task RemoveTeacherFromClassAsync(
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var command = new ClassRemoveTeacherCommand(classId);
        await httpClient.PostAsJsonAsync("/api/classes/removeteacher", command, cancellationToken);
    }
    
    public async Task AddStudentToClassAsync(
        Guid classId,
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var command = new AddStudentToClassCommand(classId, studentId);
        await httpClient.PostAsJsonAsync("/api/classes/addstudent", command, cancellationToken);
    }
    
    public async Task RemoveStudentFromClassAsync(
        Guid classId,
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var command = new ClassRemoveStudentCommand(classId, studentId);
        await httpClient.PostAsJsonAsync("/api/classes/removestudent", command, cancellationToken);
    }
}
