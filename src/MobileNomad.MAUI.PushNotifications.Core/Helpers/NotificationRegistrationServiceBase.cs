using Microsoft.Azure.NotificationHubs;
using MobileNomad.MAUI.PushNotifications.Interfaces;

namespace MobileNomad.MAUI.PushNotifications.Helpers;

/// <summary>
/// Provides a base implementation for notification registration services across platforms.
/// Handles device registration and unregistration with Azure Notification Hubs.
/// </summary>
public abstract class NotificationRegistrationServiceBase : INotificationRegistrationService
{
    /// <summary>
    /// Provides network connectivity information.
    /// </summary>
    protected readonly IConnectivity _connectivity;
    /// <summary>
    /// Client for interacting with Azure Notification Hubs.
    /// </summary>
    protected readonly INotificationHubClient _notificationHubClient;
    /// <summary>
    /// The unique identifier for the device.
    /// </summary>
    protected string? _deviceId;
    /// <summary>
    /// The push notification device token.
    /// </summary>
    protected string _deviceToken = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRegistrationServiceBase"/> class.
    /// </summary>
    /// <param name="connectivity">The connectivity service.</param>
    /// <param name="notificationHubClient">The notification hub client.</param>
    public NotificationRegistrationServiceBase(IConnectivity connectivity, INotificationHubClient notificationHubClient)
    {
        _connectivity = connectivity;
        _notificationHubClient = notificationHubClient;
    }

    /// <summary>
    /// Gets the Azure Notification Hub installation for the device.
    /// </summary>
    /// <param name="message">The registration message containing device information.</param>
    /// <returns>The installation object or null if not available.</returns>
    protected abstract Task<Installation?> GetHubInstallation(RegisterDeviceMessage message);

    /// <summary>
    /// Registers the device with Azure Notification Hubs.
    /// </summary>
    /// <param name="message">The registration message containing device information.</param>
    /// <returns>A <see cref="NotificationReturn"/> indicating the result of the registration.</returns>
    public async Task<NotificationReturn> RegisterDevice(RegisterDeviceMessage message)
    {
        var returnValue = new NotificationReturn();
        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                returnValue.Status = ReturnStatus.GeneralFailure;
            }

            var installation = await GetHubInstallation(message);
            if (installation == null)
            {
                returnValue.Status = ReturnStatus.GeneralFailure;
            }

            await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
            returnValue.Status = ReturnStatus.Success;

        }
        catch (Exception ex)
        {
            returnValue.Status = ReturnStatus.Exception;
            returnValue.Exception = ex;
        }
        return returnValue;
    }

    /// <summary>
    /// Unregisters the device from Azure Notification Hubs.
    /// </summary>
    /// <returns>A <see cref="NotificationReturn"/> indicating the result of the unregistration.</returns>
    public async Task<NotificationReturn> UnregisterDevice()
    {
        var returnValue = new NotificationReturn();
        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                returnValue.Status = ReturnStatus.GeneralFailure;
            }

            if (_deviceId == null)
            {
                returnValue.Status = ReturnStatus.GeneralFailure;
            }

            await _notificationHubClient.DeleteInstallationAsync(_deviceId).ConfigureAwait(false);
            returnValue.Status = ReturnStatus.Success;

        }
        catch (Exception ex)
        {
            returnValue.Status = ReturnStatus.Exception;
            returnValue.Exception = ex;
        }
        return returnValue;
    }
}