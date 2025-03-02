using ResultBoxes;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Executors;

namespace SchoolManagement.Domain.Workflows;

/// <summary>
/// Workflows for checking duplicate IDs before executing commands
/// </summary>
public static class DuplicateCheckWorkflows
{
    /// <summary>
    /// Result type for duplicate check operations
    /// </summary>
    public class DuplicateCheckResult
    {
        public bool IsDuplicate { get; }
        public string? ErrorMessage { get; }
        public object? CommandResult { get; }

        private DuplicateCheckResult(bool isDuplicate, string? errorMessage, object? commandResult)
        {
            IsDuplicate = isDuplicate;
            ErrorMessage = errorMessage;
            CommandResult = commandResult;
        }

        public static DuplicateCheckResult Duplicate(string errorMessage) => 
            new(true, errorMessage, null);

        public static DuplicateCheckResult Success(object commandResult) => 
            new(false, null, commandResult);
    }

    /// <summary>
    /// Checks if a student ID already exists before registering a new student
    /// </summary>
    public static async Task<DuplicateCheckResult> CheckStudentIdDuplicate(
        RegisterStudentCommand command,
        ISekibanExecutor executor)
    {
        // Check if studentId already exists
        var studentIdExists = await executor.QueryAsync(new StudentIdExistsQuery(command.StudentId)).UnwrapBox();
        if (studentIdExists)
        {
            return DuplicateCheckResult.Duplicate($"Student with ID '{command.StudentId}' already exists");
        }
        
        // If no duplicate, proceed with the command
        var result = await executor.CommandAsync(command).UnwrapBox();
        return DuplicateCheckResult.Success(result);
    }
    
    /// <summary>
    /// Checks if a teacher ID already exists before registering a new teacher
    /// </summary>
    public static async Task<DuplicateCheckResult> CheckTeacherIdDuplicate(
        RegisterTeacherCommand command,
        ISekibanExecutor executor)
    {
        // Check if teacherId already exists
        var teacherIdExists = await executor.QueryAsync(new TeacherIdExistsQuery(command.TeacherId)).UnwrapBox();
        if (teacherIdExists)
        {
            return DuplicateCheckResult.Duplicate($"Teacher with ID '{command.TeacherId}' already exists");
        }
        
        // If no duplicate, proceed with the command
        var result = await executor.CommandAsync(command).UnwrapBox();
        return DuplicateCheckResult.Success(result);
    }
}
