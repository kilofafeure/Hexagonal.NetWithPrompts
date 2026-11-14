namespace MyExpenses.ExpenseAPI.Endpoints;

internal static class StatusEndpoints
{
    public static void MapStatusEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", () => "API Corriendo");

        endpoints.MapGet("/status", () => TypedResults.Text("El servidor está activo"))
            .WithName("GetServerStatus")
            .WithSummary("Check whether the server is active")
            .WithDescription("Returns a plain-text message when the API is running.");
    }
}
