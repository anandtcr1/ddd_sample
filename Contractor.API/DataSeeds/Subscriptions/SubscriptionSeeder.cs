using Contractor.EntityFrameworkCore;

namespace Contractor.Subscriptions
{
    public class SubscriptionSeeder
    {
        internal static void SeedData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var subscriptions = SeedSubscriptions(dbcontext);
            }
        }

        private static List<Subscription> SeedSubscriptions(DatabaseContext context)
        {
            if(!context.Subscriptions.Any())
            {
                var subscription = Subscription.Create(SubscriptionConstants.DefaultSubscriptionName, SubscriptionConstants.DefaultSubscriptionStorageSpace);

                foreach (var folderName in SubscriptionConstants.DefaultSubscriptionFoldersList)
                {
                    subscription.AddProjectFolderTemplate(folderName);
                }

                context.Subscriptions.Add(subscription);

                context.SaveChanges();
            }

            return context.Subscriptions.ToList();
        }
    }
}
