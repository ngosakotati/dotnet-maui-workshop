using Microsoft.Extensions.Logging;
using MonkeyFinder.View;
using MonkeyFinder.Services;

namespace MonkeyFinder;

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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
		builder.Services.AddSingleton<IMap>(Map.Default);

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MonkeysViewModel>();

		builder.Services.AddTransient<MonkeyDetailsViewModel>();
		builder.Services.AddTransient<DetailsPage>();

		return builder.Build();
	}
}
