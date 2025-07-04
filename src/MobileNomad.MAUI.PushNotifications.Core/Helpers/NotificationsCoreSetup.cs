using Microsoft.Azure.NotificationHubs;

namespace MobileNomad.MAUI.PushNotifications.Helpers;

/// <summary>
/// Provides extension methods for setting up notification core services.
/// </summary>
public static class NotificationsCoreSetup
{
    /// <summary>
    /// Configures the notification core services for the MauiAppBuilder.
    /// </summary>
    /// <param name="builder">The MauiAppBuilder instance to configure.</param>
    /// <param name="notificationHubConnectionString">The connection string for the Azure Notification Hub.</param>
    /// <param name="hubName">The name of the Notification Hub.</param>
    /// <returns>The configured MauiAppBuilder instance.</returns>
    public static MauiAppBuilder SetupNotificationsCore(this MauiAppBuilder builder, string notificationHubConnectionString, string hubName, bool includeIConnectivity = true)
    {
        if (includeIConnectivity)
        {
            builder.Services.AddSingleton<IConnectivity>(serviceProvider =>
            {
                return Connectivity.Current;
            });
        }

        builder.Services.AddSingleton<INotificationHubClient, NotificationHubClient>((IServiceProvider provider) => {
            return NotificationHubClient.CreateClientFromConnectionString(notificationHubConnectionString, hubName, true);
        });

        return builder;
    }
}