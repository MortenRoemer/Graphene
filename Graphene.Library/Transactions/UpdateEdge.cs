namespace Graphene.Transactions
{
    public class UpdateEdge
    {
        public UpdateEdge(IReadOnlyEdge target)
        {
            Target = target;
        }
        
        public IReadOnlyEdge Target { get; }
    }
}