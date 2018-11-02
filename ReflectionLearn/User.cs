namespace ReflectionLearn
{
    /// <summary>
    /// An example of class for user data.
    /// </summary>
    class User
    {
        /// <summary>
        /// User name in the system.
        /// </summary>
        public string Name { get; set; }

        public int Level { get; set; } = 0;

        public User(string name)
        {
            Name = name;
        }
    }
}
