using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private Usuario _usuarioComReceita;
        private string _senhaUsarioComReceita;

        private Usuario _usuarioSemReceita;
        private string _senhaUsarioSemReceita;

        private Usuario _usuarioComConexao;
        private string _senhaUsarioComConexao;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descritor = services.SingleOrDefault(d => d.ServiceType == typeof(LivroDeReceitasContext));
                    if (descritor is not null)
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

                    (_usuarioComReceita, _senhaUsarioComReceita) = ContextSeedInMemory.Seed(database);
                    (_usuarioSemReceita, _senhaUsarioSemReceita) = ContextSeedInMemory.SeedUsuarioSemReceita(database);
                    (_usuarioComConexao, _senhaUsarioComConexao) = ContextSeedInMemory.SeedUsuarioComConexao(database);
                });
        }

        public Usuario RecuperarUsuario()
        {
            return _usuarioComReceita;
        }

        public string RecuperarSenha()
        {
            return _senhaUsarioComReceita;
        }

        public Usuario RecuperarUsuarioSemReceita()
        {
            return _usuarioSemReceita;
        }

        public string RecuperarSenhaUsuarioSemReceita()
        {
            return _senhaUsarioSemReceita;
        }

        public Usuario RecuperarUsuarioComConexao()
        {
            return _usuarioComConexao;
        }

        public string RecuperarSenhaUsuarioComConexao()
        {
            return _senhaUsarioComConexao;
        }
    }
}