using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebSample.Models.Dto;

namespace WebSample.Middlewares
{
    /// <summary>
    /// Middleware for catching exceptions occurred during request
    /// </summary>
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
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
                var error = new ErrorDto
                {
                    Errors = new List<string> { ex.Message }
                };

                var errorResponse = JsonSerializer.Serialize(error);

                await context.Response.WriteAsync(errorResponse);
            }
        }
    }
}
