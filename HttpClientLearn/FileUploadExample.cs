using System;
using System.IO;
using System.Net.Http;

namespace HttpClientLearn
{
    /// <summary>
    /// Example uploading file using HTTP POST request.
    /// </summary>
    static class FileUploadExample
    {
        public static void Run()
        {
            HttpClient httpClient = new HttpClient() {
                BaseAddress = new Uri("http://127.0.0.1:8080"),
            };

            var httpContent = new MultipartFormDataContent();
            var fileStream = File.Open(@"C:\tmp\sample.txt", FileMode.Open);
            httpContent.Add(new StreamContent(fileStream), "file", "data.txt");

            var response = httpClient.PostAsync("/file-upload", httpContent).Result;
            Console.WriteLine(response);
        }
    }
}
