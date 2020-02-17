using System;

namespace RESTfulAPISample.Core.Entity
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime CreateTime { get; set; }
    }
}