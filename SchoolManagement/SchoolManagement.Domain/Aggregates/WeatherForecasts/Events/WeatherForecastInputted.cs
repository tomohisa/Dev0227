using Orleans;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record WeatherForecastInputted(
    string Location,
    DateOnly Date,
    int TemperatureC,
    string Summary
) : IEventPayload;