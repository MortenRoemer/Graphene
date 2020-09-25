using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public class QueryBuilder
    {
        public VertexQueryBuilder AnyVertex()
        {
            return new VertexQueryBuilder(this);
        }

        public EdgeQueryBuilder AnyEdges()
        {
            return new EdgeQueryBuilder(this);
        }

        public EdgeQueryBuilder Edge(Guid guid)
        {
            return new EdgeQueryBuilder(this, new[] { guid });
        }

        public EdgeQueryBuilder EdgesIn(IEnumerable<Guid> range)
        {
            return new EdgeQueryBuilder(this, range);
        }

        public VertexQueryBuilder Vertex(Guid guid)
        {
            return new VertexQueryBuilder(this, new[] { guid });
        }

        public VertexQueryBuilder VerticesIn(IEnumerable<Guid> range)
        {
            return new VertexQueryBuilder(this, range);
        }
    }
}