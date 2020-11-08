namespace Graphene.InMemory.Utility
{
    public readonly struct CollectionChange<T>
    {
        public CollectionChange(T value, CollectionChangeMode mode)
        {
            Value = value;
            Mode = mode;
        }
        
        public CollectionChangeMode Mode { get; }
        
        public T Value { get; }
    }
}