using AutoMapper;
using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Comunicacao.Respostas;
using LivroDeReceitas.Domain.Repositorios.Conexao;
using LivroDeReceitas.Domain.Repositorios.Receita;

namespace LivroDeReceitas.Application.UseCases.Conexao.Recuperar;

public class RecuperarTodasConexoesUseCase : IRecuperarTodasConexoesUseCase
{
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IReceitaReadOnlyRepositorio _repositorioReceita;
    private readonly IConexaoReadOnlyRepositorio _repositorio;
    private readonly IMapper _mapper;
    private IUsuarioLogado usuarioLogado;
    private IConexaoReadOnlyRepositorio repositorioConexao;
    private IMapper automapper;
    private IReceitaReadOnlyRepositorio repositorioReceita;

    public RecuperarTodasConexoesUseCase(IUsuarioLogado usuarioLogado, IReceitaReadOnlyRepositorio repositorioReceita, 
        IConexaoReadOnlyRepositorio repositorio, IMapper mapper)
    {
        _usuarioLogado = usuarioLogado;
        _repositorioReceita = repositorioReceita;
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public RecuperarTodasConexoesUseCase(IUsuarioLogado usuarioLogado, IConexaoReadOnlyRepositorio repositorioConexao, IMapper automapper, IReceitaReadOnlyRepositorio repositorioReceita)
    {
        this.usuarioLogado = usuarioLogado;
        this.repositorioConexao = repositorioConexao;
        this.automapper = automapper;
        this.repositorioReceita = repositorioReceita;
    }

    public async Task<RespostaConexoesDoUsuarioJson> Executar()
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var conexoes = await _repositorio.RecuperarDoUsuario(usuarioLogado.Id);

        var tarefas = conexoes.Select(async usuario =>
        {
            var quantidadeReceitas = await _repositorioReceita.QuantidadeReceitas(usuario.Id);
            var usuarioJson = _mapper.Map<RespostaUsuarioConectadoJson>(usuario);
            usuarioJson.QuantidadeReceitas = quantidadeReceitas;

            return usuarioJson;
        });

        return new RespostaConexoesDoUsuarioJson
        {
            Usuarios = await Task.WhenAll(tarefas)
        };
    }
}