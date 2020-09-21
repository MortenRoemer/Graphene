namespace Graphene {

    public interface IVertexEdgeRepository : IReadOnlyRepository<IEdge> {
        IEdge Add(IVertex other, bool directed);

        void Delete();
    }

}