using Orleans;
using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record WeatherForecastDeleted() : IEventPayload;
