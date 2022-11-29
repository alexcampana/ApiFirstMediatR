namespace ApiFirstMediatR.ApiSpec3_0Test.Handlers.Pets;

public class GetPetCategoriesQueryHandler : IRequestHandler<GetPetCategoriesQuery, IEnumerable<Category>>
{
    public Task<IEnumerable<Category>> Handle(GetPetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(TestData.Categories.AsEnumerable());
    }
}