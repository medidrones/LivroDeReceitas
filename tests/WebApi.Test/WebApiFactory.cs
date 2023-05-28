﻿using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private Usuario _usuario;
        private string _senha;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descritor = services.SingleOrDefault(d => d.ServiceType == typeof(LivroDeReceitasContext));
                    if (descritor != null)
                        services.Remove(descritor);

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<LivroDeReceitasContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    var serviceProvider = services.BuildServiceProvider();

                    using var scope = serviceProvider.CreateScope();
                    var scopeService = scope.ServiceProvider;

                    var database = scopeService.GetRequiredService<LivroDeReceitasContext>();

                    database.Database.EnsureDeleted();

                    (_usuario, _senha) = ContexSeedInMemory.Seed(database);
                });
        }

        public Usuario RecuperarUsuario()
        {
            return _usuario;
        }

        public string RecuperarSenha()
        {
            return _senha;
        }
    }
}