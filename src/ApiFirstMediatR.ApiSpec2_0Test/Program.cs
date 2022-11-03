var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

app.MapControllers();

app.Run();