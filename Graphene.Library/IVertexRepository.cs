namespace Graphene
{
    public interface IVertexRepository : IRepository<IVertex>
    {
        IVertex Create();

        IVertex Create(string label);
    }
}