using AutoMapper;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Domain.Entidades;

namespace LivroDeReceitas.Application.Servicos.AutoMapper;

public class AutoMapperConfiguracao : Profile
{
    public AutoMapperConfiguracao()
    {
        CreateMap<RequisicaoRegistrarUsuarioJson, Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());
    }
}