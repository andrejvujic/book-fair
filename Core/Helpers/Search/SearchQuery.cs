namespace Core.Helpers.Search
{
    public class SearchQuery
    {
        public int Length => Tokens.Count;
        public List<string> Tokens { get; } = new List<string>();
        public bool IsEmpty => Tokens.Count == 0;    

        public SearchQuery(string rawSearchQuery, int maxTokenCount) {
            ParseRawSearchQuery(rawSearchQuery, maxTokenCount);
        }

        private void ParseRawSearchQuery(string rawSearchQuery, int maxTokenCount)
        {
            if (string.IsNullOrWhiteSpace(rawSearchQuery)) return;
            List<string> splitRawSearchQuery = rawSearchQuery.Split(SEPARATOR, maxTokenCount).ToList();
            foreach (string s in splitRawSearchQuery)
                Tokens.Add(s.Trim().ToLower());
        }

        const string SEPARATOR = ",";
    }
}
