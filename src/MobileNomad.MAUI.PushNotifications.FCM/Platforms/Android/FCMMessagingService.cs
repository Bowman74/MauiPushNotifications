using AndroidApp = Android.App;
using AndroidRes = Android.Resource;
using Android.App;
using Firebase.Messaging;

namespace MobileNomad.MAUI.PushNotifications.FCM;

[Service(Exported = false)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class FCMMessagingService : FirebaseMessagingService
{
    int messageId = 0;
    
    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);
        bool performDefaultNotification;


        performDefaultNotification = PushMessages.RaiseMessageReceived(NotificaitonType.AndroidFirebase, message);
        if (performDefaultNotification)
        {
            ShowNotification(message);
        }
    }

    private void ShowNotification(RemoteMessage message)
    {
        var notification = message.GetNotification();

        if (AndroidApp.Application.Context != null && notification != null &&
            OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            var manager = (NotificationManager?)AndroidApp.Application.Context.GetSystemService(NotificationService);
            if (manager != null)
            {
                var channel = new AndroidApp.NotificationChannel(notification.ChannelId ?? "QRTracker", "QRTracker", NotificationImportance.Max);
                manager.CreateNotificationChannel(channel);

                var displayNotification = new AndroidApp.Notification.Builder(AndroidApp.Application.Context, channel.Id)
                                            .SetContentTitle(notification.Title)
                                            .SetContentText(notification.Body)
                                            .SetSmallIcon(AndroidRes.Mipmap.SymDefAppIcon)
                                            .Build();

                manager.Notify(messageId++, displayNotification);
            }
        }
    }
}