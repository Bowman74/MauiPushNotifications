using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileNomad.MAUI.PushNotifications;
public static class NotificationsAPNSSetup
{
    public static MauiAppBuilder SetupNotificationsAPNS(this MauiAppBuilder builder)
    {
#if IOS
        builder.Services.AddScoped<Interfaces.INotificationRegistrationService, MobileNomad.MAUI.PushNotifications.APNS.NotificationRegistrationService>();
#elif MACCATALYST
        builder.Services.AddScoped<Interfaces.INotificationRegistrationService, MobileNomad.MAUI.PushNotifications.APNS.NotificationRegistrationService>();
#endif
        return builder;
    }
}
