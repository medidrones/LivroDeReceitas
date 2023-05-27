namespace LivroDeReceitas.Comunicacao.Respostas;

public class RespostaErroJson
{
    public List<string> Mensagens { get; set; }

    public RespostaErroJson(List<string> mensagem)
    {
        Mensagens = mensagem;
    }
}