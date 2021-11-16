namespace Graphene.Transactions
{
    public readonly struct DeleteEntity : IAction
    {
        public DeleteEntity(IEntityReference target)
        {
            Target = target;
        }
        
        public IEntityReference Target { get; }
    }
}