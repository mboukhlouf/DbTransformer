using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DbTransformer.Transform
{
    public class Transformer
    {
        private string delimiter;

        public string Delimiter
        {
            get => delimiter;
            set
            {
                if (value == null || value == "")
                    throw new ArgumentException();
                delimiter = value;
            }
        }

        public Transformer(string delimiter = null)
        {
            Delimiter = delimiter;
        }

        public string[] Split(string text)
        {
            return Regex.Split(text, Regex.Escape(Delimiter));
        }

        public string Transform(string text, string format)
        {
            var tokens = Split(text);
            var groupsCount = tokens.Length;
            var regex = BuildRegex(groupsCount);
            return regex.Replace(text, format);
        }

        private Regex BuildRegex(int groupsCount)
        {
            StringBuilder patternBuilder = new StringBuilder();
            patternBuilder.Append("^");
            for (int i = 0; i < groupsCount; i++)
            {
                patternBuilder.Append($"([^{Regex.Escape(Delimiter)}]*)");
                // Delimiter
                if (i != groupsCount - 1)
                {
                    patternBuilder.Append(Regex.Escape(Delimiter));
                }
            }
            patternBuilder.Append("$");
            return new Regex(patternBuilder.ToString());
        }
    }
}
