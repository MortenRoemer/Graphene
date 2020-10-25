using System;

namespace Graphene {

    public interface IVertexEdgeRepository : IReadOnlyRepository<IEdge> {
        IEdge Add(IVertex other);

        IEdge Add(IVertex other, string label);

        void Delete(IEdge edge);

        void Clear();
    }

}