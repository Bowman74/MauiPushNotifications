using Microsoft.Azure.NotificationHubs;
using MobileNomad.MAUI.PushNotifications.Helpers;
using UIKit;

namespace MobileNomad.MAUI.PushNotifications.APNS;

/// <summary>
/// Provides an implementation of <see cref="NotificationRegistrationServiceBase"/> for registering and unregistering iOS devices with Azure Notification Hubs using APNS.
/// </summary>
public class NotificationRegistrationService : NotificationRegistrationServiceBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRegistrationService"/> class for iOS.
    /// </summary>
    /// <param name="connectivity">The connectivity service.</param>
    /// <param name="notificationHubClient">The notification hub client.</param>
    public NotificationRegistrationService(IConnectivity connectivity, INotificationHubClient notificationHubClient) : base(connectivity, notificationHubClient)
    {
        // Set the device ID using the iOS device's unique identifier.
        _deviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
    }

    /// <summary>
    /// Creates an <see cref="Installation"/> object for the current iOS device using the provided registration message.
    /// </summary>
    /// <param name="message">The registration message containing the device token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Installation"/> object or null if not available.</returns>
    protected override Task<Installation?> GetHubInstallation(RegisterDeviceMessage message)
    {
        var tcs = new TaskCompletionSource<Installation?>();

        _deviceToken = message.DeviceToken;

        var installation = new Installation
        {
            InstallationId = _deviceId,
            Platform = NotificationPlatform.Apns,
            PushChannel = _deviceToken
        };
        tcs.SetResult(installation);

        return tcs.Task;
    }
}