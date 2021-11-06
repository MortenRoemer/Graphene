namespace Graphene.Transactions
{
    public class UpdateVertex : IAction
    {
        public UpdateVertex(IReadOnlyVertex target)
        {
            Target = target;
        }
        
        public IReadOnlyVertex Target { get; }
    }
}