namespace Graphene.Transactions
{
    public readonly struct UpdateEntity : IAction
    {
        public UpdateEntity(IReadOnlyEntity target)
        {
            Target = target;
        }
        
        public IReadOnlyEntity Target { get; }
    }
}