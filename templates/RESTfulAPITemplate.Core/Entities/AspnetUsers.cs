using System;
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPITemplate.Core.Entity
{
#if (DISTRIBUTEDCACHE)

    [MessagePackObject(keyAsPropertyName: true)]

#endif

    public partial class AspnetUsers

#if (RESTFULAPIHELPER)

    // : Entity

#endif

    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
}
