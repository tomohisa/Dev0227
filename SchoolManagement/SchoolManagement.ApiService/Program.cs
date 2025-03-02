using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using SchoolManagement.Domain;
using SchoolManagement.Domain.Generated;
using ResultBoxes;
using Scalar.AspNetCore;
using Sekiban.Pure.AspNetCore;
using Sekiban.Pure.Command.Handlers;
using Sekiban.Pure.CosmosDb;
using Sekiban.Pure.Orleans.Parts;
using Sekiban.Pure.Postgres;
using SchoolManagement.ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddKeyedAzureTableClient("orleans-sekiban-clustering");
builder.AddKeyedAzureBlobClient("orleans-sekiban-grain-state");
builder.AddKeyedAzureQueueClient("orleans-sekiban-queue");
builder.UseOrleans(
    config =>
    {
        // config.UseDashboard(options => { });
        config.AddAzureQueueStreams("EventStreamProvider", (SiloAzureQueueStreamConfigurator configurator) =>
        {
            configurator.ConfigureAzureQueue(options =>
            {
                options.Configure<IServiceProvider>((queueOptions, sp) =>
                {
                    queueOptions.QueueServiceClient = sp.GetKeyedService<QueueServiceClient>("orleans-sekiban-queue");
                });
            });
        });
        
        // Add grain storage for the stream provider
        config.AddAzureBlobGrainStorage("EventStreamProvider", options =>
        {
            options.Configure<IServiceProvider>((opt, sp) =>
            {
                opt.BlobServiceClient = sp.GetKeyedService<Azure.Storage.Blobs.BlobServiceClient>("orleans-sekiban-grain-state");
            });
        });
    });

builder.Services.AddSingleton(
    SchoolManagementDomainDomainTypes.Generate(SchoolManagementDomainEventsJsonContext.Default.Options));

SekibanSerializationTypesChecker.CheckDomainSerializability(SchoolManagementDomainDomainTypes.Generate());

builder.Services.AddTransient<ICommandMetadataProvider, CommandMetadataProvider>();
builder.Services.AddTransient<IExecutingUserProvider, HttpExecutingUserProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<SekibanOrleansExecutor>();



if (builder.Configuration.GetSection("Sekiban").GetValue<string>("Database")?.ToLower() == "cosmos")
{
    // Cosmos settings
    builder.AddSekibanCosmosDb();
} else
{
    // Postgres settings
    builder.AddSekibanPostgresDb();
}
// Add CORS services and configure a policy that allows all origins
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

var apiRoute = app
    .MapGroup("/api")
    .AddEndpointFilter<ExceptionEndpointFilter>();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Use CORS middleware (must be called before other middleware that sends responses)
app.UseCors();

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

