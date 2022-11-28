var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(AddPetCommandHandler));
var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }