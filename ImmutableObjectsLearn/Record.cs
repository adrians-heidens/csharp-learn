namespace ImmutableObjectsLearn
{
    /// <summary>
    /// A simple class which encapsulates primitive values.
    /// </summary>
    class Record
    {
        
        /// <remarks>
        /// for small classes or structs that just encapsulate a set of values
        /// (data) and have little or no behaviors, you should either make the
        /// objects immutable by declaring the set accessor as private
        /// (immutable to consumers) or by declaring only a get accessor
        /// (immutable everywhere except the constructor).
        /// </remarks>

        public string Name { get; }

        public uint Ttl { get; }

        public Record(string name, uint ttl)
        {
            Name = name;
            Ttl = ttl;
        }

        public override string ToString()
        {
            return $"Record(Name='{Name}', Ttl={Ttl})";
        }
    }
}
