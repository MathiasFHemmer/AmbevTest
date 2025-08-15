namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(Guid id, Type entity) 
        : base($"{nameof(entity)} with ID {id} not found!")
        {
        }

        public NotFoundException(Guid id, string entity, Exception innerException) 
        : base($"{entity} with ID {id} not found!", innerException)
        {
        }
    }
}