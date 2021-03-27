namespace Graphene.InMemory.Query
{
    public readonly struct CachedResult<T>
    {
        public CachedResult(int timeStamp, T result)
        {
            TimeStamp = timeStamp;
            Result = result;
        }
        
        public int TimeStamp { get; }
        
        public T Result { get; }
    }
}