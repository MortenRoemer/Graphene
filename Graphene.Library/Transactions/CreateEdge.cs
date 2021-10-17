namespace Graphene.Transactions
{
    public class CreateEdge
    {
        public CreateEdge(IReadOnlyEdge target)
        {
            Target = target;
        }
        
        public IReadOnlyEdge Target { get; }
    }
}