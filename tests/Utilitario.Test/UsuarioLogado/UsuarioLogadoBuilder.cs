﻿using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Domain.Entidades;
using Moq;

namespace Utilitario.Test.UsuarioLogado;

public class UsuarioLogadoBuilder
{
    private static UsuarioLogadoBuilder _instance;
    private readonly Mock<IUsuarioLogado> _repositorio;

    public UsuarioLogadoBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IUsuarioLogado>();
        }
    }

    public static UsuarioLogadoBuilder Instancia()
    {
        _instance = new UsuarioLogadoBuilder();

        return _instance;
    }

    public UsuarioLogadoBuilder RecuperarUsuario(Usuario usuario)
    {
        _repositorio.Setup(c => c.RecuperarUsuario()).ReturnsAsync(usuario);

        return this;
    }

    public IUsuarioLogado Construir()
    {
        return _repositorio.Object;
    }
}