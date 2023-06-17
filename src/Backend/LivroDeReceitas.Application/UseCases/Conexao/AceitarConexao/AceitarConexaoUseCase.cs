using HashidsNet;
using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Domain.Repositorios.Codigo;
using LivroDeReceitas.Domain.Repositorios.Conexao;
using LivroDeReceitas.Domain.Repositorios;

namespace LivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;

public class AceitarConexaoUseCase : IAceitarConexaoUseCase
{
    private readonly ICodigoWriteOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IHashids _hashids;
    private readonly IConexaoWriteOnlyRepositorio _repositorioConexoes;

    public AceitarConexaoUseCase(ICodigoWriteOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, 
        IUnidadeDeTrabalho unidadeDeTrabalho, IHashids hashids, IConexaoWriteOnlyRepositorio repositorioConexoes)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _hashids = hashids;
        _repositorioConexoes = repositorioConexoes;
    }

    public async Task<string> Executar(string usuarioParaSeConectarId)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        await _repositorio.Deletar(usuarioLogado.Id);

        var idUsuarioLeitorQRCode = _hashids.DecodeLong(usuarioParaSeConectarId).First();

        await _repositorioConexoes.Registrar(new Domain.Entidades.Conexao
        {
            UsuarioId = usuarioLogado.Id,
            ConectadoComUsuarioId = idUsuarioLeitorQRCode
        });

        await _repositorioConexoes.Registrar(new Domain.Entidades.Conexao
        {
            UsuarioId = idUsuarioLeitorQRCode,
            ConectadoComUsuarioId = usuarioLogado.Id
        });

        await _unidadeDeTrabalho.Commit();

        return _hashids.EncodeLong(usuarioLogado.Id);
    }
}