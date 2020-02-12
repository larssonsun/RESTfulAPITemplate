namespace RESTfulAPISample.Api.Resource
{
    public class ProductCreateDTO
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
    }
}