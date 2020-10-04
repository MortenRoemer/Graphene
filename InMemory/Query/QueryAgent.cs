using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    internal class QueryAgent
    {
        public QueryAgent(BuilderRoot query, IEnumerable<ulong> offset)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Offset = offset is null ? null : offset.ToArray();
        }

        private ulong[] Offset { get; }

        private BuilderRoot Query { get; }

        public bool FindSolution(out IQueryResult result)
        {
            throw new NotImplementedException();
        }

        private bool FindNextVertex(BuilderVertex vertexDefinition, out IVertex vertex)
        {
            throw new NotImplementedException();
        }

        private bool FindNextVertexRelativeTo(IEdge edge, BuilderVertex vertexDefinition, out IVertex vertex)
        {
            throw new NotImplementedException();
        }

        private bool FindNextEdge(BuilderEdge edgeDefinition, out IEdge edge)
        {
            throw new NotImplementedException();
        }

        private bool FindNextEdgeRelativeTo(IVertex vertex, BuilderEdge edgeDefinition, out IEdge edge)
        {
            throw new NotImplementedException();
        }

        private bool FindNextRoute(BuilderRoute routeDefinition, IEntity fromEntity, out IEnumerable<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}