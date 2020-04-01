namespace RESTfulAPITemplate.Core.DTO
{
    public class ProductDTO
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
        public string CreateTime { get; set; }
        public string FullName { get; set; }
    }
}