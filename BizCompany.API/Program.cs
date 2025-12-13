using BizCompany.API.Context;
using BizCompany.API.DataAccess;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
    //In order to using Lazy Loading
    //option.UseLazyLoadingProxies();
});

//CORS Policy in order to allow any origin, method, and header. We will consume this API from 4200 port in Angular application.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//In order to ignore circular references
builder.Services.AddControllers().AddJsonOptions(config =>
    config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

//In order to use Fluent Validation.
//GetExecutingAssembly(): It will scan the current assembly for any validators.
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Service Registrations
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//BlogRepository Registration
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<ProductRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//In order to use CORS policy
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
