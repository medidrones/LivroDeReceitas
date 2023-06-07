﻿using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Domain.Repositorios.Receita;
using Moq;

namespace Utilitario.Test.Repositorios;

public class ReceitaReadOnlyRepositorioBuilder
{
    private static ReceitaReadOnlyRepositorioBuilder _instance;
    private readonly Mock<IReceitaReadOnlyRepositorio> _repositorio;

    private ReceitaReadOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IReceitaReadOnlyRepositorio>();
        }
    }

    public static ReceitaReadOnlyRepositorioBuilder Instancia()
    {
        _instance = new ReceitaReadOnlyRepositorioBuilder();
        return _instance;
    }

    public ReceitaReadOnlyRepositorioBuilder RecuperarTodasDoUsuario(Receita receita)
    {
        if (receita is not null)
            _repositorio.Setup(r => r.RecuperarTodasDoUsuario(receita.UsuarioId)).ReturnsAsync(new List<Receita> { receita });

        return this;
    }

    public ReceitaReadOnlyRepositorioBuilder RecuperarPorId(Receita receita)
    {
        _repositorio.Setup(r => r.RecuperarPorId(receita.Id)).ReturnsAsync(receita);

        return this;
    }

    /*public ReceitaReadOnlyRepositorioBuilder QuantidadeReceitas(int quantidadeReceitas)
    {
        _repositorio.Setup(r => r.QuantidadeReceitas(It.IsAny<long>())).ReturnsAsync(quantidadeReceitas);

        return this;
    }*/

    public IReceitaReadOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}