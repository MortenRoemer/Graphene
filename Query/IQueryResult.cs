using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryResult
    {
        TimeSpan Duration { get; }

        float Efficiency { get; }

        IGraph Graph { get; }

        IEnumerable<IQuerySolution> Solutions { get; }
    }
}