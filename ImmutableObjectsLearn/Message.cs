using System.Collections.Generic;

namespace ImmutableObjectsLearn
{
    /// <summary>
    /// A class which encapsulates complex objects.
    /// </summary>
    class Message
    {
        public Header Header { get; set; }

        public IList<Record> Records { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public Message(Header header, IEnumerable<Record> records)
        {
            Header = header;

            // Create a copy of list. Can keep the same items or clone them as
            // well. In this case let's keep them as they should be immutable.
            var recordList = new List<Record>();
            foreach (var record in records)
            {
                recordList.Add(record);
            }
            Records = recordList.AsReadOnly(); // Expose readonly interface.
        }

        public override string ToString()
        {
            return $"Message(Id={Header.Id}, Records.Count={Records.Count})";
        }
    }
}
