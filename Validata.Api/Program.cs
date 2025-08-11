using FluentValidation;
using Validata.Application.Common.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Validata.Domain.Abstractions;
using Validata.Domain.Repositories;
using Validata.Infrastructure;
using Validata.Infrastructure.Persistence;
using Validata.Infrastructure.Repositories;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.Load("Validata.Application")));


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xml = Path.Combine(AppContext.BaseDirectory, "Validata.Api.xml");
    if (File.Exists(xml)) c.IncludeXmlComments(xml, true);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
