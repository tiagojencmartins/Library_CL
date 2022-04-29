using Library.Infrastructure;
using System.Reflection;
using MediatR;
using Library.Infrastructure.Crosscutting;
using Library.Application.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDependencies();
builder.Services.AddAuthorizationDependencies(builder.Configuration);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
