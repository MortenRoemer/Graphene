namespace Graphene
{
    public interface IAttributeSet : IReadOnlyAttributeSet
    {
        void Set(string name, object? value);

        void Clear();
    }
}