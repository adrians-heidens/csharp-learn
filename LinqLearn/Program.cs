using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLearn
{
    class User
    {
        public User(string username)
        {
            Username = username;
        }

        public User()
        {
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public override string ToString()
        {
            return $"User(Username='{Username}', Email='{Email}')";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // A linq query.
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            var numQuery = from num in numbers where (num % 2) == 0 select num;
            foreach (int num in numQuery)
            {
                Console.Write("{0,1} ", num);
            }
            Console.WriteLine();

            // Counting elements.
            Console.WriteLine(numQuery.Count());

            // Convert to array or list.
            var numList = numQuery.ToList();
            Console.WriteLine(string.Join(",", numList));

            // Filtering collection of users.
            var userCollection = new List<User> {
                new User { Username = "foo", Email = "foo@test", City = "Riga" },
                new User { Username = "bar", Email = "bar@example", City = "Oslo" },
                new User { Username = "spam", Email = "spam@test", City = "Riga" },
                new User { Username = "eggs", Email = "eggs@test", City = "Stockholm" },
            };
            var q = from user in userCollection where user.Username.Length == 4 select user;
            foreach (var user in q)
            {
                Console.WriteLine(user);
            }

            // Features of LINQ:
            // Data source.
            // Filtering.
            // Ordering.
            // Grouping.
            // Joining.
            // Selecting (Projections).

            // Grouping users. This returns list of lists.
            var userGoupQuery = from user in userCollection group user by user.City;
            foreach (var userGroup in userGoupQuery)
            {
                Console.WriteLine(userGroup.Key);
                foreach (var user in userGroup)
                {
                    Console.WriteLine("  {0}", user);
                }
            }

            var usernameList = new List<string> { "foo", "bar", "eggs" };

            var newUsers = from name in usernameList select new User { Username = name };
            foreach (var user in newUsers)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
