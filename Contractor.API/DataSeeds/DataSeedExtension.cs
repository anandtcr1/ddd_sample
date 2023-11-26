using Contractor.EntityFrameworkCore;
using Contractor.Identities;
using Contractor.Subscriptions;

namespace Contractor.DataSeeds
{
    public static class DataSeed
    {
        public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));
            try
            {
                IdentitySeeder.SeedData(app);
                SubscriptionSeeder.SeedData(app);

            }
            catch (Exception ex)
            {
                logger.LogCritical(nameof(DataSeed), ex.Message);
            }
            return app;
        }
    }
}
