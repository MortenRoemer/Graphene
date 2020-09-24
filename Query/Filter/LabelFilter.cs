using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Graphene.Query.Filter
{
    public abstract class LabelFilter : EntityFilter
    {
        public class Equal : LabelFilter
        {
            public Equal(string label)
            {
                Label = label ?? throw new ArgumentNullException(nameof(label));
            }

            public string Label { get; }

            public override bool Contains(IEntity entity) =>
                Label.Equals(entity.Label);
        }

        public class In : LabelFilter
        {
            public In(IEnumerable<string> labels)
            {
                Labels = labels ?? throw new ArgumentNullException(nameof(labels));
            }

            public IEnumerable<string> Labels { get; }

            public override bool Contains(IEntity entity) =>
                Labels.Contains(entity.Label);
        }

        public class Like : LabelFilter
        {
            public Like(string pattern)
            {
                Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            }

            public string Pattern { get; }

            public override bool Contains(IEntity entity) =>
                Regex.IsMatch(entity.Label, Pattern);
        }
    }
}