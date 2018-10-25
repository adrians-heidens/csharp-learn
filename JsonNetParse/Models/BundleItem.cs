namespace JsonNetParse.Models
{
    public class BundleItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"BundleItem(Id={Id}, Quantity={Quantity})";
        }
    }
}
