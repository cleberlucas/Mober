using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mobile.Platforms.Android.Handlers;
using ZXing.Net.Maui;

namespace Mobile;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(h =>
            {
#if ANDROID || IOS || MACCATALYST
                h.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#endif
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}