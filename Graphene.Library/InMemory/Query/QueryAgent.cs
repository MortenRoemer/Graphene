using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    internal class QueryAgent
    {
        public QueryAgent(BuilderRoot query) : this(query, null) {}

        public QueryAgent(BuilderRoot query, ulong[] offset)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Offset = offset;
        }

        private BuilderRoot Query { get; }

        private ulong[] Offset { get; }

        public bool FindSolution(out IQueryResult result)
        {
            var stack = new Stack<IEntity>();
            var token = Query.Tokens.FirstOrDefault();

            if (token is BuilderVertex vertexDefinition)
            {
                if (FindVertices(stack, vertexDefinition))
                {
                    result = PackResult(stack);
                    return true;
                }
            }
            else if (token is BuilderEdge edgeDefinition)
            {
                if (FindEdges(stack, edgeDefinition))
                {
                    result = PackResult(stack);
                    return true;
                }
            }
            else
                throw new InvalidOperationException($"query token type {token.GetType()} not supported on base level");

            result = default;
            return false;
        }

        private bool FindVertices(Stack<IEntity> stack, BuilderVertex vertexDefinition)
        {
            var vertices = vertexDefinition.Range is null
                ? Query.Graph.Vertices
                : Query.Graph.Vertices.Get(vertexDefinition.Range);

            if (Offset != null)
                vertices = vertices.SkipWhile(vertex => vertex.Id <= Offset[stack.Count]);

            if (vertexDefinition.Filter != null)
                vertices = vertices.Where(vertex => vertexDefinition.Filter.Contains(vertex));

            foreach (var vertex in vertices)
            {
                stack.Push(vertex);

                if (Query.Tokens.Count <= stack.Count)
                    return true;

                var token = Query.Tokens[stack.Count];

                if (token is BuilderEdge edgeDefinition)
                {
                    if (FindEdgesRelativeTo(stack, vertex, edgeDefinition))
                        return true;
                }
                else if (token is BuilderRoute routeDefinition)
                {
                    if (FindRouteRelativeTo(stack, vertex, routeDefinition))
                        return true;
                }
                else
                {
                    throw new InvalidOperationException($"query token type {token.GetType()} not supported relative to vertex");
                }

                stack.Pop();
            }

            return false;
        }

        private bool FindVerticesRelativeTo(Stack<IEntity> stack, IEdge edge, BuilderVertex vertexDefinition)
        {
            IEnumerable<IVertex> vertices;

            switch (vertexDefinition.SearchMode)
            {
                case VertexSearchMode.All:
                    vertices = edge.Vertices;
                    break;

                case VertexSearchMode.Source:
                    vertices = edge.Directed
                        ? new[] { edge.FromVertex }
                        : edge.Vertices as IEnumerable<IVertex>;
                    break;

                case VertexSearchMode.Target:
                    vertices = edge.Directed
                        ? new[] { edge.ToVertex }
                        : edge.Vertices as IEnumerable<IVertex>;
                    break;

                default:
                    throw new NotImplementedException();
            }
            
            if (Offset != null)
                vertices = vertices.SkipWhile(vertex => vertex.Id <= Offset[stack.Count]);

            if (vertexDefinition.Filter != null)
                vertices = vertices.Where(vertex => vertexDefinition.Filter.Contains(vertex));

            foreach (var vertex in vertices)
            {
                stack.Push(vertex);

                if (Query.Tokens.Count <= stack.Count)
                    return true;

                var token = Query.Tokens[stack.Count];

                if (token is BuilderEdge edgeDefinition)
                {
                    if (FindEdgesRelativeTo(stack, vertex, edgeDefinition))
                        return true;
                }
                else if (token is BuilderRoute routeDefinition)
                {
                    if (FindRouteRelativeTo(stack, vertex, routeDefinition))
                        return true;
                }
                else
                {
                    throw new InvalidOperationException($"query token type {token.GetType()} not supported relative to vertex");
                }

                stack.Pop();
            }

            return false;
        }

        private bool FindEdges(Stack<IEntity> stack, BuilderEdge edgeDefinition)
        {
            var edges = edgeDefinition.Range is null
                ? Query.Graph.Edges
                : Query.Graph.Edges.Get(edgeDefinition.Range);

            if (Offset != null)
                edges = edges.SkipWhile(edge => edge.Id <= Offset[stack.Count]);

            if (edgeDefinition.Filter != null)
                edges = edges.Where(edge => edgeDefinition.Filter.Contains(edge));

            foreach (var edge in edges)
            {
                stack.Push(edge);

                if (Query.Tokens.Count <= stack.Count)
                    return true;

                var token = Query.Tokens[stack.Count];

                if (token is BuilderVertex vertexDefinition)
                {
                    if (FindVerticesRelativeTo(stack, edge, vertexDefinition))
                        return true;
                }
                else if (token is BuilderRoute routeDefinition)
                {
                    if (FindRouteRelativeTo(stack, edge, routeDefinition))
                        return true;
                }
                else
                {
                    throw new InvalidOperationException($"query token type {token.GetType()} not supported relative to edge");
                }

                stack.Pop();
            }

            return false;
        }

        private bool FindEdgesRelativeTo(Stack<IEntity> stack, IVertex vertex, BuilderEdge edgeDefinition)
        {
            IEnumerable<IEdge> edges;

            switch (edgeDefinition.SearchMode)
            {
                case EdgeSearchMode.All:
                    edges = vertex.Edges;
                    break;

                case EdgeSearchMode.Ingoing:
                    edges = vertex.BidirectionalEdges.Concat(vertex.IngoingEdges);
                    break;

                case EdgeSearchMode.Outgoing:
                    edges = vertex.BidirectionalEdges.Concat(vertex.OutgoingEdges);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (Offset != null)
                edges = edges.SkipWhile(edge => edge.Id <= Offset[stack.Count]);

            if (edgeDefinition.Filter != null)
                edges = edges.Where(edge => edgeDefinition.Filter.Contains(edge));

            foreach (var edge in edges)
            {
                stack.Push(edge);

                if (Query.Tokens.Count <= stack.Count)
                    return true;

                var token = Query.Tokens[stack.Count];

                if (token is BuilderVertex vertexDefinition)
                {
                    if (FindVerticesRelativeTo(stack, edge, vertexDefinition))
                        return true;
                }
                else if (token is BuilderRoute routeDefinition)
                {
                    if (FindRouteRelativeTo(stack, edge, routeDefinition))
                        return true;
                }
                else
                {
                    throw new InvalidOperationException($"query token type {token.GetType()} not supported relative to edge");
                }

                stack.Pop();
            }

            return false;
        }

        private bool FindRouteRelativeTo(Stack<IEntity> stack, IEntity entity, BuilderRoute routeDefinition)
        {
            if (routeDefinition.OptimizerSettings is null)
                throw new NotImplementedException();

            if (routeDefinition.OptimizerSettings.AggregateMode != OptimizerAggregateMode.Sum)
                throw new NotImplementedException();

            if (routeDefinition.OptimizerSettings.TargetMode != OptimizerTargetMode.Minimum)
                throw new NotImplementedException();

            throw new NotImplementedException();
        }

        private IQueryResult PackResult(Stack<IEntity> stack)
        {
            return new AgentResult(Query, stack.Reverse());
        }
    }
}