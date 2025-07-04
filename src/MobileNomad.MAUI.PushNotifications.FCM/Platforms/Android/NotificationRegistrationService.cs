using Android.Gms.Common;
using Android.Gms.Extensions;
using Android.Provider;
using Firebase.Messaging;
using Microsoft.Azure.NotificationHubs;
using MobileNomad.MAUI.PushNotifications.Helpers;

namespace MobileNomad.MAUI.PushNotifications.FCM.Platforms.Android;
public class NotificationRegistrationService : NotificationRegistrationServiceBase
{
    private bool _playEnabled;

    public NotificationRegistrationService(IConnectivity connectivity, INotificationHubClient notificationHubClient) : base(connectivity, notificationHubClient)
    {
        _playEnabled = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Platform.AppContext) == ConnectionResult.Success;
        _deviceId = Settings.Secure.GetString(Platform.AppContext.ContentResolver, Settings.Secure.AndroidId);
    }

    protected override async Task<Installation?> GetHubInstallation(RegisterDeviceMessage message)
    {
        if (!_playEnabled || _deviceId == null)
            return null;

        var firebaseToken = await FirebaseMessaging.Instance.GetToken();
        return new Installation
        {
            InstallationId = _deviceId,
            Platform = NotificationPlatform.FcmV1,
            PushChannel = firebaseToken.ToString()
        };
    }
}
