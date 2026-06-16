# ExceptionHandlingStrategies

[![CI](https://github.com/PFalkowski/ErrorHandling/actions/workflows/ci.yml/badge.svg)](https://github.com/PFalkowski/ErrorHandling/actions/workflows/ci.yml)
[![NuGet version](https://img.shields.io/nuget/v/ExceptionHandlingStrategies.svg)](https://www.nuget.org/packages/ExceptionHandlingStrategies/)
[![NuGet downloads](https://img.shields.io/nuget/dt/ExceptionHandlingStrategies.svg)](https://www.nuget.org/packages/ExceptionHandlingStrategies/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_ErrorHandling&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_ErrorHandling)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_ErrorHandling&metric=coverage)](https://sonarcloud.io/summary/new_code?id=PFalkowski_ErrorHandling)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://choosealicense.com/licenses/mit/)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-support-yellow.svg)](https://www.buymeacoffee.com/piotrfalkowski)

Exception handling policy infrastructure using the Strategy pattern. Decouples exception-handling behaviour from business logic — wire in a policy once; change it without touching callers.

## Install

```bash
dotnet add package ExceptionHandlingStrategies
```

## Policies

| Policy | Behaviour |
|--------|-----------|
| `LogExceptionPolicy` | Logs the exception via `ILogger` |
| `CollectAndIgnoreExceptionPolicy` | Queues exceptions in a bounded `ConcurrentQueue`, swallows them |
| `IgnoreExceptionPolicy` | Silently swallows all exceptions |
| `RethrowExceptionPolicy` | Re-throws (useful as a pass-through in aggregate) |
| `AggregatedExceptionHandlingPolicy` | Fans out to multiple policies; collects inner exceptions into `AggregateException` |
| `ExceptionHandlingFilter` | Wraps a policy; blocks specified exception types and rethrows as `ExceptionCannotBeHandledException` |

## Usage

```csharp
ILogger logger = ...; // any LoggerLite ILogger

// single policy
var policy = new LogExceptionPolicy(logger);
try { ... }
catch (Exception ex) { policy.HandleException(ex); }

// aggregate
var aggregate = new AggregatedExceptionHandlingPolicy(
    new LogExceptionPolicy(logger),
    new CollectAndIgnoreExceptionPolicy());

// filter critical exceptions out
var filtered = new ExceptionHandlingFilter(aggregate);
filtered.ExcludedExceptios.Add(typeof(OutOfMemoryException));
```

## Extensions

```csharp
var collected = new CollectAndIgnoreExceptionPolicy();
// ... run some code ...
string summary = collected.Exceptions.Summary(); // "ArgumentException: 3 times\n..."
```
