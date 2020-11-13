namespace Graphene
{
    public interface IEntity
    {
        IGraph Graph { get; }

        int Id { get; }

        string Label { get; set; }

        IAttributeSet Attributes { get; }
    }
}