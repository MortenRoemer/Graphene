namespace Graphene
{
    public interface IEntity
    {
        IGraph Graph { get; }

        long Id { get; }

        string Label { get; set; }

        IAttributeSet Attributes { get; }
    }
}