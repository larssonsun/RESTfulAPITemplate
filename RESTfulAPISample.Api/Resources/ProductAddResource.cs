namespace RESTfulAPISample.Api.Resource
{
    public class ProductAddResource
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
    }
}