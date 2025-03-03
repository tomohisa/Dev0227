# Technical Context: School Management System

## Technology Stack

The School Management System is built using a modern .NET technology stack with a focus on event sourcing, distributed computing, and web technologies.

### Core Technologies

1. **ASP.NET Core**
   - Framework version: .NET 8
   - Web API for backend services
   - Dependency injection for service management
   - Configuration management
   - Middleware pipeline for request processing

2. **Blazor**
   - Server-side Blazor for interactive web UI
   - Component-based architecture
   - Razor syntax for templating
   - JavaScript interoperability for browser interactions
   - Form validation and state management

3. **Sekiban Event Sourcing Framework**
   - Event-based persistence model
   - Command handling and validation
   - Event projection for state reconstruction
   - Query handling for read operations
   - Aggregate management
   - Source generation for domain types
   - In-memory testing capabilities

4. **Orleans**
   - Distributed actor model
   - Virtual actor abstraction
   - Grain-based processing
   - Distributed state management
   - Scalable, resilient architecture

5. **Bootstrap**
   - Responsive UI components
   - Grid system for layout
   - Modal dialogs for forms
   - Styling and theming
   - Mobile-first design approach

### Data Storage

1. **Event Store**
   - Options: Cosmos DB or PostgreSQL (configurable)
   - Append-only event storage
   - Optimized for event sourcing patterns
   - Transactional consistency

2. **Read Models**
   - In-memory projections for queries
   - Optimized for specific query patterns
   - Automatically rebuilt from events

### Development Tools

1. **Visual Studio / VS Code**
   - Primary development environment
   - Debugging and diagnostics
   - IntelliSense for code completion
   - Extension support

2. **Playwright**
   - End-to-end testing framework
   - Browser automation
   - Test assertions and validation
   - Cross-browser testing support

3. **Aspire**
   - .NET Aspire for service orchestration
   - Dashboard for service monitoring
   - Service discovery and configuration
   - Local development environment

4. **xUnit**
   - Unit testing framework
   - Integration with Sekiban for domain testing
   - Support for in-memory testing
   - Assertion capabilities

## Project Structure

```
SchoolManagement/
├── SchoolManagement.Domain/           # Domain models, events, commands
├── SchoolManagement.ApiService/       # API endpoints
├── SchoolManagement.Web/              # Blazor web frontend
├── SchoolManagement.AppHost/          # Aspire host for services
├── SchoolManagement.ServiceDefaults/  # Common service configurations
├── SchoolManagement.Playwright/       # End-to-end tests
└── SchoolManagement.Domain.Tests/     # Domain unit tests
```

### Key Components

1. **SchoolManagement.Domain**
   - Contains domain models (Student, Teacher, Class, WeatherForecast)
   - Defines commands and events
   - Implements projectors for state management
   - Defines queries for data retrieval
   - JSON serialization context for AOT compilation
   - Generated domain types via Sekiban.Pure.SourceGenerator
   - Organized in a DDD-style folder structure:
     ```
     SchoolManagement.Domain/
     ├── Aggregates/                    # Domain aggregates
     │   ├── Classes/                   # Class aggregate
     │   │   ├── ClassProjector.cs      # Projector for Class aggregate
     │   │   ├── Commands/              # Class commands
     │   │   ├── Events/                # Class events
     │   │   ├── Payloads/              # Class state (Class, DeletedClass)
     │   │   └── Queries/               # Class queries
     │   ├── Students/                  # Student aggregate
     │   │   ├── StudentProjector.cs    # Projector for Student aggregate
     │   │   ├── Commands/              # Student commands
     │   │   ├── Events/                # Student events
     │   │   ├── Payloads/              # Student state (Student, DeletedStudent)
     │   │   └── Queries/               # Student queries
     │   ├── Teachers/                  # Teacher aggregate
     │   │   ├── TeacherProjector.cs    # Projector for Teacher aggregate
     │   │   ├── Commands/              # Teacher commands
     │   │   ├── Events/                # Teacher events
     │   │   ├── Payloads/              # Teacher state (Teacher, DeletedTeacher)
     │   │   └── Queries/               # Teacher queries
     │   └── WeatherForecasts/          # WeatherForecast aggregate
     │       ├── WeatherForecastProjector.cs  # Projector for WeatherForecast
     │       ├── Commands/              # WeatherForecast commands
     │       ├── Events/                # WeatherForecast events
     │       ├── Payloads/              # WeatherForecast state
     │       └── Queries/               # WeatherForecast queries
     ├── Workflows/                     # Domain workflows
     │   └── DuplicateCheckWorkflows.cs # Workflows for duplicate checking
     └── SchoolManagementDomainEventsJsonContext.cs  # JSON serialization context
     ```

