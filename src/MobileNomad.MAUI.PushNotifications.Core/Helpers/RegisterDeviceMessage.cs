namespace MobileNomad.MAUI.PushNotifications.Helpers;

/// <summary>
/// Represents a message containing device information required for push notification registration.
/// </summary>
public class RegisterDeviceMessage
{
    /// <summary>
    /// Gets or sets the device token used for push notification registration.
    /// </summary>
    public string DeviceToken { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterDeviceMessage"/> class.
    /// </summary>
    public RegisterDeviceMessage()
    {
    }
}