var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(AddPetCommandHandler));
var app = builder.Build();

app.MapControllers();

app.Run();

namespace ApiFirstMediatR.ApiSpec3_0Test
{
    public partial class Program { }
}