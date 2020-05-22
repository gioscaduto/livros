using App.Data;
using App.Utils;
using Business.Interfaces;
using Business.Models;
using Business.Notificacoes;
using Business.Services;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<LivrariaDbContext>();
            services.AddScoped<IEditoraRepository, EditoraRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IEditoraService, EditoraService>();
            services.AddScoped<ILivroService, LivroService>();
            services.AddSingleton<PaginaUtil>();

            return services;
        }
    }
}
