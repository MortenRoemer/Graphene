using System;

namespace Graphene.Random
{
    [Flags]
    public enum EdgeGenerationRule : byte
    {
        NoEdges = 0b_0000,
        AllowUndirected = 0b_0001,
        AllowDirected = 0b_0010,
        AllowEdgesToItself = 0b_0100,
    };
    
}