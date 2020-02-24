using System;
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPISample.Core.Entity
{
#if (DISTRIBUTEDCACHE)

    [MessagePackObject(keyAsPropertyName: true)]

#endif



    public class Product

#if (RESTFULAPIHELPER)

    : Entity

#endif

    {

#if (!RESTFULAPIHELPER)

        public Guid Id { get; set; }

#endif

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime CreateTime { get; set; }
    }
}