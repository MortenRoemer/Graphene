using System;

namespace Graphene.Query.Route
{
    public interface IRoot
    {
        IFromVertex FromVertex(Guid vertexId);
    }
}