using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Graphene.Query;

namespace Graphene.InMemory
{
    public class MemoryGraph : IGraph
    {
        public MemoryGraph(string name)
        {
            Name = name;
        }

        public string Name { get; }

        private ReaderWriterLockSlim Lock { get; } = new(LockRecursionPolicy.NoRecursion);

        private SortedDictionary<Guid, IEntity> Entities { get; } = new();

        private SortedDictionary<Guid, SortedSet<Guid>> EdgesByVertex { get; } = new();

        internal IReadOnlyDictionary<Guid, IEntity> _Entities => Entities;

        public IQueryRoot Select()
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyEntity> Get(Guid id)
        {
            return await Task.Run(() =>
            {
                Lock.EnterReadLock();
                IEntity result;
                if (Entities.TryGetValue(id, out var entity))
                    result = entity;
                else
                {
                    Lock.ExitReadLock();
                    throw new ArgumentException($"There is no entity with the id {id}");
                }

                Lock.ExitReadLock();
                return result;
            });
        }

        public async Task<IFindResult<IReadOnlyEntity>> FindEntities(int pageSize, Expression<Func<IReadOnlyEntity, bool>>? filter = null)
        {
            return await Task.Run(() => Find(pageSize, Guid.Empty, filter));
        }

        public async Task<IFindResult<IReadOnlyVertex>> FindVertices(int pageSize, Expression<Func<IReadOnlyVertex, bool>>? filter = null)
        {
            return await Task.Run(() => Find(pageSize, Guid.Empty, filter));
        }

        public async Task<IFindResult<IReadOnlyEdge>> FindEdges(int pageSize, Expression<Func<IReadOnlyEdge, bool>>? filter = null)
        {
            return await Task.Run(() => Find(pageSize, Guid.Empty, filter));
        }

        internal IFindResult<T> Find<T>(
            int pageSize, 
            Guid offset, 
            Expression<Func<T, bool>>? filter
        ) where T : IReadOnlyEntity
        {
            Lock.EnterReadLock();
            var results = new List<T>(pageSize);

            IEnumerable<IReadOnlyEntity> source = Entities.Values;

            if (offset != Guid.Empty)
                source = source.SkipWhile(entity => entity.Id.CompareTo(offset) <= 0);
            
            var castedSource = source
                .Where(entity => entity is T)
                .Cast<T>();

            if (filter is not null)
                castedSource = castedSource.Where(filter.Compile());
                
            foreach (var entity in castedSource)
            {
                results.Add(entity);

                if (results.Count < pageSize)
                    continue;
                    
                Lock.ExitReadLock();
                return new FindResult<T>(this, filter, pageSize, true, results);
            }
                
            Lock.ExitReadLock();
            return new FindResult<T>(this, filter, pageSize, false, results);
        }

        public async Task Execute(Transactions.Transaction transaction)
        {
            await Task.Run(() =>
            {
                Lock.EnterWriteLock();
                try
                {
                    new Transaction(transaction, this).Handle();
                }
                finally
                {
                    Lock.ExitWriteLock();    
                }
            });
        }

        public async Task Clear()
        {
            await Task.Run(() =>
            {
                Lock.EnterWriteLock();
                Entities.Clear();
                EdgesByVertex.Clear();
                Lock.ExitWriteLock();
            });
        }

        internal void CreateVertex(IReadOnlyVertex vertex)
        {
            var newVertex = new MemoryVertex(vertex.Label, vertex.Id);
            newVertex.Attributes.PatchWith(vertex.Attributes);
            Entities.Add(vertex.Id, newVertex);
        }

        internal void UpdateVertex(IReadOnlyVertex vertex)
        {
            var existingVertex = Entities[vertex.Id];
            existingVertex.Attributes.PatchWith(vertex.Attributes);
        }

        internal void CreateEdge(IReadOnlyEdge edge)
        {
            var newEdge = new MemoryEdge(edge.Label, edge.FromVertex, edge.ToVertex, edge.Directed, edge.Id);
            newEdge.Attributes.PatchWith(edge.Attributes);
            Entities.Add(newEdge.Id, newEdge);
            AddEdgeToVertexLink(newEdge.Id, newEdge.FromVertex);
            AddEdgeToVertexLink(newEdge.Id, newEdge.ToVertex);
        }
        
        internal void UpdateEdge(IReadOnlyEdge edge)
        {
            var existingEdge = Entities[edge.Id];
            existingEdge.Attributes.PatchWith(edge.Attributes);
        }

        internal void DeleteEntity(IEntityReference entity)
        {
            if (entity.EntityClass == EntityClass.Vertex && EdgesByVertex.TryGetValue(entity.Id, out var edgesToDelete))
            {
                foreach (var edgeId in edgesToDelete)
                {
                    Entities.Remove(edgeId);
                }

                EdgesByVertex.Remove(entity.Id);
            }

            Entities.Remove(entity.Id);
        }
        
        private void AddEdgeToVertexLink(Guid edgeId, Guid vertexId)
        {
            if (EdgesByVertex.TryGetValue(vertexId, out var edgeIds))
            {
                edgeIds.Add(edgeId);
            }
            else
            {
                EdgesByVertex.Add(vertexId, new SortedSet<Guid> {edgeId});
            }
        }
    }
}