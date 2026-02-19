using System.Diagnostics;

namespace Core.Helpers.Search
{
    public class SearchQuery
    {
        public int Length => Tokens == null ? 0 : Tokens.Count;
        public List<string> Tokens { get; } = new List<string>();
        public bool Empty => Tokens == null || Tokens.Count == 0;    

        public SearchQuery(string rawSearchQuery, int maxTokenCount) {
            ParseRawSearchQuery(rawSearchQuery, maxTokenCount);
        }

        private void ParseRawSearchQuery(string rawSearchQuery, int maxTokenCount)
        {
            if (string.IsNullOrWhiteSpace(rawSearchQuery)) return;
            List<string> splitRawSearchQuery = rawSearchQuery.Split(SEPARATOR, maxTokenCount).ToList();
            foreach (string s in splitRawSearchQuery)
                Tokens.Add(Normalize(s));
        }

        public static string Normalize(string value) => value.Trim().ToLower();

        public bool IsMatch(string[] f)
        {
            if (f.Length == Length)
            {
                for (int index = 0; index < Length; ++index)
                    if (!Normalize(f[index]).Contains(Tokens[index])) return false;
                return true;
            }

            return false;
        }

        const string SEPARATOR = ",";
    }
}
