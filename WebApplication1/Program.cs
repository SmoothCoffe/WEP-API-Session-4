using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Extensions;
using WebApplication1.Middlewares;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Inversion of Control (IoC) container registration.
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            #region Automatic Migrations & Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var authenticationDbContext = services.GetRequiredService<AuthenticationDbContext>();
                    await authenticationDbContext.Database.MigrateAsync();

                    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                    await applicationDbContext.Database.MigrateAsync();

                    await LibraryContextSeeding.SeedAsync(applicationDbContext);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                }
            }
            #endregion

            #region Middleware Pipeline
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestIdHeaderMiddleware>();
            app.UseMiddleware<RequestTimingMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Web API v1");

                // Makes the "Authorize" dialog accept a raw token as well as "Bearer <token>".
                options.UseRequestInterceptor("""
                    function (request) {
                        const authorization = request.headers.Authorization || request.headers.authorization;

                        if (authorization && !/^Bearer\s/i.test(authorization)) {
                            request.headers.Authorization = `Bearer ${authorization}`;
                            delete request.headers.authorization;
                        }

                        return request;
                    }
                    """);
            });

            app.UseHttpsRedirection();

            app.UseCors(ApplicationServiceExtension.CorsPolicyName);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
