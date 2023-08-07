using FluentMigrator;

namespace LivroDeReceitas.Infrastructure.Migrations.Versoes;

[Migration((long)NumeroVersoes.AlterarTabelaReceitas, "Adicionando coluna Tempo para o preparo")]
public class Versao0000003 : Migration
{
    public override void Up()
    {
        Alter.Table("Receitas").AddColumn("TempoPreparo").AsInt32().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
    }
}