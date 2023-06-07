﻿using AutoMapper;
using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;
using LivroDeReceitas.Domain.Enum;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositorios.Receita;

namespace LivroDeReceitas.Application.UseCases.Dashboard;

public class DashboardUseCase : IDashboardUseCase
{
    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public DashboardUseCase(IReceitaReadOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IMapper mapper)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
    }

    public async Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson requisicao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var receitas = await _repositorio.RecuperarTodasDoUsuario(usuarioLogado.Id);

        receitas = Filtrar(requisicao, receitas);

        return new RespostaDashboardJson()
        {
            Receitas = _mapper.Map<List<RespostaReceitaDashboardJson>>(receitas)
        };
    }

    private static IList<Domain.Entidades.Receita> Filtrar(RequisicaoDashboardJson requisicao, IList<Domain.Entidades.Receita> receitas)
    {
        if (receitas is null)
            return new List<Domain.Entidades.Receita>();

        var receitasFiltradas = receitas;

        if (requisicao.Categoria.HasValue)
        {
            receitasFiltradas = receitas.Where(r => r.Categoria == (Domain.Enum.Categoria)requisicao.Categoria.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(requisicao.TituloOuIngrediente))
        {
            receitasFiltradas = receitas.Where(r => r.Titulo.CompararSemConsiderarAcentoUpperCase(requisicao.TituloOuIngrediente) || r.Ingredientes.Any(ingrediente => ingrediente.Produto.CompararSemConsiderarAcentoUpperCase(requisicao.TituloOuIngrediente))).ToList();
        }

        return receitasFiltradas.OrderBy(c => c.Titulo).ToList();
    }
}