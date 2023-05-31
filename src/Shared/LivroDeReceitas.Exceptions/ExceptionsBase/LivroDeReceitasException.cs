using System.Runtime.Serialization;

namespace LivroDeReceitas.Exceptions.ExceptionsBase;

[Serializable]
public class LivroDeReceitasException : SystemException
{
    public LivroDeReceitasException(string message) : base(message)
    {
    }

    protected LivroDeReceitasException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}