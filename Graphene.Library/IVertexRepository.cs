namespace Graphene
{
    public interface IVertexRepository : IRepository<IVertex>
    {
        IVertex Create(string label = null);
    }
}