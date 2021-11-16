namespace Graphene
{
    public interface IAttributeSet : IReadOnlyAttributeSet
    {
        void Set(string name, object? value);

        void Clear();
    }

    public static class AttributeSetExtension
    {
        public static void PatchWith(this IAttributeSet target, IReadOnlyAttributeSet source)
        {
            foreach (var (key, value) in source)
            {
                target.Set(key, value);
            }
        }
    }
}