---
name: create-class-mcp
description: 'Use when creating or modifying MCP tool classes or methods in MyMcpServer.'
---

# Create MCP tool classes

When adding an MCP tool class or method to `src/MyMcpServer`:

1. Mark the class with `[McpServerToolType]`.
2. Mark every tool method with `[McpServerTool]`.
3. Add a `[Description("...")]` to each tool method so its purpose is clear to MCP clients.
4. Follow `EchoTools` for the existing class and method style.
5. Keep `.WithToolsFromAssembly()` in `Program.cs` so the server discovers the annotated tools.

```csharp
[McpServerToolType]
public static class ExampleTools
{
    [McpServerTool, Description("Describes what this tool does.")]
    public static string Example() => "result";
}
```
