# Aspire.Hosting Resource Pattern

This document describes the standard file and code structure for resource packages in the Aspire.Hosting repository. Use this as a reference when creating new resource integrations.

## File Structure

Each resource package typically contains:

- `*Resource.cs` — The main resource class, representing the service/container.
- `*BuilderExtensions.cs` — Extension methods for adding/configuring the resource in the distributed application builder.
- `*ContainerImageTags.cs` — Centralized constants for container registry, image, and tag.
- (Optional) `*ServiceExtensions.cs` — For lifecycle hooks or additional service registration.
- (Optional) `*LifecycleHook.cs` — For custom distributed application lifecycle logic.
- `README.md` — Documentation for the resource package.

## Resource Class (`*Resource.cs`)
- Public class, usually named `{ResourceName}Resource`.
- Inherits from `ContainerResource` or another base resource.
- Declares endpoint names as `const string`.
- Exposes primary endpoint as a property (e.g., `PrimaryEndpoint`).
- May implement `IResourceWithConnectionString` if a connection string is relevant.
- Uses file-scoped namespaces and modern C# features.
- Example:

```csharp
public class SeqResource(string name) : ContainerResource(name), IResourceWithConnectionString
{
    internal const string PrimaryEndpointName = "http";
    private EndpointReference? _primaryEndpoint;
    public EndpointReference PrimaryEndpoint => _primaryEndpoint ??= new(this, PrimaryEndpointName);
    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create($"{PrimaryEndpoint.Property(EndpointProperty.Url)}");
}
```

## Builder Extensions (`*BuilderExtensions.cs`)
- Public static class, named `{ResourceName}BuilderExtensions`.
- Contains extension methods for `IDistributedApplicationBuilder` and/or `IResourceBuilder<TResource>`.
- Adds the resource, configures endpoints, image, registry, health checks, etc.
- Uses constants from `*ContainerImageTags.cs`.
- Example:

```csharp
public static class SeqBuilderExtensions
{
    public static IResourceBuilder<SeqResource> AddSeq(this IDistributedApplicationBuilder builder, string name, int? port = null)
    {
        var seqResource = new SeqResource(name);
        var resourceBuilder = builder.AddResource(seqResource)
            .WithHttpEndpoint(port: port, targetPort: 80, name: SeqResource.PrimaryEndpointName)
            .WithImage(SeqContainerImageTags.Image, SeqContainerImageTags.Tag)
            .WithImageRegistry(SeqContainerImageTags.Registry)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithHttpHealthCheck("/health");
        return resourceBuilder;
    }
}
```

## Container Image Tags (`*ContainerImageTags.cs`)
- Public static class, named `{ResourceName}ContainerImageTags`.
- Contains `public const string` fields for `Registry`, `Image`, and `Tag`.
- Example:

```csharp
public static class SeqContainerImageTags
{
    public const string Registry = "docker.io";
    public const string Image = "datalust/seq";
    public const string Tag = "2025.1";
}
```

## Optional: Service Extensions and Lifecycle Hooks
- For resources needing custom lifecycle logic, add `*ServiceExtensions.cs` and `*LifecycleHook.cs`.
- Register hooks via `builder.Services.TryAddLifecycleHook<T>()` in service extensions.

## General Coding Style
- Use file-scoped namespaces.
- Use XML doc comments for all public APIs.
- Prefer C# 13 features and modern idioms.
- Follow `.editorconfig` for formatting.

---

This pattern ensures consistency and discoverability for all Aspire.Hosting resource integrations.
