namespace Graphene
{
    public interface IAttributeSet : IReadOnlyAttributeSet
    {
        new object this[string name] { get; set; }

        void Set(string name, object value);

        void Clear();
    }
}