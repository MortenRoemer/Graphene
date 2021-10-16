using Graphene.Query;

namespace Graphene
{
    public interface IReadOnlyGraph
    {
        string Name { get; }

        IQueryRoot Select();
    }
}