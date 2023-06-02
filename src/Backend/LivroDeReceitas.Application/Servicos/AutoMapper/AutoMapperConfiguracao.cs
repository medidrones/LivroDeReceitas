﻿using AutoMapper;
using HashidsNet;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;
using LivroDeReceitas.Domain.Entidades;

namespace LivroDeReceitas.Application.Servicos.AutoMapper;

public class AutoMapperConfiguracao : Profile
{
    private readonly IHashids _hashids;

    public AutoMapperConfiguracao(IHashids hashids)
    {
        _hashids = hashids;

        RequisicaoParaEntidade();
        EntidadeParaResposta();
    }

    private void RequisicaoParaEntidade()
    {
        CreateMap<RequisicaoRegistrarUsuarioJson, Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());

        CreateMap<RequisicaoRegistrarReceitaJson, Receita>();
        CreateMap<RequisicaoRegistrarIngredienteJson, Ingrediente>();
    }

    private void EntidadeParaResposta()
    {
        CreateMap<Receita, RespostaReceitaJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));
        CreateMap<Ingrediente, RespostaIngredienteJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));
    }
}