using System;

namespace RESTfulAPISample.Core.DomainModel
{
    public class Product
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime CreateTime { get; set; }
    }
}