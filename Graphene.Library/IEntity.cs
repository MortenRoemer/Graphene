namespace Graphene
{
    public interface IEntity
    {
        IGraph Graph { get; }

        ulong Id { get; }

        string Label { get; set; }

        IAttributeSet Attributes { get; }
    }
}