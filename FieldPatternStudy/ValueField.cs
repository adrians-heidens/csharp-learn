namespace FieldPatternStudy
{
    /// <summary>
    /// A generic type of field which wraps any value T.
    /// </summary>
    public class ValueField<T> : IField
    {
        public dynamic Value { get; set; }

        public ValueField(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"ValueField<{typeof(T)}>({Value})";
        }
    }
}
