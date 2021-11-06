using System.Collections;
using System.Collections.Generic;

namespace Graphene.Transactions
{
    public class Transaction : IEnumerable<IAction>
    {
        private List<IAction> Actions { get; } = new();

        public int Count => Actions.Count;

        public IEnumerator<IAction> GetEnumerator() 
            => Actions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();

        public Transaction Add(IAction action)
        {
            Actions.Add(action);
            return this;
        }

        public Transaction AddRange(IEnumerable<IAction> actions)
        {
            Actions.AddRange(actions);
            return this;
        }
    }
}