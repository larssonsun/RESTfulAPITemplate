#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif
namespace RESTfulAPISample.Core.DTO
{
#if (DISTRIBUTEDCACHE)
    [MessagePackObject]
#endif
    public class ProductDTO
    {
#if (DISTRIBUTEDCACHE)
        [Key(0)]
#endif
        public System.Guid Id { get; set; }
#if (DISTRIBUTEDCACHE)
        [Key(1)]
#endif
        public string Name { get; set; }
#if (DISTRIBUTEDCACHE)
        [Key(2)]
#endif
        public string Description { get; set; }
#if (DISTRIBUTEDCACHE)
        [Key(3)]
#endif
        public bool IsOnSale { get; set; }
#if (DISTRIBUTEDCACHE)
        [Key(4)]
#endif
        public string CreateTime { get; set; }
    }
}