using Microsoft.Azure.NotificationHubs;
using MobileNomad.MAUI.PushNotifications.Helpers;
using Windows.Networking.PushNotifications;
using Windows.System.Profile;
using Win = Windows;

namespace MobileNomad.MAUI.PushNotifications.WNS.Platforms.Windows;

public class NotificationRegistrationService : NotificationRegistrationServiceBase
{
    public NotificationRegistrationService(IConnectivity connectivity, INotificationHubClient notificationHubClient) : base(connectivity, notificationHubClient)
    {
        _deviceId = GetDeviceId();
    }

    protected override async Task<Installation?> GetHubInstallation(RegisterDeviceMessage message)
    {

        var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();


        var deviceToken = channel.Uri;
        return new Installation
        {
            InstallationId = _deviceId ?? Guid.NewGuid().ToString(),
            Platform = NotificationPlatform.Wns,
            PushChannel = deviceToken
        };
    }

    public string GetDeviceId()
    {
        var systemId = SystemIdentification.GetSystemIdForPublisher();

        if (systemId == null || systemId.Id == null)
        {
            throw new InvalidOperationException("Unable to retrieve device ID.");
        }

        var dataReader = Win.Storage.Streams.DataReader.FromBuffer(systemId.Id);
        byte[] bytes = new byte[systemId.Id.Length];
        dataReader.ReadBytes(bytes);
        
        return BitConverter.ToString(bytes).Replace("-", "");
    }
}

