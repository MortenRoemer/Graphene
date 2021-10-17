namespace Graphene.Transactions
{
    public class CreateVertex : IAction
    {
        public CreateVertex(IReadOnlyVertex target)
        {
            Target = target;
        }
        
        public IReadOnlyVertex Target { get; }
    }
}