namespace ImmutableObjectsLearn
{
    /// <summary>
    /// A simple class which encapsulates primitive values.
    /// </summary>
    class Header
    {
        public ushort Id { get; }

        public ushort RecordCount { get; }

        public Header(ushort id, ushort recordCount)
        {
            Id = id;
            RecordCount = recordCount;
        }

        public override string ToString()
        {
            return $"Header(Id={Id}, RecordCount={RecordCount})";
        }
    }
}
