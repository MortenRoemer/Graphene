using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    internal class QueryAgent
    {
        public QueryAgent(BuilderRoot query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        private BuilderRoot Query { get; }

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
                    throw new NotImplementedException();
                }
                else if (token is BuilderRoute routeDefinition)
                {
                    throw new NotImplementedException();
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
                ? Query.Graph.Vertices
                : Query.Graph.Vertices.Get(edgeDefinition.Range);

            if (edgeDefinition.Filter != null)
                edges = edges.Where(vertex => edgeDefinition.Filter.Contains(vertex));

            foreach (var edge in edges)
            {
                stack.Push(edge);

                if (Query.Tokens.Count <= stack.Count)
                    return true;

                var token = Query.Tokens[stack.Count];

                if (token is BuilderVertex vertexDefinition)
                {
                    throw new NotImplementedException();
                }
                else if (token is BuilderRoute routeDefinition)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new InvalidOperationException($"query token type {token.GetType()} not supported relative to edge");
                }

                stack.Pop();
            }

            return false;
        }

        private IQueryResult PackResult(Stack<IEntity> stack)
        {
            throw new NotImplementedException();
        }
    }
}