using AutoMapper;
using LivroDeReceitas.Application.Servicos.AutoMapper;
using Utilitario.Test.Hashids;

namespace Utilitario.Test.Mapper;

public class MapperBuilder
{
    public static IMapper Instancia()
    {
        var hashids = HashidsBuilder.Instance().Build();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfiguracao(hashids));
        });
        return mockMapper.CreateMapper();
    }
}