﻿using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Domain.Repositorios.Codigo;
using Moq;

namespace Utilitario.Test.Repositorios;

public class CodigoReadOnlyRepositorioBuilder
{
    private static CodigoReadOnlyRepositorioBuilder _instance;
    private readonly Mock<ICodigoReadOnlyRepositorio> _repositorio;

    private CodigoReadOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<ICodigoReadOnlyRepositorio>();
        }
    }

    public static CodigoReadOnlyRepositorioBuilder Instancia()
    {
        _instance = new CodigoReadOnlyRepositorioBuilder();

        return _instance;
    }

    public CodigoReadOnlyRepositorioBuilder RecuperarEntidadeCodigo(Codigos codigo)
    {
        _repositorio.Setup(x => x.RecuperarEntidadeCodigo(codigo.Codigo)).ReturnsAsync(codigo);

        return this;
    }

    public ICodigoReadOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}