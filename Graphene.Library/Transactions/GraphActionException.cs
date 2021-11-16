using System;

namespace Graphene.Transactions
{
    public class GraphActionException : Exception
    {
        public GraphActionException(IAction action, string message) : base(message)
        {
            Action = action;
        }
        
        public IAction Action { get; }
    }
}