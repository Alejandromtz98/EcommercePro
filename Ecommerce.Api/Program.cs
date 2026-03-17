using Ecommerce.Api.Middleware;
using Ecommerce.Application;
using Ecommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Servicios de presentacion
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
        options.SuppressModelStateInvalidFilter = true); // Deshabilitar el filtro de validación automática

builder.Services.AddSwaggerGen();

//Capas de arquitectuta
builder.Services.AddInfrastructure(builder.Configuration); //SQL y DbContext
builder.Services.AddApplication(); //MediatR, FluentValidation y comportamientos

var app = builder.Build();

//Middleware personalizado para manejo de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
