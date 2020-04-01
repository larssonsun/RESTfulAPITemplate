#if (RESTFULAPIHELPER)

using Larsson.RESTfulAPIHelper.Interface;

#endif

namespace RESTfulAPITemplate.Core.Entity
{
    public class Entity 
    
#if (RESTFULAPIHELPER)

    : IEntity

#endif

    {
        public System.Guid Id { get; set; }
    }
}