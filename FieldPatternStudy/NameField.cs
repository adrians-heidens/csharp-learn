namespace FieldPatternStudy
{
    /// <summary>
    /// A special field which wraps string value.
    /// </summary>
    public class NameField : ValueField<string>
    {
        public NameField(string value) : base(value)
        {
        }

        public override string ToString()
        {
            return $"NameField('{Value}')";
        }
    }
}
