using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.Application.Handlers;
using Movies.Core.Repositories;
using Movies.Core.Repositories.Base;
using Movies.Infrastructure.Data;
using Movies.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
c.SwaggerDoc("v2", new OpenApiInfo { Title = "Movie Review API", Version = "v1"}));

builder.Services.AddDbContext<MovieContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieConnection"));
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(CreateMovieHandler).Assembly);
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(IRepository<>));
builder.Services.AddTransient<IMovieRepository, MovieRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Review API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
