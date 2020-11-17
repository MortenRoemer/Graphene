namespace Graphene
{
    public interface IReadOnlyEntity
    {
        IReadOnlyGraph Graph { get; }

        int Id { get; }

        string Label { get; }

        IReadOnlyAttributeSet Attributes { get; }
    }
}