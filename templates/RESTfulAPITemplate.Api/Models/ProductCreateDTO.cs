namespace RESTfulAPITemplate.App.Model
{
    public class ProductCreateDTO : ProductCreateOrUpdateDTO
    {
        public System.Guid Id { get; set; }
    }
}