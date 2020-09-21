namespace Graphene
{
    public interface IVertexRepository : IRepository<IVertex>
    {
        IVertex Create();
    }
}