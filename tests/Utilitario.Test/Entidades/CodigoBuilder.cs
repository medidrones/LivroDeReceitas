﻿using Bogus;
using LivroDeReceitas.Domain.Entidades;

namespace Utilitario.Test.Entidades;

public class CodigoBuilder
{
    public static Codigos Construir(Usuario usuario)
    {
        return new Faker<Codigos>()
            .RuleFor(c => c.Id, _ => usuario.Id)
            .RuleFor(c => c.UsuarioId, _ => usuario.Id)
            .RuleFor(c => c.Codigo, _ => Guid.NewGuid().ToString());
    }
}