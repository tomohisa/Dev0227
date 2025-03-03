# School Management System

A comprehensive web application for managing students, teachers, and classes built with ASP.NET Core, Blazor, Next.js, and Sekiban Event Sourcing framework.

[日本語のREADME](./README_JP.md)

## Overview

The School Management System provides a centralized platform for tracking and managing educational institutions. It allows administrators to manage students, teachers, and classes, and track the relationships between these entities.

## Features

- **Student Management**: Register, update, and delete student records
- **Teacher Management**: Register, update, and delete teacher records
- **Class Management**: Create, update, and delete class records
- **Relationship Management**: Assign students to classes, assign teachers to classes
- **Duplicate Checking**: Prevent duplicate student and teacher IDs
- **Multiple Frontends**: Both Blazor and Next.js frontends available

## Technology Stack

- **Backend**: ASP.NET Core, Sekiban Event Sourcing, Orleans
- **Frontend**: Blazor, Next.js with shadcn/ui components
- **Architecture**: Event Sourcing, CQRS, Domain-Driven Design
- **Testing**: xUnit for domain tests, Playwright for end-to-end tests

## Project Structure

```
SchoolManagement/
├── SchoolManagement.Domain/           # Domain models, events, commands
│   └── Aggregates/                    # Domain aggregates
│       ├── Classes/                   # Class aggregate
│       ├── Students/                  # Student aggregate
│       ├── Teachers/                  # Teacher aggregate
│       └── WeatherForecasts/          # WeatherForecast aggregate
├── SchoolManagement.ApiService/       # API endpoints
├── SchoolManagement.Web/              # Blazor web frontend
├── school-management-next/            # Next.js frontend
├── SchoolManagement.AppHost/          # Aspire host for services
├── SchoolManagement.ServiceDefaults/  # Common service configurations
├── SchoolManagement.Playwright/       # End-to-end tests
└── SchoolManagement.Domain.Tests/     # Domain unit tests
```

## Running the Application

To run the application with the Aspire host:

```bash
cd SchoolManagement/SchoolManagement.AppHost
dotnet run --launch-profile https
```

Access the web frontends at:
- Blazor frontend: https://localhost:7201
- API service: https://localhost:7202

To run the Next.js frontend:

```bash
cd SchoolManagement/school-management-next
npm install
npm run dev
```

## LLM Development with Cline

This repository is designed to work with Cline, an AI assistant with a memory bank system. The memory bank contains:

- **projectbrief.md**: Core requirements and goals
- **productContext.md**: Why this project exists and problems it solves
- **systemPatterns.md**: System architecture and design patterns
- **techContext.md**: Technologies used and technical constraints
- **activeContext.md**: Current work focus and recent changes
- **progress.md**: What works and what's left to build

When working with Cline, use the command **update memory bank** to have Cline review and update all memory bank files.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.
