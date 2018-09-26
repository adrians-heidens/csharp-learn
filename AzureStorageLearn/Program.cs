using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureStorageLearn
{

    /// <summary>
    /// Example Azure storage table entity. Shows an example how to store list of values.
    /// </summary>
    class CustomerEntity : TableEntity
    {
        private IList<string> _images = new List<string>();

        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public CustomerEntity() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ImagesRaw { get; set; } = "[]";

        public void AddImage(string url)
        {
            _images.Add(url);
            ImagesRaw = JsonConvert.SerializeObject(_images);
        }

        public void RemoveImage(string url)
        {
            _images.Remove(url);
            ImagesRaw = JsonConvert.SerializeObject(_images);
        }

        public void ClearImages()
        {
            _images.Clear();
            ImagesRaw = JsonConvert.SerializeObject(_images);
        }

        [IgnoreProperty]
        public IList<string> Images
        {
            get
            {
                return JsonConvert.DeserializeObject<IList<string>>(ImagesRaw);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Get a handle to storage and table (connection to emulator, which
            // should be run separately).
            string storageConnectionString =
                "DefaultEndpointsProtocol=http;" +
                "AccountName=devstoreaccount1;" +
                "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
                "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;" +
                "TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;" +
                "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();

            // Create table if doesn't exist.
            CloudTable table = tableClient.GetTableReference("people");

            table.CreateIfNotExistsAsync().Wait();

            // Insert or replace.
            CustomerEntity customer = new CustomerEntity("Doe", "John");
            customer.Email = "john@example.test";
            customer.PhoneNumber = "12345678";
            customer.AddImage("https://example.test/image1.png");
            customer.AddImage("https://example.test/image2.png");

            TableOperation insertOperation = TableOperation.InsertOrReplace(customer);

            table.ExecuteAsync(insertOperation).Wait();

            // Query.
            TableOperation tableOperation = TableOperation.Retrieve<CustomerEntity>("Doe", "John");
            TableResult tableResult = table.ExecuteAsync(tableOperation).Result;

            var customer2 = (CustomerEntity) tableResult.Result;
            foreach (var image in customer2.Images)
            {
                Console.WriteLine(image);
            }

            var ent = new CustomerEntity();
            Console.WriteLine(ent.Images.Count);

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
