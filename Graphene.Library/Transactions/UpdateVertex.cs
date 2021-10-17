namespace Graphene.Transactions
{
    public class UpdateVertex
    {
        public UpdateVertex(IReadOnlyVertex target)
        {
            Target = target;
        }
        
        public IReadOnlyVertex Target { get; }
    }
}