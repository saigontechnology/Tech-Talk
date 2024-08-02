# .NET Aspire

## What is .NET Aspire?

.NET Aspire is an opinionated, cloud ready stack for building observable, production ready, distributed applications.​ .NET Aspire is delivered through a collection of NuGet packages that handle specific cloud-native concerns. Cloud-native apps often consist of small, interconnected pieces or microservices rather than a single, monolithic code base. Cloud-native apps generally consume a large number of services, such as databases, messaging, and caching.

Ref: [https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)

## Why .NET Aspire?

.NET Aspire is designed to improve the experience of building .NET cloud-native apps. It provides a consistent, opinionated set of tools and patterns that help you build and run distributed apps. .NET Aspire is designed to help you with:

- Orchestration: .NET Aspire provides features for running and connecting multi-project applications and their dependencies.
- Components: .NET Aspire components are NuGet packages for commonly used services, such as Redis or Postgres, with standardized interfaces ensuring they connect consistently and seamlessly with your app.
- Tooling: .NET Aspire comes with project templates and tooling experiences for Visual Studio and the dotnet CLI help you create and interact with .NET Aspire apps.

## Overview

Ref: [https://learn.microsoft.com/en-us/dotnet/aspire/dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/dashboard)

### Components

### Dashboard

### Service discovery

### Service defaults

### Health checks

### Telemetry


## Demo

See demo at Demo in source

## Deployment

.NET Aspire applications are built with cloud-agnostic principles, allowing deployment flexibility across various platforms supporting .NET and containers. Users can adapt the provided guidelines for deployment on other cloud environments or local hosting. The manual deployment process, while feasible, involves exhaustive steps prone to errors. We anticipate users will prefer leveraging CI/CD pipelines and cloud-specific tooling for a more streamlined deployment experience tailored to their chosen infrastructure.
