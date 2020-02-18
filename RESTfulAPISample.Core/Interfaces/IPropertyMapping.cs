using System.Collections.Generic;
using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Core.Interface
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}