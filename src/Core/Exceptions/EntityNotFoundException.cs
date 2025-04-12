namespace EasyPoll.Core.Exceptions;

public sealed class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string entityName, string searchTerm, string searchValue)
        : base($"Entity {entityName} with {searchTerm}: {searchValue} not found")
    {
    }
}