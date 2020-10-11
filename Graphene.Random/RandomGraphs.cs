using System;
using System.Linq;

namespace Graphene.Random
{
    public static class RandomGraphs
    {
        public delegate IGraph GraphConstructor();
        public enum edgeType {bidirectedOnly, directedOnly, hybrid};
        public static IGraph GetRandomGraph(int vertexCount, double edgeProbability, GraphConstructor graphConstructor, edgeType edgeType, bool allowSelfdirectedEdges)
        {
            if (vertexCount < 0) throw new ArgumentOutOfRangeException();
            var graph = graphConstructor();
            var randomiser = new System.Random();
            for(int vertexIndex = 0; vertexIndex < vertexCount; vertexIndex++)
            {
                var newVertex = graph.Vertices.Create();
                foreach(var vertex in graph.Vertices.Where(vertex=>vertex.Id != newVertex.Id || allowSelfdirectedEdges))
                {
                    switch (edgeType)
                    {
                        case edgeType.bidirectedOnly:
                            if (randomiser.NextDouble() < edgeProbability) vertex.BidirectionalEdges.Add(newVertex);
                            break;
                        case edgeType.directedOnly:
                            if (randomiser.NextDouble() < edgeProbability) vertex.IngoingEdges.Add(newVertex);
                            if (!(vertex.Id == newVertex.Id) && randomiser.NextDouble() < edgeProbability) vertex.OutgoingEdges.Add(newVertex);
                            break;
                        case edgeType.hybrid:
                            if (randomiser.NextDouble() < edgeProbability) vertex.IngoingEdges.Add(newVertex);
                            if (!(vertex.Id == newVertex.Id) && randomiser.NextDouble() < edgeProbability) vertex.OutgoingEdges.Add(newVertex);
                            if (randomiser.NextDouble() < edgeProbability) vertex.BidirectionalEdges.Add(newVertex);
                            break;
                    }                                        
                }
            }
            return graph;
        }
    }
}
