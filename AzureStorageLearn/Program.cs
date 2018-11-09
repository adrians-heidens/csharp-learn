using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureStorageLearn
{
    /// <summary>
    /// Example data class.
    /// </summary>
    class Resource
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int Size { get; set; }

        public override string ToString()
        {
            return $"Resource(Type={Type}, Name={Name}, Url={Url}, Size={Size})";
        }
    }

    /// <summary>
    /// Example Azure storage table entity. Shows an example how to store list of values.
    /// </summary>
    class CustomerEntity : TableEntity
    {
        private IList<string> _images = new List<string>();

        private IList<Resource> _resources = new List<Resource>();

        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public CustomerEntity() { }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ImagesRaw { get; set; } = "[]";

        public string ResourcesRaw { get; set; } = "[]";

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

        public void AddResource(Resource resource)
        {
            _resources.Add(resource);
            ResourcesRaw = JsonConvert.SerializeObject(_resources);
        }

        public void ClearResources(Resource resource)
        {
            _resources.Clear();
            ResourcesRaw = JsonConvert.SerializeObject(_resources);
        }

        [IgnoreProperty]
        public IList<Resource> Resources
        {
            get
            {
                return JsonConvert.DeserializeObject<IList<Resource>>(ResourcesRaw);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Get a handle to storage and table (connection to emulator, which
            // should be run separately).
            //string storageConnectionString =
            //    "DefaultEndpointsProtocol=http;" +
            //    "AccountName=devstoreaccount1;" +
            //    "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
            //    "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;" +
            //    "TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;" +
            //    "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";
            string storageConnectionString = "UseDevelopmentStorage=true"; // Shorter version of emulator usage.
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

            //customer.AddResource(ResourceType.Mp3, "https://example.test/some.mp3");

            customer.AddResource(new Resource
            {
                Name = "Theme Song",
                Type = "mp3",
                Url = "https://example.test/song.mp3",
                Size = 156,
            });
            customer.AddResource(new Resource
            {
                Name = "3D model",
                Type = "fbx",
                Url = "https://example.test/3d.fbx",
                Size = 356,
            });

            TableOperation insertOperation = TableOperation.InsertOrReplace(customer);

            table.ExecuteAsync(insertOperation).Wait();

            // Query.
            TableOperation tableOperation = TableOperation.Retrieve<CustomerEntity>("Doe", "John");
            TableResult tableResult = table.ExecuteAsync(tableOperation).Result;

            var customer2 = (CustomerEntity) tableResult.Result;
            Console.WriteLine("Images:");
            foreach (var image in customer2.Images)
            {
                Console.WriteLine($"  {image}");
            }

            Console.WriteLine("Resources:");
            foreach (var resource in customer2.Resources)
            {
                Console.WriteLine($"  {resource}");
            }

            var ent = new CustomerEntity();
            Console.WriteLine(ent.Images.Count);

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
