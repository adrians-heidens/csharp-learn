using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsLearn
{
    /// <summary>
    /// Custom dict-like type wrapping a Dictionary. Demonstrates indexer,
    /// enumerator, collection initiazer.
    /// </summary>
    class CustomDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> inner = new Dictionary<string, string>();

        public string this[string key]
        {
            get { return inner[key]; }
            set { inner[key] = value; }
        }

        public void Add(string key, string value) => inner.Add(key, value);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(',', inner.Select(x => $"\"{x.Key}\":\"{x.Value}\""));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
