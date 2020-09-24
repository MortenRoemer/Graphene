using System;
using System.Collections.Generic;

namespace Graphene.Query.Filter
{
    public partial class FilterBuilder
    {
        private List<Token> TokenBuffer { get; } = new List<Token>();

        private int Depth { get; set; }

        public FilterBuilder AndGroup()
        {
            TokenBuffer.Add(new Token.StartAndGroup());
            Depth++;
            return this;
        }

        public AttributeBuilder Attribute(string name)
        {
            return new AttributeBuilder(this, name);
        }

        public FilterBuilder EndGroup()
        {
            if (Depth == 0)
                return this;

            TokenBuffer.Add(new Token.EndGroup());
            Depth--;
            return this;
        }

        public EntityFilter FinishFilter()
        {
            using (var enumerator = TokenBuffer.GetEnumerator())
            {
                return BuildGroup(enumerator, group => new AndGroupFilter(group));
            }
        }

        private static EntityFilter BuildGroup(IEnumerator<Token> enumerator, Func<IEnumerable<EntityFilter>, EntityFilter> constructor)
        {
            var members = new List<EntityFilter>();

            while (enumerator.MoveNext() && !(enumerator.Current is Token.EndGroup))
            {
                members.Add(BuildToken(enumerator, inversed: false));
            }

            return constructor.Invoke(members);
        }

        private static EntityFilter BuildToken(IEnumerator<Token> enumerator, bool inversed)
        {
            if (enumerator.Current is Token.Not)
            {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("unexpected token after not-token: <empty>");

                return BuildToken(enumerator, !inversed);
            }

            EntityFilter result;
            
            if (enumerator.Current is Token.Filter filtertoken)
                result = filtertoken.Content;
            else if (enumerator.Current is Token.StartAndGroup)
                result = BuildGroup(enumerator, group => new AndGroupFilter(group));
            else if (enumerator.Current is Token.StartOrGroup)
                result = BuildGroup(enumerator, group => new OrGroupFilter(group));
            else
                throw new InvalidOperationException($"unexpected token: {enumerator.Current.GetType()}");

            return inversed ? new NotFilter(result) : result;
        }

        public FilterBuilder OrGroup()
        {
            TokenBuffer.Add(new Token.StartOrGroup());
            Depth++;
            return this;
        }

        public FilterBuilder HasAnyLabelIn(IEnumerable<string> labels)
        {
            TokenBuffer.Add(
                new Token.Filter {
                    Content = new LabelFilter.In(labels)
                }
            );

            return this;
        }

        public FilterBuilder HasLabelEqualTo(string label)
        {
            TokenBuffer.Add(
                new Token.Filter {
                    Content = new LabelFilter.Equal(label)
                }
            );

            return this;
        }

        public FilterBuilder HasLabelLike(string pattern)
        {
            TokenBuffer.Add(
                new Token.Filter {
                    Content = new LabelFilter.Like(pattern)
                }
            );

            return this;
        }

        public FilterBuilder Not()
        {
            TokenBuffer.Add(new Token.Not());
            return this;
        }

        internal abstract class Token
        {
            public class StartAndGroup : Token {}

            public class StartOrGroup : Token {}

            public class EndGroup : Token {}

            public class Filter : Token
            {
                public EntityFilter Content { get; set; }
            }

            public class Not : Token {}
        }

        public static implicit operator EntityFilter(FilterBuilder builder) => builder.FinishFilter();
    }
}