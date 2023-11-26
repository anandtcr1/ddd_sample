using Contractor.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contractor.DataSeeds
{
    public static class MigrationExtension
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app, ILogger logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical(nameof(MigrationExtension), ex.Message);
                    }
                }
            }
            return app;
        }
    }
}
