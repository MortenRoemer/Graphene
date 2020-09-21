namespace Graphene {

    public interface IVertexEdgeRepository : IReadOnlyRepository<IEdge> {
        IEdge Add(IVertex other);

        void Delete();
    }

}