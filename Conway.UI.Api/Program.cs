using Conway.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GameGrid>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services for controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure routing for controllers
app.UseRouting();
app.MapControllers();

await app.RunAsync();
