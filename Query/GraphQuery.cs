using System;

namespace Graphene.Query
{
    public class GraphQuery
    {
        public QueryNode Root { get; set; }

        public bool Contains(IEntity entity)
        {
            return Root?.Contains(entity) ?? true; 
        }
    }
}