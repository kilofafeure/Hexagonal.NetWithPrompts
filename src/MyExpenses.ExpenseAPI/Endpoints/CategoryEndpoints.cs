using Microsoft.AspNetCore.Http.HttpResults;
using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;
using MyExpenses.Domain.Entities;
using MyExpenses.ExpenseAPI.Contracts;

namespace MyExpenses.ExpenseAPI.Endpoints;

internal static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapPost(
                "",
                async Task<Created<CategoryResponse>> (
                    CreateCategoryRequest request,
                    ICreateCategoryUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    var category = await useCase.CreateCategoryAsync(request.Name, cancellationToken);

                    return TypedResults.Created(
                        "/api/categories",
                        ToResponse(category));
                })
            .WithName("CreateCategory")
            .WithSummary("Create a category")
            .WithDescription("Creates a category and returns its details.")
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet(
                "",
                async Task<Ok<IReadOnlyList<CategoryResponse>>> (
                    IGetCategoriesUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    var categories = await useCase.GetCategoriesAsync(cancellationToken);
                    IReadOnlyList<CategoryResponse> response = categories
                        .Select(ToResponse)
                        .ToArray();

                    return TypedResults.Ok(response);
                })
            .WithName("GetCategories")
            .WithSummary("Get categories")
            .WithDescription("Returns all categories.");

        group.MapDelete(
                "/{categoryId:guid}",
                async Task<NoContent> (
                    Guid categoryId,
                    IDeleteCategoryUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    await useCase.DeleteCategoryAsync(categoryId, cancellationToken);
                    return TypedResults.NoContent();
                })
            .WithName("DeleteCategory")
            .WithSummary("Delete a category")
            .WithDescription("Permanently deletes a category.")
            .ProducesProblem(StatusCodes.Status404NotFound);

        return group;
    }

    private static CategoryResponse ToResponse(Category category) =>
        new(category.Id, category.Name);
}
