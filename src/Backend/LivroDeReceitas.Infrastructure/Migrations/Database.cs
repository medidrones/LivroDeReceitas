using Dapper;
using MySqlConnector;

namespace LivroDeReceitas.Infrastructure.Migrations;

public static class Database
{
    public static void CriarDatabase( string conexaoComBancoDeDados, string nomeDatabase)
    {
        using var minhaConexao = new MySqlConnection(conexaoComBancoDeDados);

        var paramentros = new DynamicParameters();
        paramentros.Add("nome", nomeDatabase);

        var registros = minhaConexao.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @nome", paramentros);

        if (!registros.Any())
        {
            minhaConexao.Execute($"CREATE DATABASE {nomeDatabase}");
        }
    }
}