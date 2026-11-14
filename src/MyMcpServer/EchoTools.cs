using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MyMcpServer;

[McpServerToolType]
public static class EchoTools
{
    [McpServerTool, Description("Devuelve cinco nombres de colores en español.")]
    public static IEnumerable<string> GetSpanishColors() =>
        ["rojo", "azul", "verde", "amarillo", "naranja"];
}
