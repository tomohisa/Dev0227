using Sekiban.Pure.Events;

namespace SchoolManagement.Domain;

[GenerateSerializer]
public record WeatherForecastLocationUpdated(string NewLocation) : IEventPayload;
