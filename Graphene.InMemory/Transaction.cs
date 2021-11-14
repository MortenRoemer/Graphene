using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Transactions;

namespace Graphene.InMemory
{
    internal class Transaction
    {
        public Transaction(IEnumerable<IAction> actions, MemoryGraph graph)
        {
            Actions = actions;
            Graph = graph;
        }
        
        private IEnumerable<IAction> Actions { get; }
        
        private MemoryGraph Graph { get; }

        public void Handle()
        {
            var validationState = new ValidationState(Graph._Entities.Values);

            foreach (var action in Actions)
            {
                Validate(action, validationState);
            }

            foreach (var action in Actions)
            {
                Execute(action);
            }
        }

        private static void Validate(IAction action, ValidationState state)
        {
            switch (action)
            {
                case CreateVertex createVertex:
                    state.AssertValidNewId(action, createVertex.Target.Id);
                    state.AddVertex(createVertex.Target.Id);
                    break;

                case CreateEdge createEdge:
                    state.AssertValidNewId(action, createEdge.Target.Id);
                    state.AssertExistingVertex(action, createEdge.Target.FromVertex);
                    state.AssertExistingVertex(action, createEdge.Target.ToVertex);
                    state.AddEdge(createEdge.Target.Id, createEdge.Target.FromVertex, createEdge.Target.ToVertex);
                    break;
                
                case UpdateEntity updateEntity:
                    state.AssertExistingEntity(action, updateEntity.Target.Id);
                    break;
                
                case DeleteEntity deleteEntity:
                    state.AssertExistingEntity(action, deleteEntity.Target.Id);
                    state.DeleteEntity(deleteEntity.Target.Id);
                    break;

                default:
                    throw new NotImplementedException($"{action.GetType().Name} failed because it is not implemented");
            }
        }
        
        private void Execute(IAction action)
        {
            switch (action)
            {
                case CreateVertex createVertex:
                    Graph.CreateVertex(createVertex.Target);
                    break;

                case CreateEdge createEdge:
                    Graph.CreateEdge(createEdge.Target);
                    break;
                
                case UpdateEntity updateEntity:
                    Graph.UpdateEntity(updateEntity.Target);
                    break;
                
                case DeleteEntity deleteEntity:
                    Graph.DeleteEntity(deleteEntity.Target);
                    break;
                
                default:
                    throw new NotImplementedException($"{action.GetType().Name} failed because it is not implemented");
            }
        }

        private readonly struct ValidationState
        {
            public ValidationState(IEnumerable<IEntity> entities)
            {
                Entities = entities.ToDictionary(entity => entity.Id, entity => entity.EntityClass);
                EdgeByVertex = new Dictionary<Guid, List<Guid>>();

                foreach (var edge in entities.Where(entity => entity.EntityClass == EntityClass.Edge).Cast<IEdge>())
                {
                    if (EdgeByVertex.TryGetValue(edge.FromVertex, out var existingList))
                    {
                        existingList.Add(edge.Id);
                    }
                    else
                    {
                        EdgeByVertex.Add(edge.FromVertex, new List<Guid> {edge.Id});
                    }
                    
                    if (EdgeByVertex.TryGetValue(edge.ToVertex, out existingList))
                    {
                        existingList.Add(edge.Id);
                    }
                    else
                    {
                        EdgeByVertex.Add(edge.ToVertex, new List<Guid> {edge.Id});
                    }
                }
            }
            
            private Dictionary<Guid, EntityClass> Entities { get; }
            
            private Dictionary<Guid, List<Guid>> EdgeByVertex { get; }

            public void AddVertex(Guid id)
            {
                Entities.Add(id, EntityClass.Vertex);
            }

            public void AddEdge(Guid id, Guid fromVertex, Guid toVertex)
            {
                Entities.Add(id, EntityClass.Edge);
                
                if (EdgeByVertex.TryGetValue(fromVertex, out var existingList))
                {
                    existingList.Add(id);
                }
                else
                {
                    EdgeByVertex.Add(fromVertex, new List<Guid> {id});
                }
                    
                if (EdgeByVertex.TryGetValue(toVertex, out existingList))
                {
                    existingList.Add(id);
                }
                else
                {
                    EdgeByVertex.Add(toVertex, new List<Guid> {id});
                }
            }

            public void DeleteEntity(Guid id)
            {
                if (Entities[id] == EntityClass.Vertex && EdgeByVertex.TryGetValue(id, out var edgesToDelete))
                {
                    foreach (var edgeId in edgesToDelete)
                    {
                        Entities.Remove(edgeId);
                    }

                    EdgeByVertex.Remove(id);
                }
                
                Entities.Remove(id);
            }
            
            public void AssertValidNewId(IAction action, Guid id)
            {
                if (id == Guid.Empty)
                    throw new GraphActionException(action, $"{action.GetType().Name} failed because the empty Guid is not valid");
                if (Entities.ContainsKey(id))
                    throw new GraphActionException(action, $"{action.GetType().Name} failed because there is already an entity with id {id}");
            }

            public void AssertExistingEntity(IAction action, Guid id)
            {
                if (!Entities.ContainsKey(id))
                    throw new GraphActionException(action, $"{action.GetType().Name} failed because there is no entity with id {id}");
            }
            
            public void AssertExistingVertex(IAction action, Guid id)
            {
                if (!Entities.TryGetValue(id, out var entityClass) || entityClass != EntityClass.Vertex)
                    throw new GraphActionException(action, $"{action.GetType().Name} failed because there is no vertex with id {id}");
            }
        }
    }
}