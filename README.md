# FloppyShelf.Mediator
A **lightweight**, **dependency-free** mediator implementation designed for .NET Standard 2.0. It enables **CQRS (Command Query Responsibility Segregation)** patterns by decoupling request handling from business logic with minimal overhead.

---

## Features

- Fully compatible with **.NET Standard 2.0** and higher.
- **No external dependencies** - pure .NET.
- Simple setup and configuration.
- Supports **multiple assemblies** and **namespace filtering**.
- Extensible design.

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

var services = new ServiceCollection();
services.AddMediator(new[] { typeof(Program).Assembly }, null);
```

### 4. Send a Request

```csharp
var provider = services.BuildServiceProvider();
var sender = provider.GetRequiredService<ISender>();

var result = await sender.SendAsync(new GetProductQuery { Id = 1 });
Console.WriteLine(result.Name);
```

---

## API Overview

### Interfaces

- `IRequest<TResponse>`
- `IRequestHandler<TRequest, TResponse>`
- `ISender`

### Key Classes

- `Sender` - Central implementation of `ISender`.
- `ServiceCollectionExtensions` - Extension method `AddMediator()` for DI setup.

---

## Project Structure

```
FloppyShelf.Mediator/
├── Extensions/
│   └── ServiceCollectionExtensions.cs
├── Interfaces/
│   ├── IRequest.cs
│   ├── IRequestHandler.cs
│   └── ISender.cs
└── Sender.cs
```

---

## Why FloppyShelf.Mediator?

- **Clean and Minimal:** Only the essentials, no magic.
- **Fully Open Source:** MIT licensed.
- **Great for Learning:** Understand how mediators work internally.
- **Perfect for Microservices and Clean Architectures.**