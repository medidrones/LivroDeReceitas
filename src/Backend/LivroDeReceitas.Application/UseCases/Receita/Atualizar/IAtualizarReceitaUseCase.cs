﻿using LivroDeReceitas.Comunicacao.Requisicoes;

namespace LivroDeReceitas.Application.UseCases.Receita.Atualizar;

public interface IAtualizarReceitaUseCase
{
    Task Executar(long  id, RequisicaoReceitaJson requisicao);
}