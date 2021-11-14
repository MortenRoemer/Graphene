namespace Graphene.Transactions
{
    public class UpdateEntity : IAction
    {
        public UpdateEntity(IReadOnlyEntity target)
        {
            Target = target;
        }
        
        public IReadOnlyEntity Target { get; }
    }
}