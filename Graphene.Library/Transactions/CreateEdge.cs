namespace Graphene.Transactions
{
    public readonly struct CreateEdge : IAction
    {
        public CreateEdge(IReadOnlyEdge target)
        {
            Target = target;
        }
        
        public IReadOnlyEdge Target { get; }
    }
}