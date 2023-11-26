using Contractor.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Contractor.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, DatabaseContext context)
        {
            string httpVerb = httpContext.Request.Method.ToUpper();
            if (httpVerb == "POST" || httpVerb == "PUT" || httpVerb == "DELETE")
            {
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync<object, object>(null!, operation: async (dbctx, state, cancel) =>
                {
                    IDbContextTransaction transaction = null!;
                    try
                    {
                        transaction = await context.Database.BeginTransactionAsync();
                        await _next(httpContext);
                        await transaction.CommitAsync();
                        return null!;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }

                }, null);
            }
            else
            {
                await _next(httpContext);
            }
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTransaction(this IApplicationBuilder app)
            => app.UseMiddleware<TransactionMiddleware>();
    }
}
