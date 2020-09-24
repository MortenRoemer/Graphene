using System.Collections.Generic;

namespace Graphene.Query
{
    public class QueryBuilder
    {
        private List<Token> TokenBuffer { get; } = new List<Token>();

        

        private abstract class Token
        {

        }
    }
}