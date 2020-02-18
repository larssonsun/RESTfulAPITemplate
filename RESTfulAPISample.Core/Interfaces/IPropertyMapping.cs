using System.Collections.Generic;
using RESTfulAPISample.Core.Sort;

namespace RESTfulAPISample.Core.Interface
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}