using System.Collections.Generic;
using Larsson.RESTfulAPIHelper.SortAndQuery;

namespace Larsson.RESTfulAPIHelper.Interface
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}