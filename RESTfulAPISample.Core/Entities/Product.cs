using System;
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPISample.Core.Entity
{
#if (DISTRIBUTEDCACHE)
    [MessagePackObject(keyAsPropertyName: true)]
#endif
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime CreateTime { get; set; }
    }
}