using AutoMapper;
using LivroDeReceitas.Application.Servicos.AutoMapper;

namespace Utilitario.Test.Mapper;

public class MapperBuilder
{
    public static IMapper Instancia()
    {
        var configuracao = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperConfiguracao>();
        });

        return configuracao.CreateMapper();
    }
}