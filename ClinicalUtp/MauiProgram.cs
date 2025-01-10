using ClinicalUtp.Controllers;
using ClinicalUtp.viewModels;
using ClinicalUtp.views;
using Microsoft.Extensions.Logging;

namespace ClinicalUtp
{
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
                });


            builder.Services.AddSingleton<AppointmentsServices>();
            builder.Services.AddTransient<AppointmentViews>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<LoginServices>();
            builder.Services.AddSingleton<PacienteServices>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddSingleton<MedicinesServices>();
           
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