// Student Endpoints
apiRoute.MapGet("/students", async ([FromServices]SekibanOrleansExecutor executor, [FromQuery] string nameContains = null, [FromQuery] string studentIdContains = null) =>
    {
        var list = await executor.QueryAsync(new StudentQuery(nameContains, studentIdContains)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetStudents");

apiRoute.MapGet("/students/{studentId}", async ([FromServices]SekibanOrleansExecutor executor, Guid studentId) =>
    {
        var student = await executor.QueryAsync(new StudentByIdQuery(studentId)).UnwrapBox();
        return student;
    })
    .WithOpenApi()
    .WithName("GetStudentById");

apiRoute.MapGet("/classes/{classId}/students", async ([FromServices]SekibanOrleansExecutor executor, Guid classId) =>
    {
        var list = await executor.QueryAsync(new StudentsByClassIdQuery(classId)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetStudentsByClassId");

apiRoute
    .MapPost(
        "/students/register",
        async (
            [FromBody] RegisterStudentCommand command,
            [FromServices] SekibanOrleansExecutor executor) => 
        {
            // Check for duplicate studentId
            return await DuplicateCheckEndpointFilters.CheckStudentIdDuplicate(command, executor);
        })
    .WithName("RegisterStudent")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/students/update",
        async (
            [FromBody] UpdateStudentCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("UpdateStudent")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/students/delete",
        async (
            [FromBody] DeleteStudentCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("DeleteStudent")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/students/assigntoclass",
        async (
            [FromBody] AssignStudentToClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("AssignStudentToClass")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/students/removefromclass",
        async (
            [FromBody] RemoveStudentFromClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("RemoveStudentFromClass")
    .WithOpenApi();

// Teacher Endpoints
apiRoute.MapGet("/teachers", async ([FromServices]SekibanOrleansExecutor executor, [FromQuery] string nameContains = null, [FromQuery] string subjectContains = null) =>
    {
        var list = await executor.QueryAsync(new TeacherQuery(nameContains, subjectContains)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetTeachers");

apiRoute.MapGet("/teachers/{teacherId}", async ([FromServices]SekibanOrleansExecutor executor, Guid teacherId) =>
    {
        var teacher = await executor.QueryAsync(new TeacherByIdQuery(teacherId)).UnwrapBox();
        return teacher;
    })
    .WithOpenApi()
    .WithName("GetTeacherById");

apiRoute.MapGet("/classes/{classId}/teachers", async ([FromServices]SekibanOrleansExecutor executor, Guid classId) =>
    {
        var list = await executor.QueryAsync(new TeachersByClassIdQuery(classId)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetTeachersByClassId");

apiRoute
    .MapPost(
        "/teachers/register",
        async (
            [FromBody] RegisterTeacherCommand command,
            [FromServices] SekibanOrleansExecutor executor) => 
        {
            // Check for duplicate teacherId
            return await DuplicateCheckEndpointFilters.CheckTeacherIdDuplicate(command, executor);
        })
    .WithName("RegisterTeacher")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/teachers/update",
        async (
            [FromBody] UpdateTeacherCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("UpdateTeacher")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/teachers/delete",
        async (
            [FromBody] DeleteTeacherCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("DeleteTeacher")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/teachers/assigntoclass",
        async (
            [FromBody] AssignTeacherToClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("AssignTeacherToClass")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/teachers/removefromclass",
        async (
            [FromBody] RemoveTeacherFromClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("RemoveTeacherFromClass")
    .WithOpenApi();

// Class Endpoints
apiRoute.MapGet("/classes", async ([FromServices]SekibanOrleansExecutor executor, [FromQuery] string nameContains = null, [FromQuery] string classCodeContains = null) =>
    {
        var list = await executor.QueryAsync(new ClassQuery(nameContains, classCodeContains)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetClasses");

apiRoute.MapGet("/classes/{classId}", async ([FromServices]SekibanOrleansExecutor executor, Guid classId) =>
    {
        var @class = await executor.QueryAsync(new ClassByIdQuery(classId)).UnwrapBox();
        return @class;
    })
    .WithOpenApi()
    .WithName("GetClassById");

apiRoute.MapGet("/teachers/{teacherId}/classes", async ([FromServices]SekibanOrleansExecutor executor, Guid teacherId) =>
    {
        var list = await executor.QueryAsync(new ClassesByTeacherIdQuery(teacherId)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetClassesByTeacherId");

apiRoute.MapGet("/students/{studentId}/classes", async ([FromServices]SekibanOrleansExecutor executor, Guid studentId) =>
    {
        var list = await executor.QueryAsync(new ClassesByStudentIdQuery(studentId)).UnwrapBox();
        return list.Items;
    })
    .WithOpenApi()
    .WithName("GetClassesByStudentId");

apiRoute
    .MapPost(
        "/classes/create",
        async (
            [FromBody] CreateClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("CreateClass")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/classes/update",
        async (
            [FromBody] UpdateClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("UpdateClass")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/classes/delete",
        async (
            [FromBody] DeleteClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("DeleteClass")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/classes/assignteacher",
        async (
            [FromBody] ClassAssignTeacherCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("ClassAssignTeacher")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/classes/removeteacher",
        async (
            [FromBody] ClassRemoveTeacherCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("ClassRemoveTeacher")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/classes/addstudent",
        async (
            [FromBody] AddStudentToClassCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("AddStudentToClass")
    .WithOpenApi();

apiRoute
    .MapPost(
        "/classes/removestudent",
        async (
            [FromBody] ClassRemoveStudentCommand command,
            [FromServices] SekibanOrleansExecutor executor) => await executor.CommandAsync(command).UnwrapBox())
    .WithName("ClassRemoveStudent")
    .WithOpenApi();

app.MapDefaultEndpoints();

app.Run();
