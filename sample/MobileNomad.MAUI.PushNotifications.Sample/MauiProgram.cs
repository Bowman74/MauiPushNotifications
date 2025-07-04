using Microsoft.Extensions.Logging;
using MobileNomad.MAUI.PushNotifications.Helpers;

namespace MobileNomad.MAUI.PushNotifications.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.SetupNotificationsCore(Secrets.AzureConnnectionString, Secrets.AzureHubName)
			.SetupNotificationsFCM()
			.SetupNotificationsAPNS();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}
