using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Domain.Common;
using FluentValidation;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using static Ecommerce.Application.Common.Exceptions.NotFoundException;

namespace Ecommerce.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //1.Determinar el codigo del estado HTTP
            var code = exception switch
            {
                ValidationException => HttpStatusCode.BadRequest, // 400
                NotFoundException => HttpStatusCode.NotFound,
                InvalidOperationException => HttpStatusCode.BadRequest,
                InsufficientStockException => HttpStatusCode.BadRequest,
                DbUpdateConcurrencyException => HttpStatusCode.Conflict,
                // 404
                _ => HttpStatusCode.InternalServerError // 500
            };
            //2. Extraer mensajes de error
            var errors = exception switch
            {
                ValidationException ve => ve.Errors.Select(e => e.ErrorMessage),
                DbUpdateConcurrencyException => new[] { "El registro fue modificado por otro usuario o proceso. Por favor refresque los datos e intente de nuevo." },
                _ => new[] { exception.Message }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            //3.Serializar la respuesta
            var result = JsonSerializer.Serialize(new 
            { 
                status = (int)code,
                //Si es 500, ocultamos el mensaje tecnico
                message = code == HttpStatusCode.InternalServerError 
                    ? "Ocurrió un error inesperado en el servidor." 
                    : exception.Message,
                errors 
            });
            return context.Response.WriteAsync(result);
        }
    }
}
