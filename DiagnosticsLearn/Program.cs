using System;
using System.Diagnostics;

namespace DiagnosticsLearn
{
    public class SomeTraceListener : TraceListener
    {
        private readonly string _prefix;
        
        public SomeTraceListener(string prefix)
        {
            _prefix = prefix;
        }
        
        public override void Write(string message)
        {
            Console.Write(_prefix + message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(_prefix + message);
        }
    }
    
    /// <summary>
    /// Demonstrates Trace listener usage.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new SomeTraceListener(">>> "));
            Trace.WriteLine("Hello trace message!");
            
        }
    }
}