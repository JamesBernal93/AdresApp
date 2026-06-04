namespace AdresApp.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entity, object id)
        : base($"La entidad '{entity}' con identificador '{id}' no fue encontrada.") { }
}

public class ExternalServiceException : DomainException
{
    public ExternalServiceException(string message) : base(message) { }
    public ExternalServiceException(string message, Exception innerException) : base(message, innerException) { }
}
