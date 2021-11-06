namespace Graphene.Transactions
{
    public class UpdateEdge : IAction
    {
        public UpdateEdge(IReadOnlyEdge target)
        {
            Target = target;
        }
        
        public IReadOnlyEdge Target { get; }
    }
}