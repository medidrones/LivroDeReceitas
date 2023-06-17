using System.Collections;
using Utilitario.Test.Entidades;

namespace UseCases.Test.Conexao.InlineData;

public class EntidadesUsuarioConexaoDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var conexoes = ConexaoBuilder.Construir();

        return conexoes.Select(conexao => new object[] { conexao.Id, conexoes }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}