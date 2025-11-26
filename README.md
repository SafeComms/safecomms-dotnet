# SafeComms .NET SDK

Official .NET client for the SafeComms API.

## Installation

```bash
dotnet add package SafeComms.Client
```

## Usage

```csharp
using SafeComms.Client;

var client = new SafeCommsClient("your-api-key");

// Moderate text
var result = await client.ModerateTextAsync(
    content: "Some text to check",
    language: "en",
    replace: true
);
Console.WriteLine(result);

// Get usage
var usage = await client.GetUsageAsync();
Console.WriteLine(usage);
```
