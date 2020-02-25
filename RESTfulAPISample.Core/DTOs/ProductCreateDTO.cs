namespace RESTfulAPISample.Core.DTO
{
    public class ProductCreateDTO : ProductCreateOrUpdateDTO
    {
        public System.Guid Id { get; set; }
    }
}