using System;

namespace JsonNetParse.ObjectPropertySetter
{
    class Person
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public int Score { get; set; }

        public DateTime BirthDateTime { get; set; }

        public Person(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public override string ToString()
        {
            return $"Person(\n" +
                $"  Email={Email},\n" +
                $"  Password={Password},\n" +
                $"  Score={Score},\n" +
                $"  BirthDateTime={BirthDateTime})";
        }
    }
}
