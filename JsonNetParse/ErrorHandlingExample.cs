using Newtonsoft.Json;
using System;

namespace JsonNetParse
{
    class Article
    {
        public string Title { get; set; }

        public bool? IsPublished { get; set; }

        public override string ToString()
        {
            return $"Article(Title={Title}, IsPublished={IsPublished})";
        }
    }
    
    static class ErrorHandlingExample
    {
        public static void Run()
        {
            var obj = new Article { Title = "Foo", IsPublished = false };
            var json = JsonConvert.SerializeObject(obj);
            Console.WriteLine(json);

            json = "{\"Title\":\"Bar\",\"IsPublished\":true}";
            obj = JsonConvert.DeserializeObject<Article>(json);
            Console.WriteLine(obj);

            // Boolean field as string value.
            json = "{\"Title\":\"Spam\",\"IsPublished\":\"true\"}";
            obj = JsonConvert.DeserializeObject<Article>(json);
            Console.WriteLine(obj);

            // Wrong data type on field IsPublished.
            // Add settings to just print the error and move on.
            var settings = new JsonSerializerSettings { Error = (se, ev) => {
                var currentError = ev.ErrorContext.Error.Message;
                Console.WriteLine($"Error: {currentError}");
                ev.ErrorContext.Handled = true;
            }};
            json = "{\"Title\":\"Spam\",\"IsPublished\":\"1\"}";
            obj = JsonConvert.DeserializeObject<Article>(json, settings);
            Console.WriteLine(obj);
        }
    }
}
