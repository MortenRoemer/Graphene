namespace Graphene
{
    public interface IEntity : IReadOnlyEntity
    {
        new IGraph Graph { get; }

        new string Label { get; set; }

        new IAttributeSet Attributes { get; }
    }
}