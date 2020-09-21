using System.Collections.Generic;
using Graphene.Query.Filter;

namespace Graphene.Query
{
    public class VertexQueryNode : QueryNode
    {
        public IEnumerable<string> Labels { get; set; }

        public EntityFilter Filter { get; set; }
    }
}