2. **SchoolManagement.ApiService**
   - Exposes API endpoints for commands and queries
   - Configures Orleans and Sekiban
   - Handles exception filtering
   - Manages API routing

3. **SchoolManagement.Web**
   - Implements Blazor components for UI
   - Contains page layouts and navigation
   - Implements API clients for backend communication
   - Manages UI state and user interactions
   - Handles form validation and submission

4. **SchoolManagement.AppHost**
   - Orchestrates services using Aspire
   - Configures service endpoints
   - Manages service dependencies
   - Provides development dashboard

5. **SchoolManagement.ServiceDefaults**
   - Contains common service configurations
   - Implements shared middleware
   - Configures logging and telemetry
   - Defines default service behaviors

6. **SchoolManagement.Playwright**
   - Contains end-to-end tests
   - Implements page object models
   - Defines test scenarios
   - Provides test utilities and helpers

7. **SchoolManagement.Domain.Tests**
   - Contains unit tests for domain logic
   - Uses Sekiban's in-memory testing framework
   - Tests commands, events, and projectors
   - Verifies entity relationships
   - Follows Given-When-Then pattern

## Development Setup

### Prerequisites

- .NET 8 SDK
- Node.js (for frontend tooling)
- Docker (optional, for containerization)
- Visual Studio 2022 or VS Code with C# extensions

### Local Development

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SchoolManagement
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   cd SchoolManagement.AppHost
   dotnet run --launch-profile https
   ```

4. **Access the application**
   - Web frontend: https://localhost:7201
   - API service: https://localhost:7202

### Testing

1. **Run domain unit tests**
   ```bash
   dotnet test SchoolManagement.Domain.Tests
   ```

2. **Run end-to-end tests**
   ```bash
   cd SchoolManagement.Playwright
   dotnet test
   ```

### Configuration

The application uses the standard ASP.NET Core configuration system with settings in:

1. **appsettings.json** - Base configuration
2. **appsettings.Development.json** - Development overrides
3. **Environment variables** - Runtime configuration
4. **User secrets** - Local development secrets

Key configuration sections:

```json
{
  "Sekiban": {
    "Database": "Cosmos"  // or "Postgres"
  }
}
```

## Deployment

### Production Requirements

- .NET 8 runtime
- Database (Cosmos DB or PostgreSQL)
- HTTPS certificate
- Sufficient memory for Orleans silo

### Deployment Options

1. **Azure App Service**
   - Managed hosting platform
   - Easy scaling and management
   - Integrated with Azure DevOps

2. **Docker Containers**
   - Containerized deployment
   - Kubernetes orchestration
   - Consistent environments

3. **On-premises**
   - Traditional server deployment
   - Full control over infrastructure
   - Manual scaling and management

## Technical Constraints

1. **Event Sourcing Considerations**
   - Event schema evolution strategy required
   - Eventual consistency model
   - Projection rebuilding for schema changes
   - Event versioning for backward compatibility

2. **Orleans Constraints**
   - Cluster management overhead
   - Grain persistence configuration
   - Silo hosting requirements
   - State management complexity

3. **Blazor Limitations**
   - Server-side rendering performance considerations
   - SignalR dependency for real-time updates
   - JavaScript interop overhead
   - Initial load time considerations

4. **Testing Constraints**
   - Domain tests rely on source-generated types
   - Need to reference correct namespaces for testing
   - In-memory testing doesn't validate database interactions
   - Need to understand Sekiban's testing patterns

## Performance Considerations

1. **Event Store Optimization**
   - Event batching for bulk operations
   - Snapshot strategy for large aggregates
   - Indexing for efficient event retrieval
   - Caching for frequently accessed data

2. **Query Performance**
   - Optimized projections for specific query patterns
   - Pagination for large result sets
   - Filtering at the data source
   - Caching for read models

3. **UI Performance**
   - Viewport scaling for UI adaptability
   - Lazy loading of components
   - Efficient rendering strategies
   - Minimizing network requests
