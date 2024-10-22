using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
});
var app = builder.Build();

app.MapControllers();

app.Run();

namespace ApiFirstMediatR.ApiSpec2_0Test
{
    public partial class Program { }
}
