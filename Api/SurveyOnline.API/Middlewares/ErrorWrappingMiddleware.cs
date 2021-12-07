using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SurveyOnline.API.Helpers;
using System;
using System.Threading.Tasks;

namespace SurveyOnline.API.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode = 500;
            }

            if (!context.Response.HasStarted && context.Response.StatusCode != 204)
            {
                context.Response.ContentType = "application/json";

                var response = new ApiResponse(context.Response.StatusCode);

                var json = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
