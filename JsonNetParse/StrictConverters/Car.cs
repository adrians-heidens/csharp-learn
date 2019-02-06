namespace JsonNetParse.StrictConverters
{
    class Car
    {
        public string Model { get; set; }

        public decimal Length { get; set; }

        public override string ToString()
        {
            return $"Car(Model={Model}, Length={Length})";
        }
    }
}
