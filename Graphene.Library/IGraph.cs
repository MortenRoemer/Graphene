using System.Threading.Tasks;
using Graphene.Transactions;

namespace Graphene
{
    public interface IGraph : IReadOnlyGraph
    {
        Task Execute(IAction action);

        Task Clear();
    }
}
