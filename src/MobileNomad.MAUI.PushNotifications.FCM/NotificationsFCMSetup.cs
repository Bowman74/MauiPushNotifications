namespace MobileNomad.MAUI.PushNotifications;

public static class NotificationsFCMSetup
{
    public static MauiAppBuilder SetupNotificationsFCM(this MauiAppBuilder builder)
    {
#if ANDROID
        builder.Services.AddScoped<Interfaces.INotificationRegistrationService, FCM.Platforms.Android.NotificationRegistrationService>();
#endif
        return builder;
    }
}
