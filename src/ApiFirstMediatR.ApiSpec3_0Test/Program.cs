var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
});
var app = builder.Build();

app.MapControllers();

app.Run();

namespace ApiFirstMediatR.ApiSpec3_0Test
{
    public partial class Program { }
}