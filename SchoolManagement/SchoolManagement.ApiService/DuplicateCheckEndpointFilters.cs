using Microsoft.AspNetCore.Mvc;
using ResultBoxes;
using Sekiban.Pure.Command.Executor;
using Sekiban.Pure.Orleans.Parts;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Aggregates.Students.Commands;
using SchoolManagement.Domain.Aggregates.Students.Queries;

namespace SchoolManagement.ApiService;

public static class DuplicateCheckEndpointFilters
{
    public static async Task<object> CheckStudentIdDuplicate(
        RegisterStudentCommand command,
        SekibanOrleansExecutor executor)
    {
        // Check if studentId already exists
        var studentIdExists = await executor.QueryAsync(new StudentIdExistsQuery(command.StudentId)).UnwrapBox();
        if (studentIdExists)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Duplicate StudentId",
                detail: $"Student with ID '{command.StudentId}' already exists");
        }
        
        // If no duplicate, proceed with the command
        return await executor.CommandAsync(command).UnwrapBox();
    }
    
    public static async Task<object> CheckTeacherIdDuplicate(
        RegisterTeacherCommand command,
        SekibanOrleansExecutor executor)
    {
        // Check if teacherId already exists
        var teacherIdExists = await executor.QueryAsync(new TeacherIdExistsQuery(command.TeacherId)).UnwrapBox();
        if (teacherIdExists)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Duplicate TeacherId",
                detail: $"Teacher with ID '{command.TeacherId}' already exists");
        }
        
        // If no duplicate, proceed with the command
        return await executor.CommandAsync(command).UnwrapBox();
    }
}
