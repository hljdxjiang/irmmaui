using CommunityToolkit.Maui;
using IRMMAUI.Service;
using IRMMAUI.Service.impl;
using Microsoft.Extensions.Logging;
using OxyPlot.Maui.Skia;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace IRMMAUI;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        ConfigureServices(builder.Services);
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .UseOxyPlotSkia()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // 注册你的服务
        services.AddScoped<IFileProcess, FileProcess>();
        services.AddScoped<IOxyPlotService, OxyPlotService>();
        services.AddScoped<ILineProcessService, LineProcessService>();
        Services = services.BuildServiceProvider();
    }
}

