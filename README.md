# FloppyShelf.Mediator
A **lightweight** mediator implementation designed for .NET Standard 2.0 and higher. It simplifies request handling by decoupling it from application logic, enabling easier maintenance, better testability, and clean separation of concerns — while leveraging dependency injection through `Microsoft.Extensions.DependencyInjection.Abstractions`.

---

## Features
- Target frameworks: `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0`, `net472`, `net48`, and `net481`.
- **No external dependencies** (except `Microsoft.Extensions.DependencyInjection.Abstractions` for DI).
- Simple setup and configuration.
- Supports **multiple assemblies** and **namespace filtering**.
- Clean, extensible, and minimalistic design.

> **Note**: When targeting .NET Framework 4.7.2, 4.8, or 4.8.1 (net472, net48, net481), the following additional dependencies will be automatically included:
> - Microsoft.Bcl.AsyncInterfaces
> - System.Runtime.CompilerServices.Unsafe
> - System.Threading.Tasks.Extensions
> These packages are required to support modern `async/await` features on older .NET Framework versions.

---

## Installation

Install via NuGet Package Manager:

```bash
Install-Package FloppyShelf.Mediator
```

Or via .NET CLI:

```bash
dotnet add package FloppyShelf.Mediator
```

---

## Usage

### 1. Define a Request

```csharp
using FloppyShelf.Mediator.Interfaces;

public class GetProductQuery : IRequest<Product>
{
    public int Id { get; set; }
}
```

### 2. Implement a RequestHandler

```csharp
using FloppyShelf.Mediator.Interfaces;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    public Task<Product> HandleAsync(GetProductQuery request, CancellationToken cancellationToken)
    {
        // Simulated database access
        return Task.FromResult(new Product { Id = request.Id, Name = "Sample Product" });
    }
}
```

### 3. Register the Mediator

```csharp
using FloppyShelf.Mediator.Extensions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Register handlers from the specified assemblies
services.AddMediator(new[] { typeof(Program).Assembly });

// Optional: Pass a string[] to filter specific namespaces
```

### 4. Send a Request

```csharp
using Microsoft.Extensions.DependencyInjection;

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

var result = await mediator.SendAsync(new GetProductQuery { Id = 1 });
Console.WriteLine(result.Name);
```

---

## API Overview

### Interfaces

- `IRequest<TResponse>` - Represents a request expecting a response.
- `IRequestHandler<TRequest, TResponse>` - Handles a specific request.
- `IMediator` - Sends requests to handlers.

### Key Classes

- `Mediator` -  Core implementation of the mediator pattern.
- `ServiceCollectionExtensions` - Provides the `AddMediator()` extension method for easy DI setup.

---

## Why FloppyShelf.Mediator?

- **Clean and Minimal** - Only the essentials, no magic.
- **Fully Open Source** - MIT licensed.
- **Great for Learning** - Understand how mediators work internally.
- **Extremely Lightweight** - No unnecessary complexity.
