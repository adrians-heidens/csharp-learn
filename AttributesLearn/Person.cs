namespace AttributesLearn
{
    [Entity]
    class Person
    {
        public string Name { get; set; }

        public string Fullname { get; set; }

        public string Password { get; set; }

        public override string ToString()
        {
            return $"Person(Name='{Name}')";
        }
    }
}
