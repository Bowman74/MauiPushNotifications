using MobileNomad.MAUI.PushNotifications.Helpers;

namespace MobileNomad.MAUI.PushNotifications.Interfaces;

/// <summary>
/// Defines methods for registering and unregistering devices with a notification service.
/// </summary>
public interface INotificationRegistrationService
{
    /// <summary>
    /// Registers the device with the notification service using the provided registration message.
    /// </summary>
    /// <param name="message">The registration message containing device information.</param>
    /// <returns>A <see cref="NotificationReturn"/> indicating the result of the registration.</returns>
    Task<NotificationReturn> RegisterDevice(RegisterDeviceMessage message);

    /// <summary>
    /// Unregisters the device from the notification service.
    /// </summary>
    /// <returns>A <see cref="NotificationReturn"/> indicating the result of the unregistration.</returns>
    Task<NotificationReturn> UnregisterDevice();
}

/// <summary>
/// Represents the result of a notification registration or unregistration operation.
/// </summary>
public struct NotificationReturn
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationReturn"/> struct.
    /// </summary>
    public NotificationReturn() { }

    /// <summary>
    /// Gets or sets the status of the operation.
    /// </summary>
    public ReturnStatus Status = ReturnStatus.GeneralFailure;

    /// <summary>
    /// Gets or sets the exception that occurred during the operation, if any.
    /// </summary>
    public Exception? Exception;
}

/// <summary>
/// Specifies the status of a notification registration or unregistration operation.
/// </summary>
public enum ReturnStatus
{
    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    Success = 0,
    /// <summary>
    /// The operation failed due to a general non exception error.
    /// </summary>
    GeneralFailure = 1,
    /// <summary>
    /// The operation failed due to an exception.
    /// </summary>
    Exception = 2
}