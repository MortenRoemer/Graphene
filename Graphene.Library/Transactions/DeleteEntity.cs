namespace Graphene.Transactions
{
    public class DeleteEntity : IAction
    {
        public DeleteEntity(IEntityReference target)
        {
            Target = target;
        }
        
        public IEntityReference Target { get; }
    }
}