namespace Graphene.Transactions
{
    public readonly struct CreateVertex : IAction
    {
        public CreateVertex(IReadOnlyVertex target)
        {
            Target = target;
        }
        
        public IReadOnlyVertex Target { get; }
    }
}