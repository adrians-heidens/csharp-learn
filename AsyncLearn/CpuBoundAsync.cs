using System;
using System.Threading.Tasks;

namespace AsyncLearn
{
    static class CpuBoundAsync
    {
        public static void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var task = Task.Run(() => CalculateSomething());
            // Do something in between.
            var result = await task;
            Console.WriteLine($"Result: {result}");
        }

        private static string CalculateSomething()
        {
            // CPU intensive operations.
            return "Something";
        }
    }
}
