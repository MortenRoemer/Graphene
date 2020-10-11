using System;

namespace Graphene {

    public interface IVertexEdgeRepository : IReadOnlyRepository<IEdge> {
        IEdge Add(IVertex other);

        void Delete(IEdge edge);

        void Clear();
    }

}