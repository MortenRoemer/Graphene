namespace Graphene
{
    public interface IEntity : IReadOnlyEntity
    {
        new IGraph Graph { get; }

        new IAttributeSet Attributes { get; }
    }
}