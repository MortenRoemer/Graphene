namespace Graphene.Transactions
{
    public class CreateEdge : IAction
    {
        public CreateEdge(IReadOnlyEdge target)
        {
            Target = target;
        }
        
        public IReadOnlyEdge Target { get; }
    }
}