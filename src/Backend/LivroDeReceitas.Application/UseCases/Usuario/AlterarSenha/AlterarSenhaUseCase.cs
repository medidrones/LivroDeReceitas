﻿using LivroDeReceitas.Application.Servicos.Criptografia;
using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Domain.Repositorios;
using LivroDeReceitas.Domain.Repositorios.Usuario;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaUseCase : IAlterarSenhaUseCase
{
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUsuarioUpdateOnlyRepositorio _repositorio;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public AlterarSenhaUseCase(IUsuarioUpdateOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, 
        EncriptadorDeSenha encriptadorDeSenha, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _encriptadorDeSenha = encriptadorDeSenha;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Executar(RequisicaoAlterarSenhaJson requisicao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var usuario = await _repositorio.RecuperarPorId(usuarioLogado.Id);

        Validar(requisicao, usuario);

        usuario.Senha = _encriptadorDeSenha.Criptografar(requisicao.NovaSenha);

        _repositorio.Update(usuario);

        await _unidadeDeTrabalho.Commit();
    }

    private void Validar(RequisicaoAlterarSenhaJson requisicao, Domain.Entidades.Usuario usuario)
    {
        var validator = new AlterarSenhaValidator();
        var resultado = validator.Validate(requisicao);
        var senhaAtualCriptografada = _encriptadorDeSenha.Criptografar(requisicao.SenhaAtual);

        if (!usuario.Senha.Equals(senhaAtualCriptografada))
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("senhaAtual", ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
        }

        if (!resultado.IsValid)
        {
            var mensagens = resultado.Errors.Select(x => x.ErrorMessage).ToList();

            throw new ErrosDeValidacaoException(mensagens);
        }
    }
}