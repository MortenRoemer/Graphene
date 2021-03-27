using System.Collections.Generic;
using System.Linq;

namespace Graphene {

    public interface IVertexEdgeRepository : IReadOnlyRepository<IEdge> {
        
        IEdge Add(IVertex other, string label = null);

        void Delete(IEnumerable<int> ids);
        
        void Delete(int id);

        void Clear();
    }

    public static class VertexEdgeRepositoryExtension
    {
        public static void Delete(this IVertexEdgeRepository repository, IEnumerable<IReadOnlyEdge> edges)
        {
            repository.Delete(edges.Select(edge => edge.Id));
        }

        public static void Delete(this IVertexEdgeRepository repository, IReadOnlyEdge edge)
        {
            repository.Delete(edge.Id);
        }
    }
}