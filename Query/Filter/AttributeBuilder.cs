using System;
using System.Collections.Generic;

namespace Graphene.Query.Filter
{
    public partial class FilterBuilder
    {
        public class AttributeBuilder
        {
            internal AttributeBuilder(FilterBuilder builder, string name)
            {
                Builder = builder ?? throw new ArgumentNullException(nameof(builder));
                Name = name ?? throw new ArgumentNullException(nameof(name));
            }

            private FilterBuilder Builder { get; }

            private string Name { get; }

            public FilterBuilder HasValue()
            {
                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.HasValue(Name)
                    }
                );

                return Builder;
            }

            public FilterBuilder IsBetween<T>(T from, T to) where T : IComparable<T>
            {
                Builder.TokenBuffer.Add(new Token.StartAndGroup());

                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.GreaterThan<T>(Name, from)
                    }
                );

                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.LessThan<T>(Name, to)
                    }
                );

                Builder.TokenBuffer.Add(new Token.EndGroup());
                return Builder;
            }

            public FilterBuilder IsContainedIn(IEnumerable<object> values)
            {
                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.In(Name, values)
                    }
                );

                return Builder;
            }

            public FilterBuilder IsEqualTo(object value) 
            {
                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.Equal(Name, value)
                    }
                );

                return Builder;
            }

            public FilterBuilder IsGreaterThan<T>(T value) where T : IComparable<T>
            {
                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.GreaterThan<T>(Name, value)
                    }
                );

                return Builder;
            }

            public FilterBuilder IsGreaterOrEqualTo<T>(T value) where T : IComparable<T>
            {
                Builder.TokenBuffer.Add(new Token.StartOrGroup());

                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.GreaterThan<T>(Name, value)
                    }
                );

                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.Equal(Name, value)
                    }
                );

                Builder.TokenBuffer.Add(new Token.EndGroup());
                return Builder;
            }

            public FilterBuilder IsLessThan<T>(T value) where T : IComparable<T>
            {
                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.LessThan<T>(Name, value)
                    }
                );

                return Builder;
            }

            public FilterBuilder IsLessOrEqualTo<T>(T value) where T : IComparable<T>
            {
                Builder.TokenBuffer.Add(new Token.StartOrGroup());

                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.LessThan<T>(Name, value)
                    }
                );

                Builder.TokenBuffer.Add(
                    new Token.Filter {
                        Content = new AttributeFilter.Equal(Name, value)
                    }
                );

                Builder.TokenBuffer.Add(new Token.EndGroup());
                return Builder;
            }
        }
    }
}