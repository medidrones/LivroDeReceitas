using HashidsNet;
using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Comunicacao.Respostas;
using LivroDeReceitas.Domain.Repositorios.Codigo;
using LivroDeReceitas.Domain.Repositorios.Conexao;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;

namespace LivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;

public class QRCodeLidoUseCase : IQRCodeLidoUseCase
{
    private readonly IHashids _hashids;
    private readonly IConexaoReadOnlyRepositorio _repositorioConexao;
    private readonly ICodigoReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private ICodigoReadOnlyRepositorio repositorioCodigo;
    private IUsuarioLogado usuarioLogado;
    private IConexaoReadOnlyRepositorio repositorioConexao;
    private Hashids hashids;

    public QRCodeLidoUseCase(IHashids hashids, IConexaoReadOnlyRepositorio repositorioConexao, ICodigoReadOnlyRepositorio repositorio, 
        IUsuarioLogado usuarioLogado)
    {
        _hashids = hashids;
        _repositorioConexao = repositorioConexao;
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
    }

    public QRCodeLidoUseCase(ICodigoReadOnlyRepositorio repositorioCodigo, IUsuarioLogado usuarioLogado, IConexaoReadOnlyRepositorio repositorioConexao, Hashids hashids)
    {
        this.repositorioCodigo = repositorioCodigo;
        this.usuarioLogado = usuarioLogado;
        this.repositorioConexao = repositorioConexao;
        this.hashids = hashids;
    }

    public async Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string idUsuarioQueGerouQRCode)> Executar(string codigoConexao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var codigo = await _repositorio.RecuperarEntidadeCodigo(codigoConexao);

        await Validar(codigo, usuarioLogado);

        var usuarioParaSeConectar = new RespostaUsuarioConexaoJson
        {
            Id = _hashids.EncodeLong(usuarioLogado.Id),
            Nome = usuarioLogado.Nome
        };
        
        return (usuarioParaSeConectar, _hashids.EncodeLong(codigo.UsuarioId));
    }

    private async Task Validar(Domain.Entidades.Codigos codigo, Domain.Entidades.Usuario usuarioLogado)
    {
        if (codigo is null)
        {
            throw new LivroDeReceitasException(ResourceMensagensDeErro.CODIGO_NAO_ENCONTRADO);
        }

        if (codigo.UsuarioId == usuarioLogado.Id)
        {
            throw new LivroDeReceitasException(ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO);
        }

        var existeConexao = await _repositorioConexao.ExisteConexao(codigo.UsuarioId, usuarioLogado.Id);

        if (existeConexao)
        {
            throw new LivroDeReceitasException(ResourceMensagensDeErro.ESTA_CONEXAO_JA_EXISTE);
        }
    }
}