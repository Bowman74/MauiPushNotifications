
namespace MobileNomad.MAUI.PushNotifications;

public static class NotificationWNSSetup
{
    public static MauiAppBuilder SetupNotificationsWNS(this MauiAppBuilder builder)
    {
#if WINDOWS
        builder.Services.AddScoped<Interfaces.INotificationRegistrationService, WNS.Platforms.Windows.NotificationRegistrationService>();
#endif
        return builder;
    }
}
