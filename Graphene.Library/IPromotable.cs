namespace Graphene
{
    public interface IPromotable<T>
    {
        T Promote();

        bool TryPromote(out T target);
    }
}