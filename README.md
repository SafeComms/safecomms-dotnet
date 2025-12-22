# SafeComms .NET SDK

Official .NET client for the SafeComms API.

SafeComms is a powerful content moderation platform designed to keep your digital communities safe. It provides real-time analysis of text to detect and filter harmful content, including hate speech, harassment, and spam.

**Get Started for Free:**
We offer a generous **Free Tier** for all users, with **no credit card required**. Sign up today and start protecting your community immediately.

## Documentation

For full API documentation and integration guides, visit [https://safecomms.dev/docs](https://safecomms.dev/docs).

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
